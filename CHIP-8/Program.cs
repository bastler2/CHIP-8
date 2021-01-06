using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;

namespace CHIP_8
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameSettings = new GameWindowSettings
            {
                UpdateFrequency = 600
            };

            var nativeSettings = new NativeWindowSettings
            {
                Size = new Vector2i(64, 32),
                Profile = ContextProfile.Compatability,
                Title = "Chip8 Emulator"
            };

            //var window = new GameWindow(gameSettings, nativeSettings);

            //window.Run();
            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
            using (Game game = new Game(gameSettings, nativeSettings))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run();
                //game.Run(60.0);
                
            };



            
        }
    }
}
