

using System.Net.Sockets;
using System.Text;
using Http.Core.Http;

namespace Http.Core;


public class ConnectionHandle
{
    private NetworkStream _stream;
    private  Request _request;
    public ConnectionHandle(NetworkStream stream)
    {
        _stream = stream;
        Handle();
    }

    private void Handle()
    {
        ReadData();
        // to do ...
        // 通过反射获取匹配路径的方法，然后调用。
        // 将调用的结果返回
        // and then respons data;
    }
   
    private void ReadData()
    {
        try
        {
            // read a message from the stream 
            if(_stream.CanRead)
            {
                using StreamReader reader = new StreamReader(_stream);
                List<string> data = new List<string>();
                string? line ;
                while((line=reader.ReadLine())!=null)
                {
                    data.Add(line);
                }
                // parse the http massage 
                _request = Request.Parse(data);
            }

        }
        catch
        {
            throw;
        }            
    }

    private async Task ReadDataAsync()
    {
        // read a message from the stream 
        if(_stream.CanRead)
        {
            using StreamReader reader = new StreamReader(_stream);
            List<string> data = new List<string>();
            string? line = await reader.ReadLineAsync();
            while(line != null)
            {
                data.Add(line);
                line =await reader.ReadLineAsync();
            }
            // parse the http massage 
           _request = Request.Parse(data);
        }
    }
  

}