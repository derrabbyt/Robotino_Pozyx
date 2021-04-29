using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tryudp2
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigServer();
        }

        private static void ConfigServer()
        {

            IPAddress udp_ip = IPAddress.Parse("127.0.0.1");
            int udp_port_emission = 9180;
            IPEndPoint ipe = new IPEndPoint(udp_ip, udp_port_emission);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.Connect(ipe);

            byte[] message = getMessageToSend(1, 0, 1, 0, 1, 1, 0, 0,1);

            s.SendTo(message, 0, message.Length, SocketFlags.None, ipe);
            s.Close();
        }
    }
}
