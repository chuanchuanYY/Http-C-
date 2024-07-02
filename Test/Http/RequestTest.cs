


using Http.Core.http;

public class RequestTest
{
    [Test]
    public void TestParseReturnRequest()
    {
        string[] httpMessage = {
              "POST / HTTP/1.1",
              "Host:localhost:8080",
              "",
              "Hello World!"
            };
        Request request = Request.Parse(httpMessage.ToList());
        Assert.NotNull(request);
        Assert.IsTrue(request.Method == Http.Core.http.HttpMethod.POST);
        Assert.IsTrue("localhost:8080".Equals(request.Headers["Host"]));
        Assert.IsTrue("Hello World!".Equals(request.Body[0]));
        Assert.IsTrue(request.Path.Equals("/"));

    }
}