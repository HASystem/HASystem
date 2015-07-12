using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server
{
    public class Device
    {
        public string Name
        {
            get;
            private set;
        }

        public DeviceState State
        {
            get;
            private set;
        }

        public PhysicalAddress MACAddress
        {
            get;
            private set;
        }

        public System.Net.IPAddress IPAddress
        {
            get;
            private set;
        }
    }
}