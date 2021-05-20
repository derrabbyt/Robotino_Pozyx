using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace RobControl
{
    class Mqtt
    {
        private static MqttClient Client;

        public bool Connected { get; set; }
        public Position CurrentPosition { get; set; }

        string tag;
        public Logic RobLogic;
        public Mqtt(string address, string _tag, Logic rL)
        {
            Client = new MqttClient(address);
            tag = _tag;
            RobLogic = rL;
        }


       
        public string Connect()
        {
            // Create client instance 
           // new MqttClient("localhost");//

            byte code = Client.Connect(Guid.NewGuid().ToString());

            Client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            string status;

            if (code == 0x00)
            {
               status = ("Mqtt Client connected to Server node!");
               Connected = true;
            }
            else
            {
                status = ("Mqtt Connection Refused");
                Connected = false;
            }
            try
            {
                Client.Subscribe(new string[] { tag }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return status;

        }

        public void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string str = System.Text.Encoding.Default.GetString(e.Message);

            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace(", ", " ");
            string[] coords = str.Split(" ");
            int x = Convert.ToInt32(coords[0]);
            int y = Convert.ToInt32(coords[1]);
            CurrentPosition = new Position(x, y);
            RobLogic.MessageFromMqtt(System.Text.Encoding.Default.GetString(e.Message), e.Topic);
        }
    }
}
