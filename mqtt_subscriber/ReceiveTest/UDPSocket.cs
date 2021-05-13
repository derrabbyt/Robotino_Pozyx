using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace ReceiveTest
{
    public class UDPSocket
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9182);
        private AsyncCallback recv = null;

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public void Server(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Receive();
        }

        private void Receive()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                readOutput(so.buffer, 0);
            }, state);
        }

        private void readOutput(byte[] Bytes1, int inputX)
        {
            int startingByte = 4 + 4 * inputX;
            int intToReturn;

            List<byte> Bytes = new List<byte>();
            for (int i = startingByte; i < startingByte+4; i++)
            {
                Bytes.Add(Bytes1[i]);
            }
            if (Bytes[3]> 128)
            {
                intToReturn =  - (Convert.ToInt32(Bytes[0]) + (256 * Convert.ToInt32(Bytes[1])) + (256 * 256 * Convert.ToInt32(Bytes[2]) + (256 * 256 * 256 * Convert.ToInt32(Bytes[3]))));
            }
            else
            {
                intToReturn = Convert.ToInt32(Bytes[0]) + (256 * Convert.ToInt32(Bytes[1])) + (256 * 256 * Convert.ToInt32(Bytes[2]) + (256 * 256 * 256 * Convert.ToInt32(Bytes[3])));
            }
            Console.WriteLine(intToReturn);

        }
    }
}