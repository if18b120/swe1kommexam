using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace swe1kommexam.classes
{
    class Response
    {
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        public string status;
        public string serverHeader = "SWE1-KOMM-Server";
        public string content;

        public void AddHeader(string header, string value)
        {
            if (_headers.ContainsKey(header))
            {
                _headers[header] = value;
            }
            else
            {
                _headers.Add(header, value);
            }
        }

        public void Send(System.IO.Stream networkstream)
        {
            Thread thread = Thread.CurrentThread;
            StreamWriter sw = new StreamWriter(networkstream, Encoding.UTF8);

            sw.WriteLine("HTTP/1.1 " + status);
            sw.WriteLine("Server: " + serverHeader);

            foreach (var (key, value) in _headers)
            {
                sw.WriteLine(key + ": " + value);
            }
            sw.WriteLine();
            try
            {
                sw.Flush();
            }
            catch (IOException e)
            {
                Console.WriteLine("    thread" + thread.ManagedThreadId + ": exception " + e);
            }
            Console.WriteLine("thread" + thread.ManagedThreadId + ": header sent");
            sw.Write(content);
            try
            {
                sw.Flush();
            }
            catch (IOException e)
            {
                Console.WriteLine("    thread" + thread.ManagedThreadId + ": exception " + e);
            }
            
            Console.WriteLine("thread" + thread.ManagedThreadId + ": body sent");
            sw.Close();
        }
    }
}
