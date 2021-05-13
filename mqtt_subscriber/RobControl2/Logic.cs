using System;
using System.Collections.Generic;
using System.Text;

namespace RobControl
{
    static class Logic
    {
        static Mqtt mqtt_position;
        static Mqtt mqtt_nfc;
        static Server udp_client;

        public static Position currentPosition;
        public static int messageFromRobotino;
        static Position targetPosition;
        static Position startPosition;

        public static void Connect()
        {
            //mqtt_position = new Mqtt("172.17.241.103", "position_data");
            //string mqtt_status = mqtt_position.Connect();
            //System.Diagnostics.Debug.WriteLine("pozyx: " + mqtt_status);

            //mqtt_nfc = new Mqtt("172.17.241.103", "tag_nfc");
            //string mqtt_status2 = mqtt_position.Connect();
            //System.Diagnostics.Debug.WriteLine("nfc: " + mqtt_status2);

            udp_client = new Server("127.0.0.1", 9180, 9182);
            string udp_status = udp_client.Connect();
            System.Diagnostics.Debug.WriteLine("udp: " + udp_status);

        }

        public static void Start()
        {
            startPosition = new Position(31, 3);
            targetPosition = new Position(14, 22);
            AStar.FindPath(startPosition, targetPosition);
        }

        public static void Stop()
        {
            //moin
        }

        public static void UpdateMessageFromRobotino(int msg)
        {
            System.Diagnostics.Debug.WriteLine("message from robotinho: " + msg);
        }

        public static void PositionFromPozyxUpdate(Position cP)
        {
            currentPosition = cP;
            System.Diagnostics.Debug.WriteLine("X: " + cP.X + " Y: " + cP.Y);
        }
    }
}
