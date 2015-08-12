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
    }
}