using HASystem.Server.Logic;
using HASystem.Server.Logic.Components;
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
            House.Start();
            DhcpService.StartService();

#if DEBUG

            SetupDemo();

#endif
        }

#if DEBUG

        private void SetupDemo()
        {
            BinarySource src1 = new BinarySource();
            BinarySource src2 = new BinarySource();
            House.AddComponent(src1);
            src2.Config["Outputs"] = "3";
            House.AddComponent(src2);
            BinaryAnd and = new BinaryAnd();
            House.AddComponent(and);

            System.Threading.Thread.Sleep(2000);

            src1.Outputs[0].AddConnection(and.Inputs[0]);
            src2.Outputs[2].AddConnection(and.Inputs[1]);

            BinarySink sink = new BinarySink();
            House.AddComponent(sink);
        }

#endif
    }
}