using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace tryudp
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
            udpClient.Connect("172.17.240.174", 8192);

            try
            {
                string message = String.Empty;
                do
                {
                    udpClient.Send(sendBytes, sendBytes.Length);
                    Console.WriteLine();
                   
                    Console.WriteLine("should send");
                } while (true);

                udpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey();
        }
    }
}
