using HASystem.Server.Physical.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Physical.Components
{
    public abstract class PhysicalComponent
    {
        public int Id
        {
            get;
            set;
        }

        public PhysicalDevice PhysicalDevice
        {
            get;
            private set;
        }

        protected PhysicalComponent(PhysicalDevice physicalDevice)
        {
            if (physicalDevice == null)
                throw new ArgumentNullException("physicalDevice");
            this.PhysicalDevice = physicalDevice;
        }

        //TODO: some kind of test if this component supports a specific logic-component.
    }
}