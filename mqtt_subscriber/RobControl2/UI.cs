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
    public partial class UI : System.Windows.Forms.Form
    {
        // call this if you want the current pos
        Logic RobLogic = new Logic();

        bool mqtt_status2;
        bool mqtt_status; 
        bool udp_status;

        public UI()
        {
            InitializeComponent();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            (mqtt_status2, mqtt_status, udp_status) = RobLogic.Connect();
            if (mqtt_status2 && mqtt_status && udp_status)
            {
                btn_start.Enabled = true;
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            RobLogic.Start();
        }

    }
}
