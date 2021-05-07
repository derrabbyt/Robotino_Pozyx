using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algo
{
    public partial class Form1 : Form

    {

        Point lastPoint = Point.Empty;//Point.Empty represents null for a Point object

        bool isMouseDown = new Boolean();//this is used to evaluate whether our mousebutton is down or not

        public Form1()

        {

            InitializeComponent();

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)

        {

            lastPoint = e.Location;//we assign the lastPoint to the current mouse position: e.Location ('e' is from the MouseEventArgs passed into the MouseDown event)

            isMouseDown = true;//we set to true because our mouse button is down (clicked)

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)

        {

            if (isMouseDown == true)//check to see if the mouse button is down

            {

                if (lastPoint != null)//if our last point is not null, which in this case we have assigned above

                {

                    if (pictureBox1.Image == null)//if no available bitmap exists on the picturebox to draw on

                    {
                        //create a new bitmap
                        Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                        pictureBox1.Image = bmp; //assign the picturebox.Image property to the bitmap created

                    }

                    using (Graphics g = Graphics.FromImage(pictureBox1.Image))

                    {//we need to create a Graphics object to draw on the picture box, its our main tool

                        //when making a Pen object, you can just give it color only or give it color and pen size

                        g.DrawLine(new Pen(Color.Black, 2), lastPoint, e.Location);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        //this is to give the drawing a more smoother, less sharper look

                    }

                    pictureBox1.Invalidate();//refreshes the picturebox

                    lastPoint = e.Location;//keep assigning the lastPoint to the current mouse position

                }

            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)

        {

            isMouseDown = false;

            lastPoint = Point.Empty;

            //set the previous point back to null if the user lets go of the mouse button

        }

        private void clearButton_Click(object sender, EventArgs e)//our clearing button

        {

            if (pictureBox1.Image != null)

            {

                pictureBox1.Image = null;

                Invalidate();

            }

        }

    }
}
