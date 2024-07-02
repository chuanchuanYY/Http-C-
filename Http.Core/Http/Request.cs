
using System.Collections;

namespace Http.Core.http;


public partial class Request 
{
    public HttpMethod Method { get; set; }
    // path also named url
    public string Path { get; set; }
    public HttpVersion HttpVersion{ get; set; }
    public Dictionary<string,string> Headers { get; set; }
    public List<string> Body { get; set; }

    
}

public partial class Request 
{
    public static Request Parse(List<string> message)
    {
        
        string[] startLine = message[0].Split(" ");
        Dictionary<string, string> headers = new Dictionary<string, string> ();
        List<string> body = new List<string> ();
        
        IEnumerator messageEnumerator =message.GetEnumerator();
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

        return new Request{
            Method = startLine[0].IntoHttpMethod(),
            Path = startLine[1],
            HttpVersion = startLine[2].IntoHttpVersion(),
            Headers = headers,
            Body = body
        };
       
    }
}