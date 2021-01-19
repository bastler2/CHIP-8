using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CHIP_8
{
    public class Keyboard
    {
        private Game game;



        public Keyboard(Game game)
        {
            this.game = game;
        }
        /// <summary>
        /// A positional bit flag indicating the part of a key state denoting
        /// key pressed.
        /// </summary>
        private const int KeyPressed = 0x8000;

        /// <summary>
        /// Returns a value indicating if a given key is pressed.
        /// </summary>
        internal bool IsKeyDown(ushort key)
        {
            //reload config while running
            if ((GetKeyState((int)ConsoleKey.F5) & KeyPressed) != 0)
                game.loadConfig();


            return (GetKeyState((int)keyMap[key]) & KeyPressed) != 0;
        }

        /// <summary>
        /// Gets the key state of a key.
        /// </summary>
        /// <param name="key">Virtuak-key code for key.</param>
        /// <returns>The state of the key.</returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetKeyState(int key);

        public byte ReadKey()
        {
            //var x = keyMap.Select(m => m).Where(k => k.Value.Equals()).;
            var key = Console.ReadKey().Key;
            return (byte)keyMap.Where(p => p.Value == key).Select(p => p.Key).FirstOrDefault();
        }


        public Dictionary<ushort, ConsoleKey> keyMap = new Dictionary<ushort, ConsoleKey>();
        /// <summary>
        /// Keybindings
        /// </summary>
        //private static Dictionary<ushort, ConsoleKey> keyMap = new Dictionary<ushort, ConsoleKey>
        //{
        //        { 0x0, ConsoleKey.NumPad0 },
        //        { 0x1, ConsoleKey.NumPad1 },
        //        { 0x2, ConsoleKey.NumPad2 },
        //        { 0x3, ConsoleKey.NumPad3 },
        //        { 0x4, ConsoleKey.NumPad4 },
        //        { 0x5, ConsoleKey.NumPad5 },
        //        { 0x6, ConsoleKey.NumPad6 },
        //        { 0x7, ConsoleKey.NumPad7 },
        //        { 0x8, ConsoleKey.NumPad8 },
        //        { 0x9, ConsoleKey.NumPad9 },
        //        { 0xA, ConsoleKey.Q },
        //        { 0xB, ConsoleKey.W },
        //        { 0xC, ConsoleKey.E },
        //        { 0xD, ConsoleKey.A },
        //        { 0xE, ConsoleKey.S },
        //        { 0xF, ConsoleKey.D }
        //};
    }
}
