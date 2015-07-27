using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public enum DeviceHardware
    {
        Unknown = 0,

        UnserBoardVersion1 = 1,
        IpCam = 4711, //we could support this

        Unsupported = Int32.MaxValue,
    }
}