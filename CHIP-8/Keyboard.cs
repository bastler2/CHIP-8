using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;

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
        public void OnKeyDown(Keys key)
        {
            ushort mappedKey;
            keyMap.TryGetValue(key, out mappedKey);
            keysPressed[mappedKey] = true;
        }
        public void OnKeyUp(Keys key)
        {
            ushort mappedKey;
            keyMap.TryGetValue(key, out mappedKey);
            keysPressed[mappedKey] = false;
        }

        /// <summary>
        /// Keybindings
        /// </summary>
        private static Dictionary<Keys, ushort> keyMap = new Dictionary<Keys, ushort>
        {
                { Keys.KeyPad0, 0x0 },
                { Keys.KeyPad1, 0x1 },
                { Keys.KeyPad2, 0x2 },
                { Keys.KeyPad3, 0x3 },
                { Keys.KeyPad4, 0x4 },
                { Keys.KeyPad5, 0x5 },
                { Keys.KeyPad6, 0x6 },
                { Keys.KeyPad7, 0x7 },
                { Keys.KeyPad8, 0x8 },
                { Keys.KeyPad9, 0x9 },
                { Keys.Q, 0xA },
                { Keys.W, 0xB },
                { Keys.E, 0xC },
                { Keys.A, 0xD },
                { Keys.S, 0xE },
                { Keys.D, 0xF }
        };
    }
}
