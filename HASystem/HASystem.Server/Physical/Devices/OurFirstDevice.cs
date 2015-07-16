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
        private PhysicalComponent[] componenets;

        public override DeviceHardware DeviceKind
        {
            get { return DeviceHardware.UnserBoardVersion1; }
        }

        public override PhysicalComponent[] Components
        {
            get { return componenets; }
        }

        public OurFirstDevice()
        {
            componenets = new PhysicalComponent[] {
                new BinaryInOut(this, new Port[] { Port.PD0, Port.PD1, Port.PD2, Port.PD3, Port.PD4, Port.PD5, Port.PD6, Port.PD7 })
            };
        }
    }
}