using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Http.Core.Common;
using Http.Core.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Http.Core;

public class RouteMethod 
{
    public Type ClassType { get; set; } = null!;
    public MethodInfo Method { get; set; } = null!;
}
public class Route 
{
    private List<Assembly> _loadAssembly = new List<Assembly>();
    private Dictionary<string,RouteMethod> _route = new Dictionary<string, RouteMethod>(); 
  
    private  ServiceProvider _serviceProvider ;
   
    public Route()
    {
        Load();
    }

    public Route(params Assembly[] assemblies)
    {
        _loadAssembly.AddRange(assemblies);
        Load();
    }

    public Dictionary<string,RouteMethod> GetRoutes()
    {
        return new Dictionary<string, RouteMethod>(_route);
    }
    
    private void Load()
    {
        _loadAssembly.ForEach(LoadFromAssembly);
    }
    private void LoadFromAssembly(Assembly assembly)
    {
        // 加载 程序集中添加了Http 属性的类
        // 并将结果缓存
        assembly.GetTypes()
        .ToList()
        .ForEach(t=>{
            var httpAttribute = t.GetCustomAttribute<HttpAttribute>();
            if(t.IsClass &&  httpAttribute!= null)
            {
                string uri ="";
                if(httpAttribute.BaseUri!=null)
                {
                    uri += httpAttribute.BaseUri;
                }

                t.GetMethods()
                .ToList()
                .ForEach(m=>{
                    IEnumerable<HttpMethodBase>attrs=
                    m.GetCustomAttributes()
                    .Where(a=>a.GetType()
                    .IsAssignableTo(typeof(HttpMethodBase)))
                    .Cast<HttpMethodBase>();

                    foreach(HttpMethodBase attr in attrs)
                    {
                        _route.Add(uri+attr.Name,new RouteMethod{
                            Method = m,
                            ClassType = t,
                        });
                    }
                });
            }
        });

        RegisterRouteMthod();
    }
    private void RegisterRouteMthod()
    {
        ServiceCollection services =HttpServer.GetServices();
        foreach(RouteMethod routeMethod in  _route.Values)
        {
            services.AddScoped(routeMethod.ClassType);
        }
        _serviceProvider = services.BuildServiceProvider();
    }

    /// <summary>
    /// 调用方法，通过指定的路由地址
    /// </summary>
    /// 
    public  void InvokeMethodByRoute(Request request)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(request.Path);

        if(!_route.ContainsKey(request.Path))
        {
            throw new ArgumentException("route没有指定的path ");
        }

        RouteMethod routeMethod = _route[request.Path];
        object? instance = _serviceProvider.GetRequiredService(routeMethod.ClassType);
        if(instance == null)
        {
            throw new Exception("创建Type 实例失败");
        }

        // 如果是 HttpController 的子类
        if(routeMethod.ClassType.IsAssignableTo(typeof(HttpController)))
        {
           var _controller = (HttpController)instance;
           _controller.Context = new HttpContext
           {
             request = request,
           };
        }


        if(request.QueryParames.Count == 0 )
        {
            routeMethod.Method.Invoke(instance, new object[] {  });
        }
        else 
        {
            // 获取方法的参数信息
            ParameterInfo[] parameterInfo = routeMethod.Method.GetParameters();
            object[] parames = new object[parameterInfo.Length];
           
           try
           {
                if(parameterInfo.Length == request.QueryParames.Count)
                {
                    for(int i=0; i<parameterInfo.Length;i++)
                    {
                        parames[i] =  Convert.ChangeType(request.QueryParames[parameterInfo[i].Name!],parameterInfo[i].ParameterType);
                    }
                    routeMethod.Method.Invoke(instance, parames);
                }
           }
           catch
           {
             throw;
           }
        }
    }

}