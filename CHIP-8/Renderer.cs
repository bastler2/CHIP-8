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

        private static int cursorleft = Console.CursorLeft;
        private static int cursortop = Console.CursorTop;
        bool[,] lastDisplay = new bool[64, 32];
        public void Render()
        {
            bool[,] currentDisplay = new bool[64,32];

            // Loop through our display array
            for (int i = 0; i < cols * rows; i++)
            {
                int x = (i % cols);
                int y = i / cols;

                // If the value at this.display[i] == 1, then draw a pixel.
                if (display[i])
                    currentDisplay[x, y] = true;

                if (currentDisplay[x, y] == true && currentDisplay[x, y] != lastDisplay[x, y])
                {
                    Console.BackgroundColor = ConsoleColor.White; //expensive but ok i think
                    Console.SetCursorPosition(cursorleft + x, cursortop + y);
                    Console.Write((char)32);
                }
                else if (currentDisplay[x, y] == false && currentDisplay[x, y] != lastDisplay[x, y])
                {
                    Console.BackgroundColor = ConsoleColor.Black; //expensive but ok i think
                    Console.SetCursorPosition(cursorleft + x, cursortop + y);
                    Console.Write((char)32);
                }
            }
            lastDisplay = currentDisplay;
        }
    }
}
