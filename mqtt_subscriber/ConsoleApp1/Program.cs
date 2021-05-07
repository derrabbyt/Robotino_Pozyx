using System;
using System.Drawing;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap img = new Bitmap(@"C:\Users\manue\OneDrive\Dokumente\Htl Neufelden\4. Klasse\Maturaprojekt\Robotino_Pozyx\mqtt_subscribers\111links.bmp");

            bool[,] obstacleCoordinates = new bool[img.Width, img.Height];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (pixel.R * 0.2126 + pixel.G * 0.7152 + pixel.B * 0.0722 > 255 / 2)
                    {
                        obstacleCoordinates[i, j] = true;
                    }

                }
            }
        }
    }
}
