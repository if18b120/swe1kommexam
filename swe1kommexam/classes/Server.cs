using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace swe1kommexam.classes
{
    class Server
    {
        private int _port;
        TcpListener _listener;

        public Server(int port = 12345)
        {
            this._port = port;
            _listener = new TcpListener(IPAddress.Any, _port);
        }

        public void Run()
        {
            Console.WriteLine("server: starting");
            string input = "";
            Thread inputThread = new Thread(() => { input = Console.ReadLine(); });
            inputThread.Start();
            _listener.Start();
            Console.WriteLine("server: input thread and listener started");

            while(input != "x")
            {
                if (_listener.Pending())
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(client);
                    Console.WriteLine("server: client connected, thread " + clientThread.ManagedThreadId + " started");
                }
            }
            _listener.Stop();
            Console.WriteLine("server: shutting down");
        }

        private void HandleClient(object clientarg)
        {
            Thread thread = Thread.CurrentThread;
            TcpClient client = (TcpClient)clientarg;
            Request request = new Request(client.GetStream());
            request.SplitHeader();

            if (request.ValidateHeader())
            {
                URL url = new URL(request.url);
                url.CreatePathFromURL();
                Response response = new Response();

                if (request.method.ToUpper() == "GET" && url.GetContentType() == "html" && File.Exists("./" + url.path))
                {
                    response.status = "200 OK";
                    response.content = File.ReadAllText("./" + url.path);
                    response.AddHeader("Content-Length", Encoding.UTF8.GetByteCount(response.content).ToString());
                    response.AddHeader("Connection", "close");
                    response.AddHeader("Content-type", "text/html");
                    response.Send(client.GetStream());
                }
                else
                {
                    response.status = "404 Not Found";
                    response.content = "";
                    response.AddHeader("Connection", "close");
                    response.AddHeader("Content-type", "text/html");
                    response.Send(client.GetStream());
                }
            }
            client.Close();
            
            Console.WriteLine("thread" + thread.ManagedThreadId + ": closing");
        }
    }
}
