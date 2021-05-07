using System;
using System.Collections.Generic;
using System.Text;

namespace RobControl
{
    class AStar
    {
        static Position[] TurningPoints;
        static Node[,] grid = Grid.CreateGrid();
        static int proportion = 200;
        
        public static List<Node> FindPath(Position startPos, Position targetPos)
        {
            Node startNode = grid[(startPos.X / proportion), (startPos.Y / proportion)];
            Node targetNode = grid[(targetPos.X / proportion), (targetPos.Y / proportion)];

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return(RetracePath(startNode, targetNode));
                }

                foreach (Node neighbour in Grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if(newMovementCostToNeighbour< neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
            return null;
        }

        static List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            return path;
        }

        static int GetDistance(Node nodeA, Node nodeB)
        {
            int distanceX = (int)MathF.Abs(nodeA.WorldPosition.X - nodeB.WorldPosition.X);
            int distanceY = (int)MathF.Abs(nodeA.WorldPosition.Y - nodeB.WorldPosition.Y);

            if (distanceX > distanceY)
                return 14 * distanceY + 10 * (distanceX - distanceY);
            return 14 * distanceX + 10 * (distanceY - distanceX);

        }
    }
}
