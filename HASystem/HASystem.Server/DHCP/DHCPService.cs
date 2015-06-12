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
        private string startIp;
        private string macMask;
        private string adapterIp;
        private Thread dhcpServerThread = null;
        #endregion

        #region public class methods
        public static bool CheckAlive(string ipAddress)
        {
            Ping pingSender = new Ping();
            IPAddress address;
            PingReply reply;

            try
            {
                address = IPAddress.Parse(ipAddress);
                reply = pingSender.Send(address, 100);
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
            catch (Exception)
            {
                // TODO: handle exception
                return false;
            }
            finally
            {
                if (pingSender != null)
                {
                    pingSender.Dispose();
                }

                pingSender = null;
                address = null;
                reply = null;
            }
        }
        #endregion

        #region private class methods
        private static uint IPAddressToLongBackwards(string ipAddress)
        {
            IPAddress parsedIpAddress = IPAddress.Parse(ipAddress);
            byte[] ipAsByte = parsedIpAddress.GetAddressBytes();


            uint ip = (uint)ipAsByte[0] << 24;
            ip += (uint)ipAsByte[1] << 16;
            ip += (uint)ipAsByte[2] << 8;
            ip += (uint)ipAsByte[3];

            return ip;
        }
        #endregion

        #region public methods
        public void StartService()
        {
            //allow data from all network cards
            adapterIp = "0.0.0.0";
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
        #endregion

        #region private methods
        private string GetIpAddress()
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
                while (CheckAlive(ipAddress.ToString()) == true);
                //reaching here means that the ip is free

                return ipAddress.ToString();
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
                transaction.Data.SubMask = "255.255.0.0";
                transaction.Data.LeaseTime = 2000;
                transaction.Data.ServerName = "Small DHCP Server";
                transaction.Data.MyIP = adapterIp;
                transaction.Data.RouterIP = "0.0.0.0";
                transaction.Data.LogServerIP = "0.0.0.0";
                transaction.Data.DomainIP = "0.0.0.0";
                str = "IP requested for Mac: " + macId;

            }
            else
            {
                str = "Mac: " + macId + " is not part of the mask!";
            }
            dhcpServer.SendDHCPMessage(DHCPMessageType.DHCPOFFER, transaction);

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
                transaction.Data.SubMask = "255.255.0.0";
                transaction.Data.LeaseTime = 2000;
                transaction.Data.ServerName = "tiny DHCP Server";
                transaction.Data.MyIP = adapterIp;
                transaction.Data.RouterIP = "0.0.0.0";
                transaction.Data.LogServerIP = "0.0.0.0";
                transaction.Data.DomainIP = "0.0.0.0";
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
        #endregion
    }

}
