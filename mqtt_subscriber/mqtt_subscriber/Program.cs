using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Mqtt.Thingsboard
{
    class Program
    {
        static void Main(string[] args)
        {
            client.Start();
            Console.ReadKey();
        }
    }

    class client
    {
        private static MqttClient Client;
        public static void Start()
        {

            // Create client instance 
            Client = new MqttClient("172.17.241.103");   // new MqttClient("localhost");//

;

            byte code = Client.Connect(Guid.NewGuid().ToString());

            Client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;


            if (code == 0x00)
            {
                Console.WriteLine("Client connected to Server node!");
            }
            else
            {
                Console.WriteLine("Connection Refused");
            }
            try
            {
                Client.Subscribe(new string[] { "position_data" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });         

            }
            catch (Exception ex)
            {
                Console.WriteLine("Start: Exception thrown: " + ex.Message);
            }

        }

        static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received 
            Console.WriteLine(System.Text.Encoding.Default.GetString(e.Message));
            Console.WriteLine(e.Topic);

        }
    }

}
