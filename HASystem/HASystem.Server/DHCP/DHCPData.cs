using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.DHCP
{
    public struct DHCPData
    {
        public IPAddress IPAddr;
        public IPAddress SubMask;
        public uint LeaseTime;
        public string ServerName;
        public IPAddress MyIP;
        public IPAddress RouterIP;
        public IPAddress DomainIP;
        public string LogServerIP;
    }
}