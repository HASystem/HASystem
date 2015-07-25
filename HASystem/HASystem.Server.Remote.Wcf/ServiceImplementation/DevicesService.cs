using AutoMapper;
using HASystem.Server.Remote.Wcf.DataContracts;
using HASystem.Server.Remote.Wcf.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceModel.Web;

using System.Text;

using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf.ServiceImplementation
{
    public class DevicesService : IDevicesService
    {
        public Device[] GetAllDevices()
        {
            return Manager.Instance.House.Devices.Select(l => Mapper.Map<Logic.Device, Device>(l)).ToArray();
        }

        public Device GetDevice(string mac)
        {
            PhysicalAddress macAddress;
            try
            {
                macAddress = PhysicalAddress.Parse(mac); //this could fail
            }
            catch (FormatException ex)
            {
                throw new WebFaultException<FormatException>(ex, HttpStatusCode.BadRequest);
            }

            Logic.Device device = Manager.Instance.House.Devices.Where(p => Object.Equals(p.MACAddress, macAddress)).FirstOrDefault();

            if (device == null)
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<Logic.Device, Device>(device);
        }

        public void CreateDevice(Device device)
        {
            if (device == null)
                throw new WebFaultException(HttpStatusCode.BadRequest);

            PhysicalAddress mac;
            try
            {
                mac = PhysicalAddress.Parse(device.MACAddress); //this could fail
            }
            catch (FormatException ex)
            {
                throw new WebFaultException<FormatException>(ex, HttpStatusCode.BadRequest);
            }

            //test if device already exists
            if (Manager.Instance.House.Devices.Where(p => Object.Equals(p.MACAddress, mac)).FirstOrDefault() != null)
                throw new WebFaultException(HttpStatusCode.Conflict);
            if (String.IsNullOrWhiteSpace(device.Name))
                throw new WebFaultException<ArgumentNullException>(new ArgumentNullException("Name can not be empty"), HttpStatusCode.BadRequest);

            Logic.Device logicDevice = new Logic.Device();
            logicDevice.Name = device.Name;
            logicDevice.MACAddress = mac;
            if (String.IsNullOrWhiteSpace(device.State) || device.RealState == Logic.DeviceState.Created)
            {
                logicDevice.State = Logic.DeviceState.Created;
                logicDevice.IPAddress = IPAddress.None;
            }
            else if (device.RealState == Logic.DeviceState.Reserved)
            {
                logicDevice.State = Logic.DeviceState.Reserved;
                IPAddress ip = null;
                if (IPAddress.TryParse(device.IPAddress, out ip))
                {
                    logicDevice.IPAddress = ip;
                }
                else
                {
                    throw new WebFaultException<ArgumentException>(new ArgumentNullException("invalid ip"), HttpStatusCode.BadRequest);
                }
            }
            else
            {
                throw new WebFaultException<ArgumentException>(new ArgumentNullException("State is not valid"), HttpStatusCode.BadRequest);
            }

            Manager.Instance.House.AddDevice(logicDevice);
        }

        public void SaveDevice(string mac, Device device)
        {
            if (device == null)
                throw new WebFaultException(HttpStatusCode.BadRequest);

            PhysicalAddress macAddress;
            try
            {
                macAddress = PhysicalAddress.Parse(device.MACAddress); //this could fail
            }
            catch (FormatException ex)
            {
                throw new WebFaultException<FormatException>(ex, HttpStatusCode.BadRequest);
            }

            Logic.Device logicDevice = Manager.Instance.House.Devices.Where(p => Object.Equals(p.MACAddress, macAddress)).FirstOrDefault();
            if (logicDevice == null)
                throw new WebFaultException(HttpStatusCode.NotFound);
            if (String.IsNullOrWhiteSpace(device.Name))
                throw new WebFaultException<ArgumentNullException>(new ArgumentNullException("Name can not be empty"), HttpStatusCode.BadRequest);

            logicDevice.Name = device.Name;
            if (!Object.Equals(device.MACAddress, logicDevice.MACAddress)) //do we allow this?
            {
                if (Manager.Instance.House.Devices.Where(p => Object.Equals(p.MACAddress, macAddress)).FirstOrDefault() != null)
                {
                    throw new WebFaultException<ArgumentException>(new ArgumentException("mac-address is already used by another device"), HttpStatusCode.Conflict);
                }
                logicDevice.MACAddress = PhysicalAddress.Parse(device.MACAddress);
            }
            //TODO: ip-address modification
        }

        public void DeleteDevice(string mac)
        {
            PhysicalAddress macAddress;
            try
            {
                macAddress = PhysicalAddress.Parse(mac); //this could fail
            }
            catch (FormatException ex)
            {
                throw new WebFaultException<FormatException>(ex, HttpStatusCode.BadRequest);
            }

            Manager.Instance.House.RemoveDevice(macAddress);
        }
    }
}