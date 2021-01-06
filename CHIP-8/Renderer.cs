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

        public Renderer()
        {
            Console.SetWindowSize(cols, rows);
            Console.CursorVisible = false;
        }

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
                int x = (i % cols);
                int y = i / cols;

                // If the value at this.display[i] == 1, then draw a pixel.
                if (display[i])
                    debug[x, y] = true;

                if (debug[x, y] == true && debug[x, y] != lastDebug[x, y])
                    WriteAt((char)35, x, y);
                else if (debug[x, y] == false && debug[x, y] != lastDebug[x, y])
                    WriteAt((char)32, x, y);

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
