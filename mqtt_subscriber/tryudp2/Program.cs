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
            SendMessage("172.17.240.174", "1");
        }

        public static bool messageSent = false;

        public static void SendCallback(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)ar.AsyncState;

            Console.WriteLine($"number of bytes sent: {u.EndSend(ar)}");
            messageSent = true;
        }

        static void SendMessage(string server, string message)
        {
            UdpClient u = new UdpClient();

            byte[] sendBytes = Encoding.ASCII.GetBytes(message);

            // send the message
            // the destination is defined by the server name and port
            u.BeginSend(sendBytes, sendBytes.Length, "192.168.18.10", 9180, new AsyncCallback(SendCallback), u);

            // Do some work while we wait for the send to complete. For this example, we'll just sleep
            while (!messageSent)
            {
                Thread.Sleep(100);
            }
        }
    }
}
