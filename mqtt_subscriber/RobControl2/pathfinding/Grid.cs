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
        public static Node[,] CreateGrid() //pfusch it in
        {
           img = new Bitmap(@"C:\Users\manue\OneDrive\Dokumente\Htl Neufelden\4. Klasse\Maturaprojekt\Robotino_Pozyx\mqtt_subscriber\111links.bmp");
            Node[,] grid = new Node[img.Width, img.Height];
            
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    pixel = img.GetPixel(i, j);
                    if (pixel.R * 0.2126 + pixel.G * 0.7152 + pixel.B * 0.0722 > 255 / 2)   //checks if color in pic is black (dark)
                        grid[i, j] = new Node(false, new Position(i, j));
                    else
                        grid[i, j] = new Node(true, new Position(i, j));
                }
            }

            return grid;
        }

        public static List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            int x = node.WorldPosition.X;
            int y = node.WorldPosition.Y;


            for (int i = x - 1; i <= x + 1; i++)    //checks the surrounding fields (max 8)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i > -1 && i < img.Width && j > -1 && j < img.Height)  //checks OutOfRange Exception
                    {
                        neighbours.Add(node);
                    }
                }
            }

            return neighbours;
        }
    }
}
