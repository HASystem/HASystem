using HASystem.Server.Logic;
using HASystem.Server.Physical.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Physical.Devices
{
    public class OurFirstDevice : PhysicalDevice
    {
        public override DeviceHardware DeviceKind
        {
            get { return DeviceHardware.UnserBoardVersion1; }
        }

        public OurFirstDevice()
        {
            Components = new PhysicalComponent[] {
                new BinaryIn(this, new Port[] { Port.PD0, Port.PD1, Port.PD2, Port.PD3, Port.PD4, Port.PD5, Port.PD6, Port.PD7 })
            };
        }
    }
}