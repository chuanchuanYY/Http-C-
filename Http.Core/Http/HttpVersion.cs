

namespace Http.Core.http;

public enum HttpVersion 
{
    HTTP1_1,
}
public static class HttpVersionExtensions
{
    public static HttpVersion IntoHttpVersion(this string version)
    {
        switch (version)
        {
            case "HTTP/1.1":
                 return HttpVersion.HTTP1_1;
            default : 
            throw new ArgumentException("unknown Http version ", "version");
        }
    }
}