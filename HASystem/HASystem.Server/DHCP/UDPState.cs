using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.DHCP
{
    public struct UDPState
    {
        //define an end point
        public IPEndPoint EndPoint { get; set; }

        //define a client
        public UdpClient Client { get; set; } 
    }
}
