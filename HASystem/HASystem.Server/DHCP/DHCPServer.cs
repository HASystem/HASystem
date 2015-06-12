/* DHCP CLASS
 * A MAC ADDRESS REQUESTS AN IP ADDRESS
 * CHECK THE MAC ADDRESS AND SEE IF THE MASKS AND TOGETHER
 * MAC ALLOWED ASSIGN AN IP ADDRESS
 * PING TO SEE IF THE BASE IP ADDRESS IS IN USE
 * IF IT IS IN USE INCREMENT THE IP ADDRESS AND, PING AND IF ALLOWED TO ASSIGN
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace HASystem.Server.DHCP
{
    public class DHCPServer
    {
        #region fields
        // udp send and receive classs
        private UDPAsync udp;
        private string NetCard;
        #endregion

        #region events
        public event AnnouncedEventHandler Announced;
        public event RequestEventHandler Request;
        #endregion

        #region delegates
        public delegate void AnnouncedEventHandler(DHCPTransaction d_DHCP, string MacId);
        public delegate void ReleasedEventHandler();
        public delegate void RequestEventHandler(DHCPTransaction d_DHCP, string MacId);
        public delegate void AssignedEventHandler(string IPAdd, string MacID);
        #endregion

        #region ctor
        public DHCPServer(string netCard)
        {
            NetCard = netCard;
        }
        #endregion

        #region public methods
        public void StartDHCPServer()
        {
            // Function to start the DHCP server
            // Port 67 to receive, 68 to send
            try
            {   
                // Start the DHCP server
                udp = new UDPAsync(67, 68, NetCard);
                udp.dataReceived += new UDPAsync.DataReceivedEventHandler(UDPDataReceived);
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
        }

        public void SendDHCPMessage(DHCPMessageType messageType, DHCPTransaction transaction)
        {
            //mac announced itself, established IP etc....
            //send the offer to the mac

            byte[] subnet;
            byte[] hostId;
            byte[] dataToSend;


            //we shall leave everything as is structure wise
            //shall CHANGE the type to OFFER
            //shall set the client's IP-Address
            try
            {
                //change message type to reply
                transaction.Message.D_op = 2;

                //subnet
                subnet = IPAddress.Parse(transaction.Data.SubMask).GetAddressBytes();

                //create your ip address
                transaction.Message.D_yiaddr = IPAddress.Parse(transaction.Data.IPAddr).GetAddressBytes();

                //Host ID
                hostId = System.Text.Encoding.ASCII.GetBytes(transaction.Data.ServerName);
                CreateOptionStruct(ref transaction, messageType);

                //send the data to the unit
                dataToSend = BuildDataStructure(transaction.Message);
                udp.SendData(dataToSend);
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
            finally
            {
                subnet = null;
                //LeaseTime= null;
                hostId = null;

                dataToSend = null;
            }
        }
       
        public void Dispose()
        {
            if (udp != null) { udp.StopListener(); }
            udp = null;
        }
        #endregion

        #region private methods
        private string ByteToString(byte[] dataByte, byte length)
        {
            string dataString;

            try
            {
                dataString = string.Empty;
                if (dataByte != null)
                {
                    for (int i = 0; i < length; i++)
                    {
                        dataString += dataByte[i].ToString("X2");
                    }
                }
                return dataString;
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
                return string.Empty;
            }
            finally
            {
                dataString = null;
            }
        }

        private DHCPMessageType GetMessageType(DHCPTransaction transaction)
        {
            // Get the Message type, located in the options stream

            byte[] data;

            try
            {
                data = GetOptionData(DHCPOption.DHCPMessageTYPE, transaction);
                if (data != null)
                {
                    return (DHCPMessageType)data[0];
                }
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
            return 0;
        }

        private byte[] GetOptionData(DHCPOption type, DHCPTransaction transaction)
        {
            // Pass the option type that you require
            // Parse the option data
            // Return the data in a byte of what we need

            int id = 0;
            byte dataId = 0;
            byte dataLength = 0;
            byte[] dataDump;

            try
            {
                id = (int)type;
                //loop through look for the bit that states that the identifier is there
                for (int i = 0; i < transaction.Message.D_options.Length; i++)
                {
                    //at the start we have the code + length
                    //i has the code, i+1 = length of data, i+1+n = data skip
                    dataId = transaction.Message.D_options[i];
                    if (dataId == id)
                    {
                        dataLength = transaction.Message.D_options[i + 1];
                        dataDump = new byte[dataLength];
                        Array.Copy(transaction.Message.D_options, i + 2, dataDump, 0, dataLength);
                        return dataDump;
                    }
                    else
                    {
                        // Length of code
                        dataLength = transaction.Message.D_options[i + 1];
                        i += 1 + dataLength;
                    }
                }
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
            finally
            {
                dataDump = null;
            }
            return null;
        }

        private void UDPDataReceived(byte[] data, IPEndPoint endPoint)
        {
            DHCPTransaction transaction;
            DHCPMessageType messageType;
            string macId;

            try
            {
                transaction = new DHCPTransaction(data);

                //data is now in the structure, get the msg type
                messageType = GetMessageType(transaction);
                macId = ByteToString(transaction.Message.D_chaddr, transaction.Message.D_hlen);

                switch (messageType)
                {
                    case DHCPMessageType.DHCPDISCOVER:
                        // a Mac has requested an IP
                        // discover Msg Has been sent
                        Announced(transaction, macId);
                        break;
                    case DHCPMessageType.DHCPREQUEST:
                        Request(transaction, macId);
                        break;
                }

            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
        }

        private void CreateOptionStruct(ref DHCPTransaction transaction, DHCPMessageType optionReplayMessage)
        {
            byte[] parameterRequestList;
            byte[] ipAddress;
            byte[] leaseTime;
            byte[] serverIp;

            try
            {
                //we look for the parameter request list
                parameterRequestList = GetOptionData(DHCPOption.ParameterRequestList, transaction);
                //erase the options array, and set the message type to ack
                transaction.Message.D_options = null;
                CreateOptionElement(DHCPOption.DHCPMessageTYPE, new byte[] { (byte)optionReplayMessage }, ref transaction.Message.D_options);
                //server identifier, my IP
                serverIp = IPAddress.Parse(transaction.Data.MyIP).GetAddressBytes();
                CreateOptionElement(DHCPOption.ServerIdentifier, serverIp, ref transaction.Message.D_options);

                // parameterRequestList contains the option data in a byte that is requested by the unit
                foreach (byte i in parameterRequestList)
                {
                    ipAddress = null;
                    switch ((DHCPOption)i)
                    {
                        case DHCPOption.SubnetMask:
                            ipAddress = IPAddress.Parse(transaction.Data.SubMask).GetAddressBytes();
                            break;
                        case DHCPOption.Router:
                            ipAddress = IPAddress.Parse(transaction.Data.RouterIP).GetAddressBytes();
                            break;
                        case DHCPOption.DomainNameServer:
                            ipAddress = IPAddress.Parse(transaction.Data.DomainIP).GetAddressBytes();
                            break;
                        case DHCPOption.DomainName:
                            ipAddress = System.Text.Encoding.ASCII.GetBytes(transaction.Data.ServerName);
                            break;
                        case DHCPOption.ServerIdentifier:
                            ipAddress = IPAddress.Parse(transaction.Data.MyIP).GetAddressBytes();
                            break;
                        case DHCPOption.LogServer:
                            ipAddress = System.Text.Encoding.ASCII.GetBytes(transaction.Data.LogServerIP);
                            break;
                        case DHCPOption.NetBIOSoverTCPIPNameServer:
                            break;

                    }

                    if (ipAddress != null)
                    {
                        CreateOptionElement((DHCPOption)i, ipAddress, ref transaction.Message.D_options);
                    }
                }

                //lease time
                leaseTime = new byte[4];
                leaseTime[3] = (byte)(transaction.Data.LeaseTime);
                leaseTime[2] = (byte)(transaction.Data.LeaseTime >> 8);
                leaseTime[1] = (byte)(transaction.Data.LeaseTime >> 16);
                leaseTime[0] = (byte)(transaction.Data.LeaseTime >> 24);

                CreateOptionElement(DHCPOption.IPAddressLeaseTime, leaseTime, ref transaction.Message.D_options);
                CreateOptionElement(DHCPOption.RenewalTimeValue_T1, leaseTime, ref transaction.Message.D_options);
                CreateOptionElement(DHCPOption.RebindingTimeValue_T2, leaseTime, ref transaction.Message.D_options);

                //create the end option
                Array.Resize(ref transaction.Message.D_options, transaction.Message.D_options.Length + 1);
                Array.Copy(new byte[] { 255 }, 0, transaction.Message.D_options, transaction.Message.D_options.Length - 1, 1);
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
            finally
            {
                leaseTime = null;
                parameterRequestList = null;
                ipAddress = null;

            }
        }

        private byte[] BuildDataStructure(DHCPMessage message)
        {
            //function to build the data structure to a byte array
            byte[] mArray;

            try
            {
                mArray = new byte[0];
                AddOptionElement(new byte[] { message.D_op }, ref mArray);
                AddOptionElement(new byte[] { message.D_htype }, ref mArray);
                AddOptionElement(new byte[] { message.D_hlen }, ref mArray);
                AddOptionElement(new byte[] { message.D_hops }, ref mArray);
                AddOptionElement(message.D_xid, ref mArray);
                AddOptionElement(message.D_secs, ref mArray);
                AddOptionElement(message.D_flags, ref mArray);
                AddOptionElement(message.D_ciaddr, ref mArray);
                AddOptionElement(message.D_yiaddr, ref mArray);
                AddOptionElement(message.D_siaddr, ref mArray);
                AddOptionElement(message.D_giaddr, ref mArray);
                AddOptionElement(message.D_chaddr, ref mArray);
                AddOptionElement(message.D_sname, ref mArray);
                AddOptionElement(message.D_file, ref mArray);
                AddOptionElement(message.M_Cookie, ref mArray);
                AddOptionElement(message.D_options, ref mArray);
                return mArray;
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                mArray = null;
            }

        }

        private void AddOptionElement(byte[] source, ref byte[] target)
        {
            try
            {
                if (target != null)
                {
                    Array.Resize(ref target, target.Length + source.Length);
                }
                else
                {
                    Array.Resize(ref target, source.Length);
                }

                Array.Copy(source, 0, target, target.Length - source.Length, source.Length);
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
        }

        private void CreateOptionElement(DHCPOption option, byte[] data, ref byte[] target)
        {
            //create an option message 
            //shall always append at the end of the message

            byte[] tOption;

            try
            {
                tOption = new byte[data.Length + 2];
                //add the code, and data length
                tOption[0] = (byte)option;
                tOption[1] = (byte)data.Length;
                //add the code to put in
                Array.Copy(data, 0, tOption, 2, data.Length);
                //copy the data to the out array
                if (target == null)
                    Array.Resize(ref target, (int)tOption.Length);
                else
                    Array.Resize(ref target, target.Length + tOption.Length);
                Array.Copy(tOption, 0, target, target.Length - tOption.Length, tOption.Length);
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
