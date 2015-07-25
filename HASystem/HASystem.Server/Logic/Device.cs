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
        private PhysicalAddress macAddress = PhysicalAddress.None;

        public PhysicalAddress MACAddress
        {
            get { return macAddress; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                macAddress = value;
            }
        }

        public string Name
        {
            get;
            set;
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

        public IPAddress IPAddress
        {
            get;
            set;
        }
    }
}