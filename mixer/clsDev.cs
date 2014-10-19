using System.Runtime.InteropServices;
using System;
namespace mixer
{
    public class Device
    {
        [DllImport("Winmm.dll")]
        private static extern uint mixerGetNumDevs();
        [DllImport("Winmm.dll", EntryPoint = "mixerGetDevCaps")]
        private static extern int MixerGetDevCaps(uint uMxId, ref MIXERCAPS pmxcaps, int pcbmxcaps);


        public const uint DEFAULT = uint.MaxValue;

        public static uint NumDevices
        {
            get
            {
                return mixerGetNumDevs();
            }
        }

        public int DeviceID
        {get;private set;}

        public MIXERCAPS MixerInfo
        { get; private set; }

        public Device()
            : this(DEFAULT)
        {
        }

        public Device(uint ID)
        {
            MIXERCAPS mdc = new MIXERCAPS();
            MixerGetDevCaps(ID, ref mdc, Marshal.SizeOf(mdc));
            MixerInfo = mdc;
            Console.WriteLine(MixerInfo.ToString());
        }
    }
}
