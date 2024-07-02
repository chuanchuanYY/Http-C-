using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Http.Core.Common;
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
        return _route;
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
    public  void InvokeMethodByRoute(string uri)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(uri);

        if(!_route.ContainsKey(uri))
        {
            throw new ArgumentException("route没有指定的Uri ", "uri");
        }

        RouteMethod routeMethod = _route[uri];

        object? instance = _serviceProvider.GetRequiredService(routeMethod.ClassType);
        if(instance == null)
        {
            throw new Exception("创建Type 实例失败");
        }

        // 待做。。。
        // 获取 查询字符串参数：localhost:8080/Some?id=1&value=2
        routeMethod.Method.Invoke(instance, new object[] {  });
    }

}