using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobControl
{
    public partial class Form1 : Form
    {

        //Robotinho robotinho;
        Mqtt mqtt_client;
        Server udp_client;
        Position currentPosition;
        Position targetPosition;
        Position startPosition;
        Position[] turningPositions;
        public Form1()
        {
            //robotinho = new Robotinho();
            InitializeComponent();
            //currentPosition = mqtt_client.CurrentPosition;
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            //robotinho.Connect();
            mqtt_client = new Mqtt("172.17.241.103");
            string mqtt_status = mqtt_client.Connect();

            udp_client = new Server("127.0.0.1", 9180);
            string udp_status = udp_client.Connect();

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            currentPosition = mqtt_client.CurrentPosition;
            startPosition = new Position(1,5);
            List<Node> path = AStar.FindPath(startPosition, new Position(20, 34));

            for (int i = 0; i < path.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(path[i].WorldPosition.X);
                System.Diagnostics.Debug.WriteLine(path[i].WorldPosition.Y);
            }
        }
    }
}
