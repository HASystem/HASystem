using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.DHCP
{
    public enum DHCPMessageType
    {
        DHCPDISCOVER = 1,
        DHCPOFFER = 2,
        DHCPREQUEST = 3,
        DHCPDECLINE = 4,
        DHCPACK = 5,
        DHCPNAK = 6,
        DHCPRELEASE = 7,
        DHCPINFORM = 8
    }
}
