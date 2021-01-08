using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Windows.Input;
using System.Diagnostics;

namespace CHIP_8
{
    public class Keyboard
    {
        bool[] keysPressed;
        public Keyboard()
        {

            keysPressed = new bool[16];

            // Some Chip-8 instructions require waiting for the next key press. We initialize this function elsewhere when needed.
            //onNextKeyPress = null;
        }

        internal bool IsKeyPressed(int key)
        {
            return keysPressed[key];
        }
        public void OnKeyDown(ConsoleKey key)
        {
            ushort mappedKey;
            keyMap.TryGetValue(key, out mappedKey);
            keysPressed[mappedKey] = true;
        }
        public void OnKeyUp(ConsoleKey key)
        {
            ushort mappedKey;
            keyMap.TryGetValue(key, out mappedKey);
            keysPressed[mappedKey] = false;
        }



        private ConsoleKey? lastPressedKey = null; // new console == 0 ?!?!?
        private ConsoleKey? currentKey = null;
        Stopwatch test = new Stopwatch();
        public Task CheckKeys()
        {
            while (true)
            {
                currentKey = Console.ReadKey(true).Key;
                OnKeyDown((ConsoleKey)currentKey);
                Task.Delay(200);
                OnKeyUp((ConsoleKey)currentKey);
            }
            

            if (Console.KeyAvailable) // check what heppen when pressing two at once !?! button states prob. will break // supported ?
            {
                
                if (currentKey != lastPressedKey && currentKey != null)
                {

                    OnKeyDown((ConsoleKey)currentKey);
                    //OnKeyUp((ConsoleKey)currentKey);
                }
                lastPressedKey = currentKey;
            }
            else
            {
                if(currentKey == lastPressedKey && currentKey != null)
                {
                    OnKeyUp((ConsoleKey)currentKey);
                    lastPressedKey = null;
                }
                //if (lastPressedKey != new ConsoleKey())
                //{
                //    OnKeyUp(currentKey);
                //    lastPressedKey = new ConsoleKey();
                //}
            }
            return null;
                    
        }

        /// <summary>
        /// Keybindings
        /// </summary>
        private static Dictionary<ConsoleKey, ushort> keyMap = new Dictionary<ConsoleKey, ushort>
        {
                { ConsoleKey.NumPad0, 0x0 },
                { ConsoleKey.NumPad1, 0x1 },
                { ConsoleKey.NumPad2, 0x2 },
                { ConsoleKey.NumPad3, 0x3 },
                { ConsoleKey.NumPad4, 0x4 },
                { ConsoleKey.NumPad5, 0x5 },
                { ConsoleKey.NumPad6, 0x6 },
                { ConsoleKey.NumPad7, 0x7 },
                { ConsoleKey.NumPad8, 0x8 },
                { ConsoleKey.NumPad9, 0x9 },
                { ConsoleKey.Q, 0xA },
                { ConsoleKey.W, 0xB },
                { ConsoleKey.E, 0xC },
                { ConsoleKey.A, 0xD },
                { ConsoleKey.S, 0xE },
                { ConsoleKey.D, 0xF }
        };
    }
}
