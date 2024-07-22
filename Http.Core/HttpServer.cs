using System.Net.Sockets;
using System.Reflection;
using Http.Core.Common;
using Http.Core.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Http.Core;


public class HttpServer 
{
    private static readonly ServiceCollection _services = new ServiceCollection();
    
    private int _errorRestartTime = 0;
    public int MaxErrorRestartTime {get;set;} = 3;

    
    public static ServiceCollection GetServices()
    {
        return _services;
    }
    public void Run(HttpServerOption option)
    {
        ArgumentNullException.ThrowIfNull(option, "option");
        if(option.IPAddress == null )
        {
            throw new ArgumentNullException(ErrorMessages.OPTION_VALUE_NULL);
        }
        Restart:
        try
        {
            using TcpListener tcpListener = new TcpListener(option.IPAddress,option.Port);
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
            if(_errorRestartTime > MaxErrorRestartTime)
            {
                throw;
            }
            goto Restart;
        }
        
    }
  
}