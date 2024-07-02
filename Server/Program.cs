



using System.Net;
using Http.Core;

Console.WriteLine("OK");

new HttpServer()
.Run(new HttpServerOption{
    Port = 9999,
    IPAddress = IPAddress.Parse("127.0.0.1"),
});