using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        public Request(System.IO.Stream networkstream)
        {
            tokens = new Dictionary<string, string>();
            StreamReader sr = new StreamReader(networkstream);
            string line;

            while(!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                _header += line + "\n";
            }
        }

        public void SplitHeader()
        {
            string[] splitHeader = { "" };
            string[] splitPair;
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
            }
        }

        public bool ValidateHeader()
        {
            string[] validMethods = { "GET", "HEAD", "POST", "PUT", "DELETE", "CONNECT", "OPTIONS", "TRACE", "PATCH"};

            if (validMethods.Any(_method.ToUpper().Contains))
            {
                return this.valid = true;
            }
            else
            {
                return this.valid = false;
            }
        }
    }
}
