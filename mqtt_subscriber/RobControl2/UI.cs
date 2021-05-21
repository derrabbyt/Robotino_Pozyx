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
        System.Threading.Thread t;
        // call this if you want the current pos
        Logic RobLogic = new Logic();

        public UI()
        {
            InitializeComponent();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            RobLogic.Connect();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            RobLogic.Start();
            Position currentPosition = RobLogic.GetCurrentPosition();      // call this if you want the current pos

            // Connect the Paint event of the PictureBox to the event handler method.


            // Add the PictureBox control to the Form.
            this.Controls.Add(pictureBox1);

        }

        public void DoThisAllTheTime()
        {
            while (true)
            {
                //you need to use Invoke because the new thread can't access the UI elements directly
                MethodInvoker mi = delegate () { draw(); };
                this.Invoke(mi);
            }


        }

        public void draw()
        {
            pictureBox1.Paint += new PaintEventHandler(this.pictureBox1_Paint);


        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Red);
            // Draw a point in the PictureBox.y
            if (RobLogic.currentPosition != null)
            {
                System.Diagnostics.Debug.WriteLine("asdfasdf");

                g.DrawEllipse(pen, RobLogic.currentPosition.X/200, 10, 2, 2);



            }


        }

        private void UI_Load(object sender, EventArgs e)
        {
            t = new System.Threading.Thread(DoThisAllTheTime);
            t.Start();
        }

        private void Form1_Activated(object sender, System.EventArgs e)
        {
            //pictureBox1.Paint += new PaintEventHandler(this.pictureBox1_Paint);

            //// Add the PictureBox control to the Form.
            //this.Controls.Add(pictureBox1);
        }
    }
}
