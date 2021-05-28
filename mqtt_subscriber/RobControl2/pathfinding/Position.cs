using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobControl
{
    class Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position(int x, int y, int d)
        {
            X = x;
            Y = y;
            Direction = d;
        }
        public int X { get; set; }

        public int Y { get; set; }

        public int Direction { get; set; }

        public double Angle { get; set; }
    }
}
