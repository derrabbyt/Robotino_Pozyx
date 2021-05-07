﻿using System;
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
        Mqtt mqtt_position;
        Mqtt mqtt_nfc;
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
            mqtt_position = new Mqtt("172.17.241.103", "position_data");
            string mqtt_status = mqtt_position.Connect();

            mqtt_position = new Mqtt("172.17.241.103", "tag_nfc");
            string mqtt_status2 = mqtt_position.Connect();

            udp_client = new Server("127.0.0.1", 9180);
            string udp_status = udp_client.Connect();

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            currentPosition = mqtt_position.CurrentPosition;
            startPosition = new Position(1,5);
            AStar.FindPath(startPosition, new Position(3, 7));
        }
    }
}
