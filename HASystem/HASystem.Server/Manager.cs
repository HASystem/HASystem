using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server
{
    public class Manager
    {
        private static Manager instance = new Manager();

        public House House
        {
            get;
            private set;
        }

        public DHCP.DHCPService DhcpService
        {
            get;
            private set;
        }

        public static Manager Instance
        {
            get
            {
                return instance;
            }
        }

        public Manager()
        {
            House = new House();
            DhcpService = new DHCP.DHCPService();
        }

        public void Start()
        {
            DhcpService.StartService();
        }
    }
}