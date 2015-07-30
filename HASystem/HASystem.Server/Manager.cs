using HASystem.Server.DHCP;
using HASystem.Server.Logic;
using HASystem.Server.Logic.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HASystem.Server
{
    public class Manager
    {
        private static Manager instance = new Manager();

        private DHCPServer dhcpServer;
        private IPAddress networkIpMin = IPAddress.Parse("192.168.0.1");
        private IPAddress networkIpMax = IPAddress.Parse("192.168.0.255");
        private IPAddress currentIp = IPAddress.Parse("192.168.0.1");

        //private IPAddress adapterIp = IPAddress.Parse("192.168.0.254");
        private IPAddress adapterIp = IPAddress.Parse("0.0.0.0");

        private Thread dhcpServerThread = null;

        public House House
        {
            get;
            private set;
        }

        public LogicComponentsFactory ComponentsFactory
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
            ComponentsFactory = new LogicComponentsFactory();
        }

        public void Start()
        {
            ComponentsFactory.Init();

            House.Start();
            StartDhcpService();

#if DEBUG

            SetupDemo();

#endif
        }

        private void StartDhcpService()
        {
            //allow data from all network cards
            dhcpServer = new DHCPServer(adapterIp);
            dhcpServer.Announced += new DHCPServer.AnnouncedEventHandler(DHCPAnnounced);
            dhcpServer.Request += new DHCPServer.RequestEventHandler(DHCPRequest);

            dhcpServerThread = new Thread(dhcpServer.StartDHCPServer);
            dhcpServerThread.Start();
        }

        private void DHCPAnnounced(DHCPTransaction transaction, string macId)
        {
            //options should be filled with valid data
            transaction.Data.IPAddr = GetIpForDevice(macId);
            transaction.Data.SubMask = IPAddress.Parse("255.255.255.0");
            transaction.Data.LeaseTime = 2000;
            transaction.Data.ServerName = "HASystem";
            transaction.Data.MyIP = adapterIp;
            transaction.Data.RouterIP = IPAddress.None;
            transaction.Data.LogServerIP = "0.0.0.0";
            transaction.Data.DomainIP = IPAddress.None;
            dhcpServer.SendDHCPMessage(DHCPMessageType.DHCPOFFER, transaction);
        }

        private void DHCPRequest(DHCPTransaction transaction, string macId)
        {
            //announced so then send the offer
            transaction.Data.IPAddr = GetIpForDevice(macId);
            transaction.Data.SubMask = IPAddress.Parse("255.255.255.0");
            transaction.Data.LeaseTime = 2000;
            transaction.Data.ServerName = "HASystem";
            transaction.Data.MyIP = adapterIp;
            transaction.Data.RouterIP = IPAddress.None;
            transaction.Data.LogServerIP = "0.0.0.0";
            transaction.Data.DomainIP = IPAddress.None;
            dhcpServer.SendDHCPMessage(DHCPMessageType.DHCPACK, transaction);
        }

        private IPAddress GetIpForDevice(string macAddress)
        {
            PhysicalAddress mac = PhysicalAddress.Parse(macAddress);

            lock (dhcpServer)
            {
                Device device = House.Devices.FirstOrDefault(p => Object.Equals(mac, p.MACAddress));
                if (device == null)
                {
                    device = new Device();
                    device.MACAddress = mac;
                    device.IPAddress = IPAddress.None;
                    device.Name = "Autodetect-" + mac;
                    House.AddDevice(device);
                }
                else
                {
                    if (!Object.Equals(device.IPAddress, IPAddress.None))
                    {
                        return device.IPAddress;
                    }
                }

                device.IPAddress = GetNextIp();

                return device.IPAddress;
            }
        }

        private IPAddress GetNextIp()
        {
            IPAddress ipAddress;
            byte[] ipAsByte;
            UInt32 parsedIpAddress;

            parsedIpAddress = DHCPService.IPAddressToLongBackwards(currentIp);
            parsedIpAddress -= 1;
            do
            {
                parsedIpAddress += 1;
                ipAsByte = new byte[4];
                ipAsByte[3] = (byte)(parsedIpAddress);
                ipAsByte[2] = (byte)(parsedIpAddress >> 8);
                ipAsByte[1] = (byte)(parsedIpAddress >> 16);
                ipAsByte[0] = (byte)(parsedIpAddress >> 24);
                ipAddress = new IPAddress(ipAsByte);

                if (Object.Equals(ipAddress, networkIpMax))
                {
                    ipAddress = networkIpMin;
                    parsedIpAddress = DHCPService.IPAddressToLongBackwards(networkIpMin);
                }

                if (House.Devices.Any(p => Object.Equals(p.IPAddress, ipAddress))) //TODO: some kind of aging
                {
                    continue;
                }
            }
            while (DHCPService.CheckAlive(ipAddress) == true);

            //reaching here means that the ip is free
            currentIp = ipAddress;

            return ipAddress;
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

            binaryIn.Outputs[0].AddConnection(and.Inputs[0]);
            src2.Outputs[0].AddConnection(and.Inputs[1]);

            BinaryOut binaryOut = new BinaryOut();
            House.AddComponent(binaryOut);
            and.Outputs[0].AddConnection(binaryOut.Inputs[0]);

            Logger logger = new Logger();
            House.AddComponent(logger);
            and.Outputs[0].AddConnection(logger.Inputs[0]);

            binaryIn.Config["test"] = "hallo";
            binaryIn.Config["test2"] = "test2";
            logger.Config["foo"] = "bar";
        }

#endif
    }
}