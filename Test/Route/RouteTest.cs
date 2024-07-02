

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

        route.InvokeMethodByRoute("/GetMethod");
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