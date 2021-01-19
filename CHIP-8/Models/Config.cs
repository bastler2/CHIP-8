using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP_8.Models
{
    class Config
    {
        public int FpsRender { get; set; }
        public int FpsTimer { get; set; }
        public int ExecutionTickDelay { get; set; }
        public List<ConsoleKey> KeyBinding { get; set; }
        public bool ShowDebugging { get; set; }
    }
}
