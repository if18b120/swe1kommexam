using swe1kommexam.classes;
using System;
using System.Threading;

namespace swe1kommexam
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
        }
    }
}
