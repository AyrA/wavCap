using System;
using WaveLib;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace wavDemo
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
        private const int BUFFERS = 64;
        /// <summary>
        /// size of a single buffer
        /// </summary>
        private const int BUFFERSIZE = 1024*2;

        private static WaveStream WS;
        private static long Pos = 0;

        static void Main(string[] args)
        {
            WS = new WaveStream(File.OpenRead(@"C:\Users\Administrator\Desktop\test.wav"));
            Pos = WS.Position;
            WaveOutPlayer plr = new WaveOutPlayer(DEFAULT_SOUNDCARD, WS.Format, BUFFERSIZE, BUFFERS, new BufferFillEventHandler(DataSent));
            Console.ReadKey(true);
            plr.Dispose();
        }

        private static void DataSent(IntPtr start, int size)
        {
            byte[] b = new byte[size];
            int readed = WS.Read(b, 0, b.Length);

            Marshal.Copy(b, 0, start, size);

            Console.SetCursorPosition(0, 0);
            Console.Write("{0}%",WS.Position*100/WS.Length);
            if (readed < size)
            {
                Console.Clear();
                WS.Seek(Pos, SeekOrigin.Begin);
            }
        }
    }
}
