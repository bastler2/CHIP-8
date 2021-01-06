using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Input;

namespace CHIP_8
{
    public class Renderer
    {
        static int cols = 64;
        static int rows = 32;
        private bool[] display = new bool[cols * rows];

        public bool SetPixel(int x, int y)
        {
            if (x > cols)
                x -= cols;
            else if (x < 0)
                x += cols;

            if (y > rows)
                y -= rows;
            else if (y < 0)
                y += rows;

            int pixelLoc = x + (y * cols);
            display[pixelLoc] ^= true;
            return !display[pixelLoc];
        }
        public void Clear()
        {
            display = new bool[cols * rows];
        }

        bool[,] lastDebug = new bool[64, 32];
        public void Render()
        {
            bool[,] debug = new bool[64,32];


            // Loop through our display array
            for (int i = 0; i < cols * rows; i++)
            {
                // Grabs the x position of the pixel based off of `i`
                int x = (i % cols);
                // Grabs the y position of the pixel based off of `i`
                int y = i / cols;

                // If the value at this.display[i] == 1, then draw a pixel.
                if (display[i])
                {
                    // Place a pixel at position (x, y) with a width and height of scale
                    debug[x, y] = true;
                }
            }


            for (int i = 0; i <= 31; i++)
            {
                for (int j = 0; j <= 63; j++)
                {
                    if (debug[j, i] == true && debug[j, i] != lastDebug[j, i])
                    {
                        WriteAt((char)35, j, i);
                    }
                    else if(debug[j, i] == false && debug[j, i] != lastDebug[j, i])
                    {
                        WriteAt((char)32, j, i);
                    }
                }
            }

            lastDebug = debug;
        }
        private static int cursorleft = Console.CursorLeft;
        private static int cursortop = Console.CursorTop;

        protected static void WriteAt(char s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(cursorleft + x, cursortop + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        internal bool CheckPixel()
        {
            throw new NotImplementedException();
        }
    }
}
