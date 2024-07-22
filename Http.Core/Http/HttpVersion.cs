

using Http.Core.Common;

namespace Http.Core.Http;

public enum HttpVersion 
{
    HTTP1_1,
}
public sealed class HttpVersionConverter
{
    private HttpVersionConverter()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static HttpVersion GetHttpVersion(string version)
    {
        ArgumentNullException.ThrowIfNull(version);
        switch (version)
        {
            case "HTTP/1.1":
                 return HttpVersion.HTTP1_1;
            default : 
            throw new ArgumentException(ErrorMessages.UNKNOWN_HTTP_VERSION, nameof(version));
        }
    }
}