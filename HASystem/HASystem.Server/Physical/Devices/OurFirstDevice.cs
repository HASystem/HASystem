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
            List<PhysicalComponent> components = new List<PhysicalComponent>();

            foreach (Port port in Enum.GetValues(typeof(Port)))
            {
                components.Add(new BinaryIn(this, port));
                components.Add(new BinaryOut(this, port));
            }

            Components = components.ToArray();
        }

        public override PhysicalComponent[] GetSupportedComponents(LogicComponent logicComponent)
        {
            //TODO: filter if port is already used by other input/output
            if (logicComponent.GetType() == typeof(Logic.LogicInput))
            {
                return Components.Where(p => p.GetType() == typeof(BinaryIn)).ToArray();
            }
            else if (logicComponent.GetType() == typeof(Logic.LogicOutput))
            {
                //TODO: filter if output component is already assigned to other logic component
                return Components.Where(p => p.GetType() == typeof(BinaryOut)).ToArray();
            }
            return new PhysicalComponent[0];
        }
    }
}