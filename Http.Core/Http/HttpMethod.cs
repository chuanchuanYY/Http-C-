namespace Http.Core.http;


public enum HttpMethod 
{
    GET,
    POST,
}
public static class HttpMethodsExtension
{
    public static HttpMethod IntoHttpMethod(this string msg)
    {
        switch (msg)
        {
            case "POST": 
                 return HttpMethod.POST;  
            case "GET": 
                 return HttpMethod.GET;
            default : 
            throw new ArgumentException("unknown http Methods" + msg);
        }
    }
}