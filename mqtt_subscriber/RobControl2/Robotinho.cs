using System;
using System.Collections.Generic;
using System.Text;

namespace RobControl
{
    class Robotinho
    {
        Mqtt mqtt_client;
        Server udp_client;
        Position CurrentPosition;
        Position TargetPosition;
        Position StartPosition;
        Position[] TurningPoints;
        public void Start()
        {
            StartPosition = CurrentPosition;
            List<Node> path = AStar.FindPath(CurrentPosition, TargetPosition);
        }

        public void Connect()
        {
            mqtt_client = new Mqtt("172.17.241.103");
            string mqtt_status = mqtt_client.Connect();

            udp_client = new Server("127.0.0.1", 9180);
            string udp_status = udp_client.Connect();      
        }

        public void Stop()
        {
            
        }
    }
}
