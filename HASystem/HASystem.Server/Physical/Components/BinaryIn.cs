using HASystem.Server.Physical.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Physical.Components
{
    public class BinaryIn : PhysicalComponent
    {
        public Port[] SupportedPorts
        {
            get;
            private set;
        }

        public BinaryIn(PhysicalDevice physicalDevice, Port[] supportedPorts)
            : base(physicalDevice)
        {
            SupportedPorts = supportedPorts;
        }
    }
}