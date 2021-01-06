using System;
using System.Collections.Generic;
using System.IO;

namespace CHIP_8
{
    class CPU
    {

        public Renderer renderer;
        public Speaker speaker;
        public Keyboard keyboard;
        private byte[] memory;
        private byte delayTimer;
        private int soundTimer;
        private int programmCounter;
        private List<int> stack;
        private bool paused;
        private int speed;
        private byte[] v;
        private int i;

        public CPU(Renderer renderer, Speaker speaker, Keyboard keyboard)
        {
            this.renderer = renderer;
            this.speaker = speaker;
            this.keyboard = keyboard;

            //4096 bytes (4KB) memory
            memory = new byte[4096];

            //16 x 8-bit registers
            v = new byte[16];

            //Current memory addresses
            i = 0;

            //Timers
            delayTimer = 0;
            soundTimer = 0;

            //Programm counter, starting from 0x200 for most applications
            programmCounter = 0x200;

            //dont initialize to avoid empty entrys - using List to avoid ? 
            stack = new List<int>();

            // Some instructions require pausing, like Fx0A.
            paused = false;

            // Speed
            speed = 10;
        }


        public void executeInstruction(int opcode)
        {
            programmCounter += 2;

            // We only need the 2nd nibble, so grab the value of the 2nd nibble
            // and shift it right 8 bits to get rid of everything but that 2nd nibble.
            int x = (opcode & 0x0F00) >> 8;

            // We only need the 3rd nibble, so grab the value of the 3rd nibble
            // and shift it right 4 bits to get rid of everything but that 3rd nibble.
            int y = (opcode & 0x00F0) >> 4;

            //Console.WriteLine("Executing Opcode: " + (opcode & 0xF000));
            switch (opcode & 0xF000)
            {
                case 0x0000:
                    switch (opcode)
                    {
                        case 0x00E0: //00E0 - CLS - Clear the Display
                            renderer.Clear();
                            break;
                        case 0x00EE: //00EE - RET - Return from a subroutine. The interpreter sets the program counter to the address at the top of the stack, then subtracts 1 from the stack pointer.
                            programmCounter = stack[stack.Count-1]; //needed to be debugged and optimized
                            stack.RemoveAt(stack.Count-1);
                            break;
                    }
                    break;
                case 0x1000: //1nnn - JP addr - Jump to location nnn. The interpreter sets the program counter to nnn.
                    programmCounter = (opcode & 0xFFF);
                    break;
                case 0x2000: //2nnn - CALL addr - Call subroutine at nnn. The interpreter increments the stack pointer, then puts the current PC on the top of the stack. The PC is then set to nnn.
                    stack.Add(programmCounter);
                    programmCounter = (opcode & 0xFFF);
                    break;
                case 0x3000: //3xkk - SE Vx, byte - Skip next instruction if Vx = kk. The interpreter compares register Vx to kk, and if they are equal, increments the program counter by 2.
                    if (v[x] == (opcode & 0xFF))
                        programmCounter += 2;
                    break;
                case 0x4000: //4xkk - SNE Vx, byte - Skip next instruction if Vx != kk. The interpreter compares register Vx to kk, and if they are not equal, increments the program counter by 2.
                    if (v[x] != (opcode & 0xFF))
                        programmCounter += 2;
                    break;
                case 0x5000: //5xy0 - SE Vx, Vy - Skip next instruction if Vx = Vy. The interpreter compares register Vx to register Vy, and if they are equal, increments the program counter by 2.
                    if (v[x] == v[y])
                        programmCounter += 2;
                    break;
                case 0x6000: //6xkk - LD Vx, byte - Set Vx = kk. The interpreter puts the value kk into register Vx.
                    v[x] = Convert.ToByte(opcode & 0xFF);
                    
                    break;
                case 0x7000: //7xkk - ADD Vx, byte - Set Vx = Vx + kk. Adds the value kk to the value of register Vx, then stores the result in Vx.
                    v[x] += Convert.ToByte(opcode & 0xFF);
                    break;
                case 0x8000:
                    switch (opcode & 0xF)
                    {
                        case 0x0: //8xy0 - LD Vx, Vy - Set Vx = Vy. Stores the value of register Vy in register Vx.
                            v[x] = v[y];
                            break;
                        case 0x1: //8xy1 - OR Vx, Vy - Set Vx = Vx OR Vy. Performs a bitwise OR on the values of Vx and Vy, then stores the result in Vx.A bitwise OR compares the corrseponding bits from two values, and if either bit is 1, then the same bit in the result is also 1.Otherwise, it is 0.
                            v[x] |= v[y];
                            break;
                        case 0x2: //8xy2 - AND Vx, Vy - Set Vx = Vx AND Vy. Performs a bitwise AND on the values of Vx and Vy, then stores the result in Vx.A bitwise AND compares the corrseponding bits from two values, and if both bits are 1, then the same bit in the result is also 1.Otherwise, it is 0.
                            v[x] &= v[y];
                            break;
                        case 0x3: //8xy3 - XOR Vx, Vy - Set Vx = Vx XOR Vy. Performs a bitwise exclusive OR on the values of Vx and Vy, then stores the result in Vx.An exclusive OR compares the corrseponding bits from two values, and if the bits are not both the same, then the corresponding bit in the result is set to 1.Otherwise, it is 0.
                            v[x] ^= v[y];
                            break;
                        case 0x4: //8xy4 - ADD Vx, Vy - Set Vx = Vx + Vy, set VF = carry. The values of Vx and Vy are added together.If the result is greater than 8 bits(i.e., > 255,) VF is set to 1, otherwise 0.Only the lowest 8 bits of the result are kept, and stored in Vx.
                            byte sum = (v[x] += v[y]);
                            v[0xF] = 0;
                            if (sum > 0xFF)
                                v[0xF] = 1;
                            v[x] = sum;
                            break;
                        case 0x5: //8xy5 - SUB Vx, Vy - Set Vx = Vx - Vy, set VF = NOT borrow. If Vx > Vy, then VF is set to 1, otherwise 0.Then Vy is subtracted from Vx, and the results stored in Vx.
                            v[0xF] = 0;
                            if (v[x] > v[y])
                                v[0xF] = 1;
                            v[x] -= v[y];
                            break;
                        case 0x6: //8xy6 - SHR Vx {, Vy} - Set Vx = Vx SHR 1. If the least - significant bit of Vx is 1, then VF is set to 1, otherwise 0.Then Vx is divided by 2.
                            v[0xF] = Convert.ToByte(v[x] & 0x01);
                            v[x] >>= 1;
                            break;
                        case 0x7: //8xy7 - SUBN Vx, Vy - Set Vx = Vy - Vx, set VF = NOT borrow. If Vy > Vx, then VF is set to 1, otherwise 0.Then Vx is subtracted from Vy, and the results stored in Vx.
                            v[0xF] = 0;
                            if (v[y] > v[x])
                                v[0xF] = 1;
                            v[x] = Convert.ToByte(v[y] - v[x]); // why do i have to convert when substracting two bytes
                            break;
                        case 0xE: //8xyE - SHL Vx {, Vy} - Set Vx = Vx SHL 1. If the most - significant bit of Vx is 1, then VF is set to 1, otherwise to 0.Then Vx is multiplied by 2.
                            v[0xF] = Convert.ToByte(v[x] & 0x80);
                            v[x] <<= 1;
                            break;
                    }
                    break;
                case 0x9000: //9xy0 - SNE Vx, Vy - Skip next instruction if Vx != Vy. The values of Vx and Vy are compared, and if they are not equal, the program counter is increased by 2.
                    if (v[x] != v[y])
                        programmCounter += 2;
                    break;
                case 0xA000: //Annn - LD I, addr - Set I = nnn. The value of register I is set to nnn.
                    i = (opcode & 0xFFF);
                    break;
                case 0xB000: //Bnnn - JP V0, addr - Jump to location nnn +V0. The program counter is set to nnn plus the value of V0.
                    programmCounter = (opcode & 0xFFF) + v[0];
                    break;
                case 0xC000: //Cxkk - RND Vx, byte - Set Vx = random byte AND kk. The interpreter generates a random number from 0 to 255, which is then ANDed with the value kk. The results are stored in Vx.See instruction 8xy2 for more information on AND.
                    v[x] = Convert.ToByte(new Random().Next(0, 255) & (opcode & 0xFF));
                    break;
                case 0xD000: //Dxyn - DRW Vx, Vy, nibble - Display n-byte sprite starting at memory location I at(Vx, Vy), set VF = collision. The interpreter reads n bytes from memory, starting at the address stored in I.These bytes are then displayed as sprites on screen at coordinates(Vx, Vy). Sprites are XORed onto the existing screen.If this causes any pixels to be erased, VF is set to 1, otherwise it is set to 0.If the sprite is positioned so part of it is outside the coordinates of the display, it wraps around to the opposite side of the screen.See instruction 8xy3 for more information on XOR, and section 2.4, Display, for more information on the Chip - 8 screen and sprites.



                    int width = 8;
                    int height = (opcode & 0xF);

                    v[0xF] = 0;

                    for (int row = 0; row < height; row++)
                    {
                        int sprite = memory[i + row];

                        for (int col = 0; col < width; col++)
                        {
                            // If the bit (sprite) is not 0, render/erase the pixel
                            if ((sprite & 0x80) > 0)
                            {
                                // If setPixel returns 1, which means a pixel was erased, set VF to 1
                                if (renderer.SetPixel(v[x] + col, v[y] + row))
                                {
                                    v[0xF] = 1;
                                }
                            }

                            // Shift the sprite left 1. This will move the next next col/bit of the sprite into the first position.
                            // Ex. 10010000 << 1 will become 0010000
                            sprite <<= 1;
                        }
                    }
                    break;
                case 0xE000:
                    switch (opcode & 0xFF)
                    {
                        case 0x9E: //Ex9E - SKP Vx - Skip next instruction if key with the value of Vx is pressed. Checks the keyboard, and if the key corresponding to the value of Vx is currently in the down position, PC is increased by 2.
                            if (keyboard.IsKeyPressed(v[x]))
                                programmCounter += 2;
                            break;
                        case 0xA1: //ExA1 - SKNP Vx - Skip next instruction if key with the value of Vx is not pressed. Checks the keyboard, and if the key corresponding to the value of Vx is currently in the up position, PC is increased by 2.
                            if (!keyboard.IsKeyPressed(v[x]))
                                programmCounter += 2;
                            break;
                    }
                    break;
                case 0xF000:
                    switch (opcode & 0xFF)
                    {
                        case 0x07: //Fx07 - LD Vx, DT - Set Vx = delay timer value. The value of DT is placed into Vx.
                            v[x] = delayTimer;
                            break;
                        case 0x0A: //Fx0A - LD Vx, K - Wait for a key press, store the value of the key in Vx. All execution stops until a key is pressed, then the value of that key is stored in Vx.
                            paused = true;
                            //ToDo
                            //keyboard.OnNextKeyPress = function(key) {
                            //    v[x] = key;
                            //    paused = false;
                            //}.bind(this);
                            break;
                        case 0x15: //Fx15 - LD DT, Vx - Set delay timer = Vx. DT is set equal to the value of Vx.
                            delayTimer = v[x];
                            break;
                        case 0x18: //Fx18 - LD ST, Vx - Set sound timer = Vx. ST is set equal to the value of Vx.
                            soundTimer = v[x];
                            break;
                        case 0x1E: //Fx1E - ADD I, Vx - Set I = I + Vx. The values of I and Vx are added, and the results are stored in I.
                            i += v[x];
                            break;
                        case 0x29: //Fx29 - LD F, Vx - Set I = location of sprite for digit Vx. The value of I is set to the location for the hexadecimal sprite corresponding to the value of Vx.See section 2.4, Display, for more information on the Chip - 8 hexadecimal font.
                            i = v[x] * 5;
                            break;
                        case 0x33: //Fx33 - LD B, Vx - Store BCD representation of Vx in memory locations I, I + 1, and I+2. The interpreter takes the decimal value of Vx, and places the hundreds digit in memory at location in I, the tens digit at location I+1, and the ones digit at location I + 2.
                            // Get the hundreds digit and place it in I.
                            memory[i] = Convert.ToByte(v[x] / 100);
                            // Get tens digit and place it in I+1. Gets a value between 0 and 99,
                            // then divides by 10 to give us a value between 0 and 9.
                            memory[i + 1] = Convert.ToByte((v[x] % 100) / 10);
                            // Get the value of the ones (last) digit and place it in I+2.
                            memory[i + 2] = Convert.ToByte(v[x] % 10);
                            break;
                        case 0x55: //Fx55 - LD [I], Vx - Store registers V0 through Vx in memory starting at location I. The interpreter copies the values of registers V0 through Vx into memory, starting at the address in I.
                            for (int registerIndex = 0; registerIndex <= x; registerIndex++)
                                memory[i + registerIndex] = Convert.ToByte(v[registerIndex]); //ToDo: CHECK IF RIGHT
                            break;
                        case 0x65: //Fx65 - LD Vx, [I] - Read registers V0 through Vx from memory starting at location I. The interpreter reads values from memory starting at location I into registers V0 through Vx.
                            for (int registerIndex = 0; registerIndex <= x; registerIndex++)
                                v[registerIndex] = memory[i + registerIndex];
                            break;
                    }
                    break;
                default:
                    throw new Exception("Unknown opcode " + opcode);
            }

        }

        public void loadSpritesIntoMemory()
        {
            // Array of hex values for each sprite. Each sprite is 5 bytes.
            byte[] sprites = new byte[] {
                0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
                0x20, 0x60, 0x20, 0x20, 0x70, // 1
                0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
                0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
                0x90, 0x90, 0xF0, 0x10, 0x10, // 4
                0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
                0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
                0xF0, 0x10, 0x20, 0x40, 0x40, // 7
                0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
                0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
                0xF0, 0x90, 0xF0, 0x90, 0x90, // A
                0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
                0xF0, 0x80, 0x80, 0x80, 0xF0, // C
                0xE0, 0x90, 0x90, 0x90, 0xE0, // D
                0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
                0xF0, 0x80, 0xF0, 0x80, 0x80  // F
            };
            // Stored in memory starting at hex 0x000
            for (int i = 0; i < sprites.Length; i++)
                memory[i] = sprites[i];
        }

        public void loadRom(string romPath)
        {
            // Reading rom from File as Bytes
            var rom = File.ReadAllBytes(romPath);
            // Loading programm into memory starting at 0x200
            for (int loc = 0; loc < rom.Length; loc++)
                memory[0x200 + loc] = rom[loc];
        }

        public void cycle()
        {
            for (int i = 0; i < speed; i++)
                if (!paused)
                    executeInstruction(memory[programmCounter] << 8 | memory[programmCounter + 1]);
            if (!paused)
                updateTimers();
            playSound();
            renderer.Render();
        }

        void updateTimers()
        {
            if (delayTimer > 0)
                delayTimer -= 1;
            if (soundTimer > 0)
                soundTimer -= 1;
        }
        void playSound()
        {
            if (soundTimer > 0)
                speaker.Play(440);
            else
                speaker.Stop();
        }
    }
}
