//#define BLOCK

using System;
using WaveLib;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace wavCap
{
    class Program
    {
        /// <summary>
        /// represents the default soundcard.
        /// </summary>
        private const int DEFAULT_SOUNDCARD = -1;
        /// <summary>
        /// Number of buffers
        /// </summary>
        private const int BUFFERS = 32;
        /// <summary>
        /// size of a single buffer
        /// </summary>
        private const int BUFFERSIZE = 128;
        /// <summary>
        /// playback rate in Hz.
        /// </summary>
        private const int RATE = 8000;
        /// <summary>
        /// data bits in Hz (usually 8 or 16)
        /// </summary>
        private const int BITS = 8;
        /// <summary>
        /// number of channels (1=Mono, 2=Stereo, 6=5.1 surround, etc)
        /// </summary>
        private const int CHANNELS = 1;

        /// <summary>
        /// block drawing char
        /// </summary>
        private const char C = '█';

        /// <summary>
        /// global counter. Increased for each buffer
        /// </summary>
        private static int counter = 0;
        /// <summary>
        /// increased for each processed buffer
        /// </summary>
        private static int loopcount = 0;
        /// <summary>
        /// Holds all the buffers
        /// </summary>
        private static List<byte[]> Buffers;

        /// <summary>
        /// the code of the stack language
        /// </summary>
        private static string code;

        /// <summary>
        /// Stack language processor
        /// </summary>
        private static StackLang L;

        /// <summary>
        /// Main entry point. Checks for DLL file
        /// </summary>
        /// <param name="args">command line arguments</param>
        static int Main(string[] args)
        {
            if (!System.IO.File.Exists("wavDLL.dll"))
            {
                Console.WriteLine("wavDLL.dll not found!");
                return 1;
            }
            go();
            return 0;
        }

        /// <summary>
        /// main loop
        /// </summary>
        private static void go()
        {
            //initial formula
            code = "B9*4B>&B5*7B>&|B3*92<B/&|]";
            int pos = code.Length;
            L = new StackLang();
            Buffers = new List<byte[]>(BUFFERS);
            for (int i = 0; i < BUFFERS; i++)
            {
                Buffers.Add(new byte[BUFFERSIZE]);
            }

            WaveFormat f = new WaveFormat(RATE, BITS, CHANNELS);
            WaveOutPlayer plr = new WaveOutPlayer(DEFAULT_SOUNDCARD, f, BUFFERSIZE, BUFFERS, new BufferFillEventHandler(DataSent));

            bool cont = true;

            DisplayHelp();

            while (cont)
            {
                display(code, pos);
                ConsoleKeyInfo CKI = Console.ReadKey(true);
                switch (CKI.Key)
                {
                    case ConsoleKey.Escape:
                        cont = false;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (pos > 0)
                        {
                            pos--;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (pos < code.Length)
                        {
                            pos++;
                        }
                        break;
                    case ConsoleKey.Insert:
                        if (pos < code.Length)
                        {
                            code = code.Insert(pos, " ");
                        }
                        break;
                    case ConsoleKey.Delete:
                        if (pos < code.Length)
                        {
                            code = code.Remove(pos, 1);
                        }
                        break;
                    case ConsoleKey.Backspace:
                        if (pos > 0)
                        {
                            code = code.Remove(--pos, 1);
                            Console.Write(' ');
                        }
                        break;
                    default:
                        if (CKI.KeyChar > 31 && CKI.KeyChar < 127)
                        {
                            if (pos < code.Length)
                            {
                                code = code.Remove(pos, 1);
                                code = code.Insert(pos++, CKI.KeyChar.ToString().ToUpper());
                            }
                            else
                            {
                                code += CKI.KeyChar.ToString().ToUpper();
                                pos++;
                            }
                        }
                        break;
                }
            }
            plr.Dispose();
            Console.WriteLine();
            Console.WriteLine("Finished");
        }

        /// <summary>
        /// Displays the command help
        /// </summary>
        private static void DisplayHelp()
        {
            Console.WriteLine("\r\n");
            Console.WriteLine("Assuming the Top value of the stack is y and the second top value is x");
            //─│┌┐└┘┼┴┬┤├
            Console.WriteLine(@"
┌──┬─────────────────────────────────────────────────────────────────┐
│A │ put the counter on stack          (resets for each buffer)      │
│B │ put the global counter on stack   (increments indefinitely)     │
│C │ put the loop counter on stack     (resets on each buffer cycle) │
├──┼────────────┬─────────────────┬───┬────────────┬─────────────────┤
│+ │ Add        │ x + y           │ - │ Subtract   │ x - y           │
│* │ Multiply   │ x * y           │ / │ Divide     │ x / y           │
│% │ Modulo     │ x % y           │ ! │ Pow        │ Pow(x,y)        │
├──┼────────────┼─────────────────┼───┼────────────┼─────────────────┤
│> │ Shift R    │ x >> y          │ < │ Shift L    │ x << y          │
├──┼────────────┼─────────────────┼───┼────────────┼─────────────────┤
│[ │ Increment  │ y + 1           │ ] │ Decrement  │ y - 1           │
├──┼────────────┼─────────────────┼───┼────────────┼─────────────────┤
│| │ Or         │ x | y           │ & │ And        │ x & y           │
│^ │ Xor        │ x ^ y           │ ~ │ Not        │ ~ y             │
├──┴────────────┴─────────────────┴───┴────────────┴─────────────────┤
│Empty stack will return 0 on operations whenever possible.          │
│Division by zero is prevented by supplying 1 to y if needed.        │
├────────────────────────────────────────────────────────────────────┤
│Values that affect A and C:                                         │
│                                                                    │
│Number of Buffers: {0,5}    (C is smaller than this)                │
│Buffer size:       {1,5}    (A is smaller than this)                │
└────────────────────────────────────────────────────────────────────┘
", BUFFERS, BUFFERSIZE);
        }

        /// <summary>
        /// displays formula and code in color
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pos"></param>
        private static void display(string code, int pos)
        {
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(string.Empty.PadRight(Console.BufferWidth - 1));
            Console.CursorLeft = 0;
            string temp = string.Format("Form: {0}", L.eval(code));
            if (temp.Length > Console.BufferWidth - 1)
            {
                temp = temp.Substring(0, Console.BufferWidth - 4) + "...";
            }
            Console.Write(temp);
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(string.Empty.PadRight(Console.BufferWidth - 1));
            Console.CursorLeft = 0;
            Console.Write("Code: {0} ", code);
            Console.CursorLeft--;
            Console.ForegroundColor = ConsoleColor.Red;
            if (pos < code.Length)
            {
                Console.CursorLeft = pos + 6;
                Console.Write(code[pos]);
            }
            else
            {
                Console.Write(C);
            }
            Console.ResetColor();
            Console.CursorLeft--;
        }

        /// <summary>
        /// Soundcard event if data is needed
        /// </summary>
        /// <param name="start">Memory start location to put data</param>
        /// <param name="size">Number of bytes to put</param>
        private static void DataSent(IntPtr start, int size)
        {
            for (int i = 0; i < BUFFERSIZE; i++)
            {
                int sample = L.Process(code, new string[] { "A" + i.ToString(), "B" + counter.ToString(), "C" + (loopcount % BUFFERS).ToString() });
                Buffers[loopcount % BUFFERS][i] = (byte)sample;
                counter++;
            }
            Marshal.Copy(Buffers[loopcount % BUFFERS], 0, start, size);
            loopcount++;
        }
    }
}
