using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace swe1kommexam.classes
{
    class Request
    {
        private string _header = "";
        private string _method;
        public string method { get { return _method; } }
        private string _url;
        public Dictionary<string, string> tokens;
        public bool valid;
        public string url { get{ return _url; } }
        public Request(NetworkStream networkstream)
        {
            tokens = new Dictionary<string, string>();
            if (networkstream.DataAvailable ==  true)
            {
                StreamReader sr = new StreamReader(networkstream);
                string line;
                while(!String.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    _header += line + "\n";
                }
            }
        }

        public void SplitHeader()
        {
            string[] splitHeader = { "" };
            string[] splitPair;
            Thread thread = Thread.CurrentThread;
            if (!String.IsNullOrEmpty(_header))
            {
                splitHeader = _header.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                splitPair = splitHeader[0].Split(' ', StringSplitOptions.None);
                _method = splitPair[0];
                _url = splitPair[1];
                foreach (string item in splitHeader.Skip(1))
                {
                    splitPair = item.Split(": ", StringSplitOptions.RemoveEmptyEntries);
                    tokens.Add(splitPair[0], splitPair[1]);
                }
                Console.WriteLine("thread" + thread.ManagedThreadId + ": header not empty and split");
            }
            else
            {
                Console.WriteLine("thread" + thread.ManagedThreadId + ": header empty");
            }
        }

        public bool ValidateHeader()
        {
            string[] validMethods = { "GET", "HEAD", "POST", "PUT", "DELETE", "CONNECT", "OPTIONS", "TRACE", "PATCH"};
            Thread thread = Thread.CurrentThread;

            if (!String.IsNullOrEmpty(_header) && validMethods.Any(_method.ToUpper().Contains))
            {
                Console.WriteLine("thread" + thread.ManagedThreadId + ": header valid");
                return this.valid = true;
            }
            else
            {
                Console.WriteLine("thread" + thread.ManagedThreadId + ": header not valid");
                return this.valid = false;
            }
        }
    }
}
