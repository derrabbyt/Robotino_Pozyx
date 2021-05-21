using System;
using System.Collections.Generic;
using System.Text;

namespace RobControl
{
    class Logic
    {
        Mqtt mqtt_position;
        Mqtt mqtt_nfc;
        public Server udp_client;

        public bool drivable;

        int turningPointIndex = 0;
        Position currentTurningPoint;

        Position targetPosition;
        Position startPosition;
        public Position currentPosition;

        public double currentDirection;
        public double TargetDirection;

        AStar Algo = new AStar();


        int count = 0;


        public List<Position> TurningPoints = new List<Position>();
        public void Connect()
        {

            udp_client = new Server("127.0.0.1", 9180, 9182, this);
            string udp_status = udp_client.Connect();
            System.Diagnostics.Debug.WriteLine("udp: " + udp_status);

            mqtt_position = new Mqtt("172.17.241.222", "position_data", this);
            string mqtt_status = mqtt_position.Connect();
            System.Diagnostics.Debug.WriteLine("pozyx: " + mqtt_status);

            mqtt_nfc = new Mqtt("172.17.241.222", "tag_nfc", this);
            string mqtt_status2 = mqtt_position.Connect();
            System.Diagnostics.Debug.WriteLine("nfc: " + mqtt_status2);

        }

        public void Start()
        {
            startPosition = new Position(31, 3);
            targetPosition = new Position(14, 22);
            TurningPoints = Algo.FindPath(startPosition, targetPosition);
            currentTurningPoint = TurningPoints[0];
        }

        public void Stop()
        {
            //moin
        }

        public void MessageFromRobotino1(int msg) //checks if the first element of the message is "1", if yes the turning point was reached
        {
            System.Diagnostics.Debug.WriteLine("message from robotinho: " + msg);
            if (msg == 1)
            {

                if (TurningPoints != null && currentPosition != null)
                {
                    if (turningPointIndex < TurningPoints.Count - 1)
                    {
                        turningPointIndex++;
                        currentTurningPoint = TurningPoints[turningPointIndex];
                        SendTargetCoordinate();
                    }

                }
            }
        }

        public void MessageFromMqtt(string str, string topic)
        {
            if (topic == "position_data")
            {
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace(", ", " ");
                string[] coords = str.Split(" ");
                int x = Convert.ToInt32(coords[0]);
                int y = Convert.ToInt32(coords[1]);
                count++;
               

                if(x!= 0 && y!= 0)
                {
                    currentPosition = new Position(x, y);
                    System.Diagnostics.Debug.WriteLine(currentPosition.X + " " + currentPosition.Y);
                }

                if (currentTurningPoint != null && currentPosition != null)
                {
                    SendTargetCoordinate();
                }


            }
            else if (topic == "tag_nfc")
            {

            }
        }

        public void SendTargetCoordinate() => udp_client.Send(currentTurningPoint.X*200, currentTurningPoint.Y*200, 0, currentPosition.X, currentPosition.Y, 0, 0, 123);
        public Position GetCurrentPosition() => currentPosition;
    }
}
