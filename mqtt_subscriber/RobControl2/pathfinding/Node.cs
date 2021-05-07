using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RobControl
{
    class Node
    {
        public bool Walkable { get; set; }
        public Position WorldPosition { get; set; }

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

        public Node(bool walkable, Position position)
        {
            Walkable = walkable;
            WorldPosition = position;
        }

    }
}
