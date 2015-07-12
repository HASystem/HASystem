using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server
{
    public class House
    {
        private List<Device> devices = new List<Device>();

        public IReadOnlyCollection<Device> Devices
        {
            get
            {
                return devices;
            }
        }
    }
}