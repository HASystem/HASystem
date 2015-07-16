using HASystem.Server.Logic;
using HASystem.Server.Physical.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Physical.Devices
{
    public abstract class PhysicalDevice
    {
        public abstract DeviceHardware DeviceKind
        {
            get;
        }

        public abstract PhysicalComponent[] Components
        {
            get;
        }
    }
}