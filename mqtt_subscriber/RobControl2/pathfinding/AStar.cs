using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobControl
{
    class AStar
    {
        static Node[,] grid;
        public static List<Position> TurningPoints = new List<Position>();
        public static void FindPath(Position startPos, Position targetPos)
        {

            grid  = Grid.CreateGrid(startPos,targetPos);

            Node startNode = grid[startPos.Y, startPos.X];
            Node targetNode = grid[targetPos.Y, targetPos.X];

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                    {
                        if (openSet[i].hCost < node.hCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (Node neighbour in Grid.GetNeighbours(node))
                {

                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);

                    }
                }
            }
        }

        static void RetracePath(Node startNode, Node endNode)
        {

            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            CalculateTurningPoints(path);
        }

        static int GetDistance(Node nodeA, Node nodeB)
        {

            //if(grid[nodeB.X, nodeB.Y].Walkable == false)
            //{
            //    return 2000;
            //}
            int dstX = Math.Abs(nodeA.X - nodeB.X);
            int dstY = Math.Abs(nodeA.Y - nodeB.Y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        static void CalculateTurningPoints(List<Node> path) 
        {
            int count = 0;
            int direction = -1;
            int currentDirection;

            for (int i = 1; i < path.Count - 1; i++) //i hob keine ahnung wos genau der code mocht oba danke stackoverflow
            {
                if(path[i].X - path[i-1].X != 0)
                    currentDirection = 0;
                else
                    currentDirection = 1;

                if(direction != -1)
                {
                    if(currentDirection != direction)
                    {
                        count++;
                        path[i-1].IsTurningPoint = true;
                        TurningPoints.Add(new Position(path[i-1].X, path[i-1].Y));
                        System.Diagnostics.Debug.WriteLine("changed direction at: x:" + path[i].X + " y: " + path[i].Y );
                    }
                }
                direction = currentDirection;
            }


            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].IsTurningPoint == true)
                    {
                        System.Diagnostics.Debug.Write("Ä");
                    }
                    else if (grid[i, j].IsPath == true)
                    {
                        System.Diagnostics.Debug.Write("Ü");
                    }
                    else if (grid[i, j].Walkable == true)
                    {
                        System.Diagnostics.Debug.Write(" ");
                    }
                    else if (grid[i, j].Walkable == false)
                    {
                        System.Diagnostics.Debug.Write("x");
                    }

                }
                System.Diagnostics.Debug.WriteLine("");
            }
            System.Diagnostics.Debug.WriteLine("");

        }
    }
}
