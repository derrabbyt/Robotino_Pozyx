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
        public Position CurrentPosition { get; set; }
        public Mqtt(string address)
        {
            Client = new MqttClient(address);
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
            }
            else
            {
                status = ("Mqtt Connection Refused");
            }
            try
            {
                Client.Subscribe(new string[] { "position_data" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

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


            //int[] bytesAsInts = Array.ConvertAll(e.Message, c => (int)c);
            //System.Diagnostics.Debug.WriteLine(bytesAsInts[0]);
            //System.Diagnostics.Debug.WriteLine(bytesAsInts[1]);

            //CurrentPosition = new Position(bytesAsInts[0], bytesAsInts[1]);

        }



    }
}
