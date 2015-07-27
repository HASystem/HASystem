/*
 * clsUDP
 * shall start a listner, and raise an event every time data arrives on a port
 * shall also be able to send data via udp protocol
 * .Dispose shall remove all resources associated with the class
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HASystem.Server.DHCP
{
    public class UDPAsync
    {
        #region fields

        private Int32 portToListen = 0;
        private Int32 portToSend = 0;
        private IPAddress endpointIp;
        private bool isListening;

        // callbacks for send/receive
        private UDPState callback;

        #endregion fields

        #region events

        public delegate void DataReceivedEventHandler(byte[] data, IPEndPoint endPoint);

        public event DataReceivedEventHandler dataReceived;

        public delegate void ErrorEventHandler(string message);

        #endregion events

        #region ctor

        public UDPAsync()
        {
            isListening = false;
        }

        public UDPAsync(Int32 portToListen, Int32 portToSend, IPAddress endpointIp)
        {
            try
            {
                isListening = false;
                this.portToListen = portToListen;
                this.portToSend = portToSend;
                this.endpointIp = endpointIp;

                StartListener();
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
        }

        ~UDPAsync()
        {
            try
            {
                StopListener();

                if (callback.Client != null)
                {
                    callback.Client.Close();
                }

                callback.Client = null;
                callback.EndPoint = null;
            }
            catch (Exception)
            {
                // TODO: handle exception
                //Console.WriteLine(ex.Message);
            }
        }

        #endregion ctor

        #region public methods

        public void SendData(byte[] data)
        {
            //function to send data as a byte stream to a remote socket
            // modified to work as a callback rather than a block

            try
            {
                callback.Client.BeginSend(data, data.Length, "255.255.255.255", portToSend, new AsyncCallback(OnDataSent), callback);
            }
            catch (Exception)
            {
                // TODO: handle exception
                // Console.WriteLine(ex.Message);
            }
        }

        public void StopListener()
        {
            //stop the listener thread
            try
            {
                isListening = false;

                if (callback.Client != null)
                {
                    callback.Client.Close();
                }

                callback.Client = null;
                callback.EndPoint = null;
            }
            catch (Exception)
            {
                // TODO: handle exception
                //Console.WriteLine(ex.Message);
            }
        }

        #endregion public methods

        #region private methods

        private void OnDataSent(IAsyncResult asyn)
        {
            // This is the call back function, which will be invoked when a client is connected

            try
            {
                //get the data
                UdpClient client = ((UDPState)asyn.AsyncState).Client;
                // stop the send call back
                client.EndSend(asyn);
            }
            catch (Exception)
            {
                if (isListening == true)
                {
                    // TODO: handle exception
                    //Console.WriteLine(ex.Message);
                }
            }
        }

        private void InitializeListnerCallBack()
        {
            //function to start the listener call back everytime something is recieved
            try
            {
                // start receive callback
                callback.Client.BeginReceive(new AsyncCallback(OnDataReceived), callback);
            }
            catch (Exception)
            {
                if (isListening == true)
                {
                    // TODO: handle exception
                    //Console.WriteLine(ex.Message);
                }
            }
        }

        private void OnDataReceived(IAsyncResult asyn)
        {
            // This is the call back function, which will be invoked when a client is connected
            Byte[] receiveBytes;
            UdpClient client;
            IPEndPoint endPoint;

            try
            {
                client = (UdpClient)((UDPState)(asyn.AsyncState)).Client;
                endPoint = (IPEndPoint)((UDPState)(asyn.AsyncState)).EndPoint;

                receiveBytes = client.EndReceive(asyn, ref endPoint);
                //raise the event with the data received
                dataReceived(receiveBytes, endPoint);
            }
            catch (Exception)
            {
                if (isListening == true)
                {
                    // TODO: handle exception
                    //Console.WriteLine(ex.Message);
                }
            }
            finally
            {
                client = null;
                endPoint = null;
                receiveBytes = null;
                // recall the call back
                InitializeListnerCallBack();
            }
        }

        private void StartListener()
        {
            //function to start the listener
            //if the the listner is active, destroy it and restart
            // shall mark the flag that the listner is active

            IPEndPoint ipLocalEndPoint;

            try
            {
                isListening = false;

                //get the ipEndPoint
                ipLocalEndPoint = new IPEndPoint(endpointIp, portToListen);

                // if the udpclient interface is active destroy
                if (callback.Client != null)
                {
                    callback.Client.Close();
                }

                callback.Client = null;
                callback.EndPoint = null;

                //re initialise the udp client
                callback = new UDPState();
                callback.EndPoint = ipLocalEndPoint;
                callback.Client = new UdpClient(ipLocalEndPoint);
                isListening = true;

                // wait for data
                InitializeListnerCallBack();
            }
            catch (Exception)
            {
                if (isListening == true)
                {
                    // TODO: handle exception
                    //Console.WriteLine(ex.Message);
                }
            }
            finally
            {
                if (callback.Client == null)
                {
                    Thread.Sleep(1000);
                    StartListener();
                }
                else
                {
                    ipLocalEndPoint = null;
                }
            }
        }

        #endregion private methods
    }
}