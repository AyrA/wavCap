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
        private const int DEFAULT_SOUNDCARD = -1;
        private const int BUFFERS = 32;
        private const int BUFFERSIZE = 128;
        private const int RATE = 8000;//8000;
        private const int BITS = 8;
        private const int CHANNELS = 1;

        private const char C = '█';

        private static int counter = 0;
        private static int loopcount = 0;
        private static List<byte[]> Buffers;

        private static string code;
        private static StackLang L;

        static void Main(string[] args)
        {
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

        private static void DataSent(IntPtr start, int size)
        {
            int i;
            for (i = 0; i < BUFFERSIZE; i++)
            {
                int sample = L.Process(code, new string[] { "A" + i.ToString(), "B" + counter.ToString(), "C" + (loopcount % BUFFERS).ToString() });
                Buffers[loopcount % BUFFERS][i] = (byte)sample;
                counter++;
            }
            Marshal.Copy(Buffers[loopcount % BUFFERS], 0, start, size);
        }
    }
}
