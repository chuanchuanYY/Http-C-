using Http.Core.Common;

namespace Http.Core.Http;


public enum HttpMethod 
{
    GET,
    POST,
}
public sealed class HttpMethodConverter
{
    private HttpMethodConverter()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public  static HttpMethod GetMethod(string msg)
    {
        ArgumentNullException.ThrowIfNull(msg);
        switch (msg)
        {
            case "POST": 
                 return HttpMethod.POST;  
            case "GET": 
                 return HttpMethod.GET;
            default : 
            throw new ArgumentException(ErrorMessages.UNKNOWN_HTTP_METHOD,nameof(msg));
        }
    }
}