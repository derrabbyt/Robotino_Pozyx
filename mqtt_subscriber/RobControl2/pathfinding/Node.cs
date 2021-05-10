using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RobControl
{
    class Node
    {
        public bool Walkable { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int gCost;
        public int hCost;

        public Node parent;

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public Node(bool walkable, int x, int y)
        {
            Walkable = walkable;
            X = x;
            Y = y;

        }

    }
}
