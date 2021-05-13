using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ReceiveTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UDPSocket s = new UDPSocket();
            s.Server("127.0.0.1", 9182);

            Console.ReadKey();
        }

    }


}

