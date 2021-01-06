using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Threading.Tasks;

namespace CHIP_8
{
    public class Game : GameWindow
    {
        private Renderer renderer = new Renderer();
        private Keyboard keyboard = new Keyboard();
        private Speaker speaker = new Speaker();
        private CPU cpu;
        int loop;
        TimeSpan fpsInterval = new TimeSpan(0,0,0,0,5);
        TimeSpan then;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base (gameWindowSettings, nativeWindowSettings)
        {
            cpu = new CPU(renderer, speaker, keyboard);

            cpu.loadSpritesIntoMemory();
            cpu.loadRom(@"C:\Users\Sebastian\Downloads\octoachip8story.ch8");

            Task.Run(() => CpuCycle());
            //while (true)
            //{
            //    cpu.cycle();
            //}
        }
        private Task CpuCycle()
        {
            while (true)
            {
                cpu.cycle();
            }
        }



        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, this.Size.X, this.Size.Y);
            base.OnResize(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            cpu.keyboard.OnKeyUp(e.Key);
            base.OnKeyUp(e);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
                Close();

            cpu.keyboard.OnKeyDown(e.Key);
            base.OnKeyDown(e);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
        }

    }
}
