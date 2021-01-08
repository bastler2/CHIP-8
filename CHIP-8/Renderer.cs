using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;
using OpenTK.Input;

namespace CHIP_8
{
    public class Renderer
    {
        static int cols = 64;
        static int rows = 32;

        static int rowsDebug = 17;

        private bool[] display = new bool[cols * rows];

        private bool isDebugging = true;

        Stopwatch stopWatch;

        public Renderer()
        {
            var x = Console.LargestWindowHeight;
            var y = Console.LargestWindowWidth;
            if (isDebugging)
            {
                stopWatch = new Stopwatch();
                Console.SetWindowSize(cols * 2, rows + rowsDebug);
                showDebuggingTitle();
                stopWatch.Start();
            }
            else
                Console.SetWindowSize(cols * 2, rows);

            Console.CursorVisible = false;
            lastDisplay = new bool[cols * 2, rows];


            
            
        }

        private void showDebuggingTitle()
        {

            Console.SetCursorPosition(cursorleft, cursortop + rows);
            Console.Write("registry");

            Console.SetCursorPosition(cursorleft + 15, cursortop + rows);
            Console.Write("current frametime");

            Console.SetCursorPosition(cursorleft + 15, cursortop + (rows + 2));
            Console.Write("current fps");
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
        bool[,] lastDisplay;
        int fpsCounter = 0;

        private void debugging(byte[] v, byte[] memory) // need to clean section of debuggin to not leave numbers behind example first number:120 second:9 = 920 because x20 was not deleted
        {
            Console.BackgroundColor = ConsoleColor.Black; // expensive but ok i think

            // Clear Debugging Section
            for (int i = 0; i < 16; i++)
            {

            }

            // debugging screen registry
            for (int i = 1; i <= 16; i++)
            {
                //clear section beforehand, flickering not so nice
                Console.SetCursorPosition(cursorleft + 7, cursortop + rows + i);
                Console.Write("    "); 

                Console.SetCursorPosition(cursorleft, cursortop + rows + i);
                Console.Write("v[" + (i - 1) + "] = " + v[i - 1]);
            }

            //look at part of memory from config maybe ?? 



            if (fpsCounter == 500)
            {
                Console.SetCursorPosition(cursorleft + 15, cursortop + rows + 1);
                Console.Write("                                           "); // need to check lenght when besides it other values are displayed
                Console.SetCursorPosition(cursorleft + 15, cursortop + rows + 3);
                Console.Write("                                           "); // need to check lenght when besides it other values are displayed

                //look at current time in milliseconds how long a instruction takes to display it, so alle including debugging screen and everything
                Console.SetCursorPosition(cursorleft + 15, cursortop + rows + 1);
                Console.Write(stopWatch.Elapsed.TotalMilliseconds/500 + "ms");

                //fps
                Console.SetCursorPosition(cursorleft + 15, cursortop + rows + 3);
                Console.Write((1000 / (stopWatch.Elapsed.TotalMilliseconds/500)) + " updates/s");
                fpsCounter = 0;
            }
            fpsCounter++;

            stopWatch.Reset();
            stopWatch.Start();

            

            //int counter = 0;
            //int offset = 30; // starts 30 from the left
            //for (int i = 0; i < memory.Length; i++)
            //{
            //    if (i == 1024 || i == 2048 || i == 3072 || i == 4096) //16 in one line
            //    {
            //        offset += 15;
            //        counter = 0;
            //    }

            //    Console.SetCursorPosition(cursorleft + offset, cursortop + rows + counter);
            //    Console.Write("mem[" + i + "]=" + memory[i]);
            //    counter++;
            //}
        }
        public void Render(byte[] v, byte[] memory)
        {
            bool[,] currentDisplay = new bool[cols * 2, rows];
            if(isDebugging)
                debugging(v, memory);


            // Loop through our display array
            for (int i = 0; i < cols * rows; i++)
            {
                int x = (i % cols);
                int y = i / cols;

                // If the value at this.display[i] == 1, then draw a pixel.
                if (display[i])
                    currentDisplay[x * 2, y] = true;

                if (currentDisplay[x * 2, y] == true && currentDisplay[x * 2, y] != lastDisplay[x * 2, y])
                {
                    Console.BackgroundColor = ConsoleColor.White; //expensive but ok i think
                    Console.SetCursorPosition(cursorleft + x * 2, cursortop + y);
                    Console.Write("  ");
                }
                else if (currentDisplay[x * 2, y] == false && currentDisplay[x * 2, y] != lastDisplay[x * 2, y])
                {
                    Console.BackgroundColor = ConsoleColor.Black; //expensive but ok i think
                    Console.SetCursorPosition(cursorleft + x * 2, cursortop + y);
                    Console.Write("  ");
                }
            }
            lastDisplay = currentDisplay;


            // after redner complete set curcsor to bottom right to start writing there when inputs are incomming
            Console.SetCursorPosition(cursorleft * 2 + cols, cursortop + rows);


        }
    }
}
