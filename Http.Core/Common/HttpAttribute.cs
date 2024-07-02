
using System.Collections.Specialized;

namespace Http.Core.Common;

[AttributeUsage(AttributeTargets.Class)]
public class HttpAttribute :Attribute 
{
    public HttpAttribute()
    {
        
    }
    public HttpAttribute(string baseUri)
    {
        BaseUri = baseUri;
    }
    public string? BaseUri { get; set; }
}