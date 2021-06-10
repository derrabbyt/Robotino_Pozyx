using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Threading;

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
        public void mythread()
        {
            while (true)
            {
                Draw();
                System.Diagnostics.Debug.WriteLine("not nuasfasdfadll");
            }
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            Thread thr = new Thread(new ThreadStart(mythread));

            (mqtt_status2, mqtt_status, udp_status) = RobLogic.Connect();
            if (mqtt_status2 && mqtt_status && udp_status)
            {
                btn_start.Enabled = true;
                thr.Start();
            }
        }

        private void Draw()
        {
            pictureBox1.Paint += pictureBox1_Paint;
            System.Diagnostics.Debug.WriteLine("not nuasfasdfadll");
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Position currentPosition = RobLogic.GetCurrentPosition();      // call this if you want the current pos
            Pen pen = new Pen(Color.Red);

            g.DrawEllipse(pen, 200, 200, 4, 4);

            if (RobLogic.currentPosition != null)
            {

                System.Diagnostics.Debug.WriteLine("not null");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("null");
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            //RobLogic.Start();
            //Position currentPosition = RobLogic.GetCurrentPosition();      // call this if you want the current pos

            // Connect the Paint event of the PictureBox to the event handler method.
            // Add the PictureBox control to the Form.
            //this.Controls.Add(canvas);
        }

    }
}
