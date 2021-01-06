using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
using System;

namespace CHIP_8
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.CursorVisible = false;
            Console.SetWindowSize(64, 32);
            var gameSettings = new GameWindowSettings
            {
                UpdateFrequency = 60
            };

            var nativeSettings = new NativeWindowSettings
            {
                Size = new Vector2i(64, 32),
                Profile = ContextProfile.Compatability,
                Title = "Chip8 Emulator"
            };
            using (Game game = new Game(gameSettings, nativeSettings))
            {
                game.Run();
            }
        }
    }
}
