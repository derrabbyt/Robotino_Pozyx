using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Algo
{
    class Obstacles
    {

        private bool[,] obstalcles = new bool[25, 35];
        private const int size = 16;
        private const int space = 2;

        public void Draw(Graphics g)
        {
            //Brush brush;
            //for (int j = 0; j < obstalcles.GetLength(1); j++)
            //{
            //    for (int i = 0; i < obstalcles.GetLength(0); i++)
            //    {
            //        if (obstalcles[i, j])
            //            brush = Brushes.Red;
            //        else
            //            brush = Brushes.Green;
            //        g.FillRectangle(brush, i * (size + space), j * (size + space), size, size);
            //    }
            //}
        }
    }
}
