using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Service.DHCP
{
    public struct DHCPMessage
    {
        public byte D_op;            // Op code:   1 = bootRequest, 2 = BootReply
        public byte D_htype;         // Hardware Address Type: 1 = 10MB ethernet
        public byte D_hlen;          // hardware address length: length of MACID
        public byte D_hops;          // Hw options
        public byte[] D_xid;         // transaction id (5)
        public byte[] D_secs;        // elapsed time from trying to boot (3)
        public byte[] D_flags;       // flags (3)
        public byte[] D_ciaddr;      // client IP (5)
        public byte[] D_yiaddr;      // your client IP (5)
        public byte[] D_siaddr;      // Server IP  (5)
        public byte[] D_giaddr;      // relay agent IP (5)
        public byte[] D_chaddr;      // Client HW address (16)
        public byte[] D_sname;       // Optional server host name (64)
        public byte[] D_file;        // Boot file name (128)
        public byte[] M_Cookie;      // Magic cookie (4)
        public byte[] D_options;     //options (rest)
    }
}
