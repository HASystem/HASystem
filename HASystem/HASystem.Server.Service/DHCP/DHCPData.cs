using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Service.DHCP
{
    public struct DHCPData
    {
        public string IPAddr;
        public string SubMask;
        public uint LeaseTime;
        public string ServerName;
        public string MyIP;
        public string RouterIP;
        public string DomainIP;
        public string LogServerIP;
    }
}
