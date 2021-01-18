using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CHIP_8
{
    public class Renderer
    {
        static int cols = 64;
        static int rows = 32;
        static int rowsDebug = 17;

        private bool[] display = new bool[cols * rows];
        public bool isDebugging = true;
        private static int cursorleft = Console.CursorLeft;
        private static int cursortop = Console.CursorTop;
        bool[,] lastDisplay = new bool[cols * 2, rows];

        Stopwatch stopWatch;

        public Renderer()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void showDebuggingTitle()
        {
            Console.SetCursorPosition(cursorleft, cursortop + rows);
            Console.Write("registry");
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

        private void debugging(byte[] v, byte[] memory) // need to clean section of debuggin to not leave numbers behind example first number:120 second:9 = 920 because x20 was not deleted
        {
            Console.BackgroundColor = ConsoleColor.Black; // expensive but ok i think


            // debugging screen registry
            for (int i = 1; i <= 16; i++)
            {
                //clear section beforehand, flickering not so nice
                Console.SetCursorPosition(cursorleft + 7, cursortop + rows + i);
                Console.Write("    "); 

                Console.SetCursorPosition(cursorleft, cursortop + rows + i);
                Console.Write("v[" + (i - 1) + "] = " + v[i - 1]);
            }


        }
        public int RenderFps = 60;
        Stopwatch stopwatchRender = new Stopwatch();
        public Task RenderAsync()
        {
            stopwatchRender.Start();
            while (true)
                if (stopwatchRender.ElapsedMilliseconds > (1000 / RenderFps))
                {
                    //if debugging enabled and console size is small = make big
                    if (Console.WindowHeight == (rows) && Console.WindowWidth == ((cols * 2)) && isDebugging)
                        Console.SetWindowSize(cols * 2, rows + rowsDebug);
                    else if ((Console.WindowHeight != rows || Console.WindowWidth != (cols * 2)) && isDebugging == false)
                        Console.SetWindowSize(cols * 2, rows);

                    bool[,] currentDisplay = new bool[cols * 2, rows];

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
                    stopwatchRender.Reset();
                    stopwatchRender.Start();
                }
        }
        public void Render(byte[] v, byte[] memory)
        {

            //if debugging enabled and console size is small = make big
            if (Console.WindowHeight == (rows) && Console.WindowWidth == ((cols * 2)) && isDebugging)
                Console.SetWindowSize(cols * 2, rows + rowsDebug);
            else if ((Console.WindowHeight != rows || Console.WindowWidth != (cols * 2)) && isDebugging == false)
                Console.SetWindowSize(cols * 2, rows);

            // if debugging show infos
            if (isDebugging)
                debugging(v, memory);


            bool[,] currentDisplay = new bool[cols * 2, rows];



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


        }
    }
}
