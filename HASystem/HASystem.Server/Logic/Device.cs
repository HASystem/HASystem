using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic
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
            set;
        }

        public DeviceHardware DeviceHardware
        {
            get;
            set;
        }

        public PhysicalAddress MACAddress
        {
            get;
            set;
        }

        public IPAddress IPAddress
        {
            get;
            set;
        }
    }
}