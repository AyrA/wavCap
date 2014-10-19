using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mixer
{
    public static class Default
    {
        public static void Main(string[] args)
        {
            for (uint i = 0; i < Device.NumDevices; i++)
            {
                new Device(i);
            }
            Console.ReadKey(true);
        }
    }
}
