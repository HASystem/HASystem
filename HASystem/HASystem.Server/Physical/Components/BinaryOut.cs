using HASystem.Server.Physical.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Physical.Components
{
    public class BinaryOut : PhysicalComponent
    {
        public Port[] SupportedPorts
        {
            get;
            private set;
        }

        public BinaryOut(PhysicalDevice physicalDevice, Port[] supportedPorts)
            : base(physicalDevice)
        {
            SupportedPorts = supportedPorts;
        }
    }
}