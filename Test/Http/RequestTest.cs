


using Http.Core.Http;

public class RequestTest
{
    [Test]
    public void TestParseReturnRequest()
    {
        string[] httpMessage = {
              "POST /?name=tom HTTP/1.1",
              "Host:localhost:8080",
              "",
              "Hello World!"
            };
        Request request = Request.Parse(httpMessage.ToList());
        Assert.NotNull(request);
        Assert.IsTrue(request.Method == Http.Core.Http.HttpMethod.POST);
        Assert.IsTrue("localhost:8080".Equals(request.Headers["Host"]));
        Assert.IsTrue("Hello World!".Equals(request.Body[0]));
        Assert.IsTrue(request.Path.Equals("/"));
        Assert.IsTrue("tom".Equals(request.QueryParames["name"]));
        
        

        // 多个参数参数
        string[] httpMessage2 = {
              "POST /?name=tom&age=18&id=1 HTTP/1.1",
              "Host:localhost:8080",
              "",
              "Hello World!"
            };
        Request request2 = Request.Parse(httpMessage2.ToList());
        Assert.NotNull(request2);
        Assert.IsTrue(request2.Method == Http.Core.Http.HttpMethod.POST);
        Assert.IsTrue("localhost:8080".Equals(request2.Headers["Host"]));
        Assert.IsTrue("Hello World!".Equals(request2.Body[0]));
        Assert.IsTrue(request2.Path.Equals("/"));
        Assert.IsTrue("tom".Equals(request2.QueryParames["name"]));
        Assert.IsTrue(3 == request2.QueryParames.Count);

        // foreach (string key in request2.QueryParames.Keys)
        // {
        //     Console.WriteLine($"key:{key} value:{request2.QueryParames[key]}");
        // }
    }
}