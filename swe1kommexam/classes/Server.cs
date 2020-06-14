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
            _listener.Start();

            while(true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Thread serverThread = new Thread(new ParameterizedThreadStart(HandleClient));
                serverThread.Start(client);
            }
        }

        private void HandleClient(object clientarg)
        {
            TcpClient client = (TcpClient)clientarg;

            Request request = new Request(client.GetStream());
            request.SplitHeader();

            if (request.ValidateHeader())
            {
                URL url = new URL(request.url);
                url.CreatePathFromURL();
                Response response = new Response();

                if (request.method.ToUpper() == "GET")
                {
                    response.status = "200 OK";
                    response.content = File.ReadAllText("./" + url.path);
                    response.AddHeader("Content-Length", Encoding.UTF8.GetByteCount(response.content).ToString());
                    response.AddHeader("Connection", "close");
                    response.AddHeader("Content-type", "text/html");
                    response.Send(client.GetStream());
                }
            }
        }
    }
}
