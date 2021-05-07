using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;

namespace pathfinding_test
{
    class Grid
    {
        public Vector2 gridWorldSize = new Vector2(25, 32);
        public float nodeRadius = 3;
        float nodeDiameter;
        int gridSizeX, gridSizeY;
        Node[,] grid;

        void Start()
        {
            //nodeDiameter = nodeRadius = 2;
            //gridSizeX = (int)Math.Round(gridWorldSize.X / nodeDiameter);
            //gridSizeY = (int)Math.Round(gridWorldSize.Y / nodeDiameter);
            CreateGrid();
           
        }

        //private void DrawGrid(Bitmap img)
        //{
        //    for (int i = 0; i < img.Width; i++)
        //    {
        //        for (int j = 0; j < img.Height; j++)
        //        {
        //          if (grid[i,j] == 
  
        //        }
        //    }
        //}

        public void CreateGrid() //pfusch it in
        {
            Bitmap img = new Bitmap(@"C:\Users\Florian\Desktop\Robotino\mqtt_subscriber\111links.bmp"); //pic of roomlayout in 1:500 
            Color pixel;    //obstacles are black in the pic
            grid = new Node[img.Width, img.Height];

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    pixel = img.GetPixel(i, j);
                    if (pixel.R * 0.2126 + pixel.G * 0.7152 + pixel.B * 0.0722 > 255 / 2)
                    {
                        //checks if color in pic is black (dark)
                        grid[i, j] = new Node(false, new Position(i, j));
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("o");
                        grid[i, j] = new Node(true, new Position(i, j));
                    }
                        
                }
                Console.WriteLine();
            }
        }
    }
}
