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
        private LogicComponentsFactory logicComponentsFactory = new LogicComponentsFactory();

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
            logicComponentsFactory.Init();

            House.Start();
            DhcpService.StartService();

#if DEBUG

            SetupDemo();

#endif
        }

#if DEBUG

        private void SetupDemo()
        {
            BinaryIn binaryIn = new BinaryIn();
            BinaryIn src2 = new BinaryIn();
            House.AddComponent(binaryIn);
            House.AddComponent(src2);
            BinaryAnd and = new BinaryAnd();
            House.AddComponent(and);

            System.Threading.Thread.Sleep(2000);

            binaryIn.Outputs[0].AddConnection(and.Inputs[0]);
            src2.Outputs[0].AddConnection(and.Inputs[1]);

            BinaryOut binaryOut = new BinaryOut();
            House.AddComponent(binaryOut);

            System.Threading.Thread.Sleep(2000);

            and.Outputs[0].AddConnection(binaryOut.Inputs[0]);
        }

#endif
    }
}