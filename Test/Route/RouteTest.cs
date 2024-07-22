

using Http.Core.Common;

namespace Test.Route;

public class RouteTest
{
    [Test]
    public void TestRouteLoad()
    {
        var route = new Http.Core.Route(this.GetType().Assembly);
        Assert.IsNotNull(route.GetRoutes());
        var routes = route.GetRoutes();
        // classone 
        Assert.IsTrue(routes.ContainsKey("/GetMethod"));
        Assert.IsTrue(routes.ContainsKey("/PostMethod"));
        //classTwo
        Assert.IsTrue(routes.ContainsKey("GetMethod"));
        Assert.IsTrue(routes.ContainsKey("PostMethod"));
        
        // foreach(var key in routes.Keys)
        // {
        //     Console.WriteLine(key);
        // }
        // foreach(var value in routes.Values)
        // {
        //     Console.WriteLine($"class:{value.ClassType.Name} method:{value
        //     .Method.Name}");
        // }
    }

    [Test]
    public void TestInvokeMethod()
    {
        var route = new Http.Core.Route(this.GetType().Assembly);
        Assert.IsNotNull(route.GetRoutes());

        route.InvokeMethodByRoute(new Http.Core.Http.Request{
            Path = "/GetMethod",
            QueryParames = new Dictionary<string, string>()
            
        });
    }

    [Test]
    public void TestInvokeMethodWithQueryParameter()
    {
        var route = new Http.Core.Route(this.GetType().Assembly);
        Assert.IsNotNull(route.GetRoutes());
        Dictionary<string,string> paramers = new Dictionary<string, string>
        {
            { "name", "tom" },
            { "age", "20" }
        };
        route.InvokeMethodByRoute(new Http.Core.Http.Request{
            Path = "/withParames/GetMethod",
            QueryParames = paramers,
        });
    }


    [Test]
    public void TestWithHttpController()
    {
        var route = new Http.Core.Route(this.GetType().Assembly);
        Assert.IsNotNull(route.GetRoutes());

        route.InvokeMethodByRoute(new Http.Core.Http.Request{
            Path = "/WithController/GetMethod",
            QueryParames = new Dictionary<string, string>()
        });
    }
}


[Http(baseUri:"/")]
public class HttpClassOne
{
    [HttpGet(name:"GetMethod")]
    public string GetMethod()
    {
        Console.WriteLine("Get...");
        return "Get";
    }

    [HttpPost(name:"PostMethod")]
    public string PostMethod()
    {
        return "Post";
    }
}
[Http]
public class HttpClassTwo
{
    [HttpGet(name:"GetMethod")]
    public string GetMethod()
    {
        return "Get";
    }

    [HttpPost(name:"PostMethod")]
    public string PostMethod()
    {
        return "Post";
    }
}
// 不采用path.Combind 策略 ，所以路径分隔符得自己组合
[Http(baseUri:"/withParames/")]
public class HttpClassThree
{
    [HttpGet(name:"GetMethod")]
    public string GetMethod(string name,int age)
    {
        Console.WriteLine($"name is :{name} ,next year age is :{age+1} ");
       return name + age.ToString();
    }

    [HttpPost(name:"PostMethod")]
    public string PostMethod()
    {
        return "Post";
    }
}


[Http(baseUri:"/WithController/")]
public class HttpClassWithController:HttpController
{
    [HttpGet(name:"GetMethod")]
    public string GetMethod()
    {
        Console.WriteLine(Context.request.Path);
        return Context.request.Path;
    }
}