using System;
using System.Runtime.InteropServices;

namespace WaveLib
{
    public static class WaveDev
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct WAVEINCAPS
        {
            public ushort wMid; //2
            public ushort wPid; //2
            public uint vDriverVersion; //4
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname; //32
            public uint dwFormats; //4
            public ushort wChannels; //2
            public ushort wReserved1; //2
        } //48 Bytes

        [DllImport("winmm.dll", EntryPoint = "waveInGetDevCaps", SetLastError = true)]
        private static extern int waveInGetDevCaps(int uDeviceID, ref WAVEINCAPS pwic, int cbwic);

        [DllImport("winmm.dll", EntryPoint = "waveInGetNumDevs", SetLastError = true)]
        private static extern int waveInGetNumDevs();

        public static int NumDev
        {
            get
            {
                return waveInGetNumDevs();
            }
        }

        public static string[] allDevices
        {
            get
            {
                return null;
            }
        }

        public static string DeviceName(int ID)
        {
            WAVEINCAPS w = new WAVEINCAPS();
            waveInGetDevCaps(ID, ref w, Marshal.SizeOf(w));
            return w.szPname;
        }
    }
}
