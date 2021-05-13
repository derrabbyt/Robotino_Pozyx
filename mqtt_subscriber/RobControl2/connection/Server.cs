using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace RobControl
{
    class Server
    {
        int udp_port_emission;
        int udp_port_reception;
        IPAddress udp_ip;
        Socket sSend;
        Socket sReceive;
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private AsyncCallback recv = null;
        IPEndPoint ipe_emission;
        EndPoint ipe_reception;


        public Server(string address, int port_emission, int port_reception)
        {
            udp_port_emission = port_emission;
            udp_port_reception = port_reception;
            udp_ip = IPAddress.Parse(address);

        }
        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public string Connect()
        {
            ipe_emission = new IPEndPoint(udp_ip, udp_port_emission);
            ipe_reception = new IPEndPoint(udp_ip, udp_port_reception);


            sSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sReceive = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            sSend.Connect(ipe_emission);

            sReceive.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            sReceive.Bind(ipe_reception);

            Receive();
            return "connecting to server via udp oba is nu ned do";
        }

        public void Stop()
        {
            sSend.Close();
        }

        private static void Send(IPEndPoint ipe, Socket s, int xSoll, int ySoll, int phiSoll, int xIst, int
            yIst, int phiIst, int restart, int input7)
        {
            byte[] message = getMessageToSend(0, xSoll, ySoll, phiSoll, xIst, yIst, phiIst, restart, input7);
            s.SendTo(message, 0, message.Length, SocketFlags.None, ipe);
        }

        private static byte[] getMessageToSend(int ID_Message, int input0, int input1, int input2, int input3, int
            input4, int input5, int input6, int input7)
        {
            char[] Hex_Char = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            long[] InputTable = new long[] { input0, input1, input2, input3, input4, input5, input6, input7 };
            int checksum = 36;
            string valuesToString = "";
            string messageToSendToRobotinoView = "";

            if (ID_Message == 0)
                messageToSendToRobotinoView = "002400";
            else if (ID_Message == 1)
            {
                messageToSendToRobotinoView = "012400";
                checksum++;
            }

            for (int i = 0; i < 8; i++)
            {
                if (InputTable[i] < 0)
                    InputTable[i] = 4294967295 + InputTable[i] + 1;

                byte Byte3 = (byte)(InputTable[i] / (256 * 256 * 256));
                checksum += Byte3;

                InputTable[i] = InputTable[i] % (256 * 256 * 256);
                byte Byte2 = ((byte)(InputTable[i] / (256 * 256)));
                checksum += Byte2;

                InputTable[i] = InputTable[i] % (256 * 256);
                byte Byte1 = ((byte)(InputTable[i] / (256)));
                checksum += Byte1;

                InputTable[i] = InputTable[i] % 256;
                byte Byte0 = ((byte)InputTable[i]);
                checksum += Byte0;

                valuesToString += (Convert.ToString(Hex_Char[(Byte0 / 16)]) + Convert.ToString(Hex_Char[(Byte0 % 16)]) + Convert.ToString(Hex_Char[(Byte1 / 16)]) + Convert.ToString(Hex_Char[(Byte1 % 16)]) + Convert.ToString(Hex_Char[(Byte2 / 16)]) + Convert.ToString(Hex_Char[(Byte2 % 16)]) + Convert.ToString(Hex_Char[(Byte3 / 16)]) + Convert.ToString(Hex_Char[(Byte3 % 16)]));

            }

            checksum = 255 - (checksum % 256);
            string checksumToString = Convert.ToString(Hex_Char[checksum / 16]) + Convert.ToString(Hex_Char[checksum % 16]);
            messageToSendToRobotinoView += checksumToString + valuesToString;
            Console.WriteLine(messageToSendToRobotinoView);
            return StringToByteArrayFastest(messageToSendToRobotinoView);
        }
        public static byte[] StringToByteArrayFastest(string hex) //converts a string in hex to a byte array
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        } //used by the method above


        private void Receive()
        {
            sReceive.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref ipe_reception, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                sReceive.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref ipe_reception, recv, so);
                readOutput(so.buffer, 0);
            }, state);
        }

        private void readOutput(byte[] Bytes1, int inputX)
        {
            int startingByte = 4 + 4 * inputX;
            int intToReturn;

            List<byte> Bytes = new List<byte>();
            for (int i = startingByte; i < startingByte + 4; i++)
            {
                Bytes.Add(Bytes1[i]);
            }
            if (Bytes[3] > 128)
            {
                intToReturn = -(Convert.ToInt32(Bytes[0]) + (256 * Convert.ToInt32(Bytes[1])) + (256 * 256 * Convert.ToInt32(Bytes[2]) + (256 * 256 * 256 * Convert.ToInt32(Bytes[3]))));
            }
            else
            {
                intToReturn = Convert.ToInt32(Bytes[0]) + (256 * Convert.ToInt32(Bytes[1])) + (256 * 256 * Convert.ToInt32(Bytes[2]) + (256 * 256 * 256 * Convert.ToInt32(Bytes[3])));
            }

            Logic.UpdateMessageFromRobotino(intToReturn);
        }
    }
}
