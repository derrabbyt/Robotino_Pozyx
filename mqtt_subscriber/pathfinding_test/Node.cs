using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace pathfinding_test
{
    class Node
    {
        public bool Walkable { get; set; }
        public Position WorldPosition { get; set; }

        public Node(bool walkable, Position worldposition)
        {
            Walkable = walkable;
            WorldPosition = worldposition;
        }

    }
}
