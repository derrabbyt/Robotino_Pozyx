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
    public partial class UI : Form
    {
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
        }
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics gObject = canvas.CreateGraphics();
            Brush red = new SolidBrush(Color.Red);
            Pen redPen = new Pen(red, 8);

            gObject.DrawLine(redPen, 50, 50, 50, 50);
        }
    }
}
