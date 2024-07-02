using System.Net.Sockets;
using System.Reflection;
using Http.Core.Common;
using Http.Core.http;
using Microsoft.Extensions.DependencyInjection;

namespace Http.Core;


public class HttpServer 
{

    private static readonly ServiceCollection _services = new ServiceCollection();
    public static ServiceCollection GetServices()
    {
        return _services;
    }
    public void Run(HttpServerOption option)
    {
        using TcpListener tcpListener =
        new TcpListener(option.IPAddress,option.Port);
        try
        {
            tcpListener.Start();
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                // handle each client connection 

                ThreadPool.QueueUserWorkItem((_)=>{
                    new ConnectionHandle(client.GetStream());
                });
            }
        }
        catch
        {
            throw;
        }
        finally{
            tcpListener.Stop();
        }
        
    }
  
}