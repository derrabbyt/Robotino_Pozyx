using System;
using System.Collections.Generic;
using System.Text;

namespace RobControl
{
    class AStar
    {
        static Node[,] grid;
        static List<Node> _path;
        public static void FindPath(Position startPos, Position targetPos)
        {
            System.Diagnostics.Debug.WriteLine("path find enter");

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
                        System.Diagnostics.Debug.WriteLine("checks costs");
                        if (openSet[i].hCost < node.hCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == targetNode)
                {
                    System.Diagnostics.Debug.WriteLine("should enter retrace path");
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (Node neighbour in Grid.GetNeighbours(node))
                {
                    System.Diagnostics.Debug.WriteLine("try checking neighbours");
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
                        System.Diagnostics.Debug.WriteLine("open neighbours");
                    }
                }
            }

        }

        static void RetracePath(Node startNode, Node endNode)
        {
            System.Diagnostics.Debug.WriteLine("entered retrace path");
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            _path = path;
            CalculateTurningPoints(path);
        }

        static int GetDistance(Node nodeA, Node nodeB)
        {
            System.Diagnostics.Debug.WriteLine("enter getdistance");
            int dstX = Math.Abs(nodeA.X - nodeB.X);
            int dstY = Math.Abs(nodeA.Y - nodeB.Y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        static void CalculateTurningPoints(List<Node> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine("x: " + path[i].X + " y: " + path[i].Y + " walkable: " + path[i].Walkable +  " cost: " + path[i].hCost);
            
            
            }
        }
    }
}
