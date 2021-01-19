using CHIP_8.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CHIP_8
{
    public class Game
    {
        private Renderer renderer = new Renderer();
        private Keyboard keyboard;
        private CPU cpu;
        public Game(string[] args)
        {
            keyboard = new Keyboard(this);
            cpu = new CPU(renderer, keyboard);
            loadConfig();
            cpu.loadSpritesIntoMemory();
            if (args.Length > 0)
                cpu.loadRom(args[0]);
            else
                cpu.loadRom(@"BLINKY");

            Task.Run(() => renderer.RenderAsync());
            //Task.Run(() => cpu.cycle());
            cpu.cycle();
        }


        public void loadConfig()
        {
            if (!File.Exists("config.json"))
                File.WriteAllText("config.json", @"{""FpsRender"":60,""FpsTimer"":60,""ExecutionTickDelay"":5000,""KeyBinding"":[96,97,98,99,100,101,102,103,104,105,65,66,67,68,69,70],""ShowDebugging"":false}");
            var config = JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json"));
            cpu.TimerFps = config.FpsTimer;
            cpu.ExecutionTickDelay = config.ExecutionTickDelay;
            renderer.isDebugging = config.ShowDebugging;
            renderer.RenderFps = config.FpsRender;
            keyboard.keyMap = new Dictionary<ushort, ConsoleKey>()
            {
                    { 0x0, config.KeyBinding[0] },
                    { 0x1, config.KeyBinding[1] },
                    { 0x2, config.KeyBinding[2] },
                    { 0x3, config.KeyBinding[3] },
                    { 0x4, config.KeyBinding[4] },
                    { 0x5, config.KeyBinding[5] },
                    { 0x6, config.KeyBinding[6] },
                    { 0x7, config.KeyBinding[7] },
                    { 0x8, config.KeyBinding[8] },
                    { 0x9, config.KeyBinding[9] },
                    { 0xA, config.KeyBinding[10] },
                    { 0xB, config.KeyBinding[11] },
                    { 0xC, config.KeyBinding[12] },
                    { 0xD, config.KeyBinding[13] },
                    { 0xE, config.KeyBinding[14] },
                    { 0xF, config.KeyBinding[15] }
            };
        }
    }
}
