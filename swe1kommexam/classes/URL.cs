using System;
using System.Collections.Generic;
using System.Text;

namespace swe1kommexam.classes
{
    class URL
    {
        private string _rawUrl;
        private string _path;
        public string path { get { return _path; } }

        public URL(string url)
        {
            _rawUrl = url ?? "";
        }

        public void CreatePathFromURL()
        {
            if (_rawUrl.IndexOf('?') != -1)
            {
                _path = _rawUrl.Substring(0, _rawUrl.IndexOf('?'));
            }
            else if (_rawUrl.IndexOf('#') != -1)
            {
                _path = _rawUrl.Substring(0, _rawUrl.IndexOf('#'));
            }
            else
            {
                _path = _rawUrl;
            }
            if (_path == "/")
            {
                _path += "index.html";
            }
        }

        public string GetContentType()
        {
            string[] segments = _path.Split("/", StringSplitOptions.None);
            segments = segments[segments.Length - 1].Split(".", StringSplitOptions.None);

            if (segments.Length == 2)
            {
                return segments[1];
            }
            else
            {
                return "unknown";
            }
        }
    }
}
