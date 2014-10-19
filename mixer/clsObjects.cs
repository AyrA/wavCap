using System.Runtime.InteropServices;

namespace mixer
{
    /// <summary>
    /// Mixer Information structure; Size=48
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MIXERCAPS
    {
        /// <summary>
        /// Manufacturer ID
        /// </summary>
        public ushort wMid;
        /// <summary>
        /// Product ID
        /// </summary>
        public ushort wPid;
        /// <summary>
        /// Drivr Version
        /// Format: 1.2.3.4=0x01020304
        /// </summary>
        public int vDriverVersion;
        /// <summary>
        /// Product Name
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        /// <summary>
        /// Support Bits
        /// </summary>
        public uint fdwSupport;
        /// <summary>
        /// Number of destination lines
        /// </summary>
        public uint cDestinations;

        public override string ToString()
        {
            return string.Format("MID: {0}, PID: {1}, Driver: {2}, Name: \"{3}\", Support: {4}, Lines: {5}",
                wMid, wPid, vDriverVersion, szPname, fdwSupport, cDestinations);
        }
    }
}
