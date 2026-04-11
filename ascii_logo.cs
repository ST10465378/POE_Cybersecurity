using System;
using System.Drawing;

namespace dynabot_cybersecurity
{//Start of namespace
    public class ascii_logo
    {//Start of class
        public ascii_logo()
        {//start of constructor
            //call the asci method
            asci();
        }//end of constructor

        //method to display the ASCII logo
        private void asci()
        {//start of method
            //automatically get the folder where the app is running
            //this works on any PC regardless of username or folder structure
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png");
            Bitmap image = new Bitmap(path);

            // Resize for better console fit
            int width = 150;
            int height = 70;
            Bitmap resized = new Bitmap(image, new Size(width, height));

            // ASCII characters mapped from dark to light
            string asciiChars = "@#S%?*+;:,. ";

            Console.ForegroundColor = ConsoleColor.Green;

            //start by the height
            for (int y = 0; y < resized.Height; y++)
            {
                //then width
                for (int x = 0; x < resized.Width; x++)
                {
                    //get the pixel color at x and y
                    Color pixel = resized.GetPixel(x, y);
                    // Convert to grayscale
                    int gray = (pixel.R + pixel.G + pixel.B) / 3;
                    // Map grayscale to ASCII character
                    int index = (gray * (asciiChars.Length - 1)) / 255;
                    Console.Write(asciiChars[index]);
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }//end of method
    }//end of class
}//end of namespace