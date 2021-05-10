using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;

namespace RobControl
{
    class Grid
    {
        //pic of roomlayout in 1:500 
        static Color pixel;    //obstacles are black in the pic
        static Bitmap img;
        static Node[,] grid;
        public static Node[,] CreateGrid(Position startPos, Position targetPos) //pfusch it in
        {
            img = new Bitmap(@"C:\Users\Florian\Desktop\Robotino\mqtt_subscriber\111links.bmp");
            grid = new Node[img.Width, img.Height];
            System.Diagnostics.Debug.WriteLine("gridSizeXY: " + img.Width +  "x" + img.Height);

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    pixel = img.GetPixel(i, j);
                    if (pixel.R * 0.2126 + pixel.G * 0.7152 + pixel.B * 0.0722 > 255 / 2)//checks if color in pic is black (dark)
                    {
                        grid[i, j] = new Node(true, i, j);
                        System.Diagnostics.Debug.Write(" ");        //no obstacle
                    }   
                    else
                    {
                        grid[i, j] = new Node(false, i, j);
                        System.Diagnostics.Debug.Write("x");        //obstacle
                    }   
                    if(i== targetPos.Y && j == targetPos.X)
                    {
                        System.Diagnostics.Debug.Write("t");
                    }
                    if (i == startPos.Y && j == startPos.X)
                    {
                        System.Diagnostics.Debug.Write("o");
                    }
                    //width = y = i and height = x = j
                }
                System.Diagnostics.Debug.WriteLine("");
            }
            System.Diagnostics.Debug.WriteLine("");

            //for (int i = 0; i < grid.GetLength(0); i++)
            //{
            //    for (int j = 0; j < grid.GetLength(1); j++)
            //    {
            //        if(grid[i,j].Walkable == true)
            //        {
            //            System.Diagnostics.Debug.Write(" ");
            //        }
            //        if(grid[i, j].Walkable == false)
            //        {
            //            System.Diagnostics.Debug.Write("x");
            //        }
            //    }
            //    System.Diagnostics.Debug.WriteLine("");
            //}
            System.Diagnostics.Debug.WriteLine("grid created");
            return grid;
        }

        public static List<Node> GetNeighbours2(Node node)
        {
            System.Diagnostics.Debug.WriteLine("get neighbours enter");
            List<Node> neighbours = new List<Node>();
            int x = node.X;
            int y = node.Y;


            for (int i = x - 1; i <= x + 1; i++)    //checks the surrounding fields (max 8)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i > -1 && i < img.Width && j > -1 && j < img.Height)  //checks OutOfRange Exception
                    {
                        System.Diagnostics.Debug.WriteLine("add neighbour");
                        neighbours.Add(node);
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("finish adding neighbour");
            return neighbours;
        } //old code unused
        public static List<Node> GetNeighbours(Node node)
        {
            System.Diagnostics.Debug.WriteLine("entering getNeighbours");
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.X + x;
                    int checkY = node.Y + y;

                    if (checkX >= 0 && checkX < img.Height && checkY >= 0 && checkY < img.Width)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("finish adding neighbour");
            return neighbours;
        }
    }
}
