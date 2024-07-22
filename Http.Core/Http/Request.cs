
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Http.Core.Common;

namespace Http.Core.Http;



public partial class Request 
{
    public HttpMethod Method { get; set; }
    // path also called url
    public string Path { get; set; }
    public HttpVersion HttpVersion{ get; set; }
    public Dictionary<string,string> Headers { get; set; }
    public List<string> Body { get; set; }

    public Dictionary<string,string> QueryParames;

   
    
}

public partial class Request 
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Request Parse(List<string> message)
    {
        ArgumentNullException.ThrowIfNull(message);

        string[] startLine = message[0].Split(" ");
        Dictionary<string, string> headers = new Dictionary<string, string> ();
        List<string> body = new List<string> ();
        
        IEnumerator messageEnumerator =message.GetEnumerator();
        try{
            messageEnumerator.MoveNext();
            while(messageEnumerator.MoveNext())
            {
                string line = (string)messageEnumerator.Current;
                if(!String.IsNullOrEmpty(line))
                {
                    string[] parts = line.Split(':',2);
                    headers.Add(parts[0], parts[1]);
                }
                else 
                {
                    break;
                }
            }

            
            while(messageEnumerator.MoveNext())
            {
                body.Add ((string)messageEnumerator.Current);
            }
            var splitResult = startLine[1].Split("?",2);
            // 有查询参数
            Dictionary<string,string> queryParames =new ();
            if(splitResult.Length == 2 )
            {
                queryParames = ParseQueryParameters(splitResult[1]);
            }
            return new Request{
                Method = HttpMethodConverter.GetMethod(startLine[0]),
                Path = splitResult[0],
                HttpVersion = HttpVersionConverter.GetHttpVersion(startLine[2]),
                Headers = headers,
                Body = body,
                QueryParames = queryParames 
            };
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidOperationException(ErrorMessages.PARSE_ERROR,e);
        }
        catch
        {
            throw;
        }
       
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paramStr"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static Dictionary<string,string> ParseQueryParameters(string paramStr)
    {
        ArgumentNullException.ThrowIfNull(paramStr);
        Dictionary<string,string> queryParams = new ();
        foreach (var item in paramStr.Split("&"))
        {
            if(!item.Contains("="))
            {
                throw new ArgumentException(ErrorMessages.STRING_FORMAT_ERROR,nameof(paramStr));
            }
            var splitResult = item.Split("=");
            var key = splitResult[0];
            var value = splitResult[1];
            queryParams.Add(key, value);
        }
        return queryParams;
    }
}