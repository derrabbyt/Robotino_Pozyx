using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace tryudp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<List<Node>> Grid = MakeGrid();
            Astar algo = new Astar(Grid);        
        }

        private static List<List<Node>> MakeGrid()
        { 
            List<List<Node>> Grid = new List<List<Node>>(1);
            List<Node> Collums = new List<Node>();

            Node n = new Node(new System.Numerics.Vector2(1, 2), false, 1);
            nL[0] = n;
            Grid[0] = nL;
            return Grid;
        }
    }
}
