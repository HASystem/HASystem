using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HASystem.Server.DHCP
{
    public class DHCPService
    {
        #region fields

        private DHCPServer dhcpServer;
        private IPAddress startIp = IPAddress.Parse("10.222.0.0");
        private string macMask = "";
        private IPAddress adapterIp = IPAddress.Parse("0.0.0.0");
        private Thread dhcpServerThread = null;

        #endregion fields

        #region public class methods

        public static bool CheckAlive(IPAddress address)
        {
            try
            {
                using (Ping pingSender = new Ping())
                {
                    PingReply reply = pingSender.Send(address, 100);
                    if (reply.Status == IPStatus.Success)
                    {
                        // TODO: handle reply
                        //Console.WriteLine("Address: {0}", reply.Address.ToString());
                        //Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                        //Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                        //Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                        //Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                        return true;
                    }
                    else
                    {
                        // TODO: handle reply status
                        // Console.WriteLine(reply.Status);
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                // TODO: handle exception
                return false;
            }
        }

        #endregion public class methods

        #region private class methods

        public static uint IPAddressToLongBackwards(IPAddress ipAddress)
        {
            byte[] ipAsByte = ipAddress.GetAddressBytes();

            uint ip = (uint)ipAsByte[0] << 24;
            ip += (uint)ipAsByte[1] << 16;
            ip += (uint)ipAsByte[2] << 8;
            ip += (uint)ipAsByte[3];

            return ip;
        }

        #endregion private class methods

        #region public methods

        public void StartService()
        {
            //allow data from all network cards
            dhcpServer = new DHCPServer(adapterIp);
            dhcpServer.Announced += new DHCPServer.AnnouncedEventHandler(DHCPAnnounced);
            dhcpServer.Request += new DHCPServer.RequestEventHandler(DHCPRequest);

            dhcpServerThread = new Thread(dhcpServer.StartDHCPServer);
            dhcpServerThread.Start();
        }

        public void StopService()
        {
            dhcpServer.Dispose();
        }

        #endregion public methods

        #region private methods

        private IPAddress GetIpAddress()
        {
            IPAddress ipAddress;
            byte[] ipAsByte;
            UInt32 parsedIpAddress;

            try
            {
                parsedIpAddress = IPAddressToLongBackwards(startIp);
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
                    // yy = IPAddress.HostToNetworkOrder(ii);
                }
                while (CheckAlive(ipAddress) == true);
                //reaching here means that the ip is free

                return ipAddress;
            }
            catch
            {
                return null;
            }
        }

        private void DHCPAnnounced(DHCPTransaction transaction, string macId)
        {
            string str = string.Empty;

            if (macId.StartsWith(macMask) == true)
            {
                //options should be filled with valid data
                transaction.Data.IPAddr = GetIpAddress();
                transaction.Data.SubMask = IPAddress.Parse("255.255.0.0");
                transaction.Data.LeaseTime = 2000;
                transaction.Data.ServerName = "HASystem";
                transaction.Data.MyIP = adapterIp;
                transaction.Data.RouterIP = IPAddress.Parse("0.0.0.0");
                transaction.Data.LogServerIP = "0.0.0.0";
                transaction.Data.DomainIP = IPAddress.Parse("0.0.0.0");
                str = "IP requested for Mac: " + macId;
                dhcpServer.SendDHCPMessage(DHCPMessageType.DHCPOFFER, transaction);
            }
            else
            {
                str = "Mac: " + macId + " is not part of the mask!";
            }

            // TODO: handle str
            // str contains the ip
        }

        private void DHCPRequest(DHCPTransaction transaction, string macId)
        {
            string str = string.Empty;
            if (macId.StartsWith(macMask) == true)
            {
                //announced so then send the offer
                transaction.Data.IPAddr = GetIpAddress();
                transaction.Data.SubMask = IPAddress.Parse("255.255.0.0");
                transaction.Data.LeaseTime = 2000;
                transaction.Data.ServerName = "HASystem";
                transaction.Data.MyIP = adapterIp;
                transaction.Data.RouterIP = IPAddress.Parse("0.0.0.0");
                transaction.Data.LogServerIP = "0.0.0.0";
                transaction.Data.DomainIP = IPAddress.Parse("0.0.0.0");
                dhcpServer.SendDHCPMessage(DHCPMessageType.DHCPACK, transaction);
                str = "IP " + transaction.Data.IPAddr + " Assigned to Mac: " + macId;
            }
            else
            {
                str = "Mac: " + macId + " is not part of the mask!";
            }

            // TODO: handle str
            // str contains the ip
        }

        #endregion private methods
    }
}