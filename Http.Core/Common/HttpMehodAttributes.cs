namespace Http.Core.Common;


[AttributeUsage(AttributeTargets.Method)]
public class HttpMethodBase  : Attribute 
{
    public string Name { get; set; }
    public HttpMethodBase(string name)
    {
        Name = name;
    }
}

public class HttpPost : HttpMethodBase
{
    public HttpPost(string name) : base(name)
    {
    }
}

public class HttpGet : HttpMethodBase
{
    public HttpGet(string name) : base(name)
    {
    }
}