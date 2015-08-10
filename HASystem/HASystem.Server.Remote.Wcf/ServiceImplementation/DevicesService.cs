using AutoMapper;
using HASystem.Shared.Remote.Wcf.DataContracts;
using HASystem.Shared.Remote.Wcf.ServiceContracts;
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
        static DevicesService()
        {
            //local to plain
            Mapper.CreateMap<Logic.Device, Device>()
                .ForMember(p => p.Name, m => m.MapFrom(l => l.Name))
                .ForMember(p => p.State, m => m.MapFrom(l => l.State))
                .ForMember(p => p.MACAddress, m => m.MapFrom(l => l.MACAddress))
                .ForMember(p => p.IPAddress, m => m.MapFrom(l => l.IPAddress))
                ;
        }

        public Device[] GetAllDevices()
        {
            return Manager.Instance.Current.Devices.Select(l => Mapper.Map<Logic.Device, Device>(l)).ToArray();
        }

        public Device GetDevice(string mac)
        {
            PhysicalAddress macAddress;
            try
            {
                macAddress = PhysicalAddress.Parse(mac);
            }
            catch (FormatException ex)
            {
                throw new WebFaultException<FormatException>(ex, HttpStatusCode.BadRequest);
            }

            Logic.Device device = Manager.Instance.Current.Devices.FirstOrDefault(p => Object.Equals(p.MACAddress, macAddress));

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
                mac = PhysicalAddress.Parse(device.MACAddress);
            }
            catch (FormatException ex)
            {
                throw new WebFaultException<FormatException>(ex, HttpStatusCode.BadRequest);
            }

            //test if device already exists
            if (Manager.Instance.Current.Devices.Any(p => Object.Equals(p.MACAddress, mac)))
                throw new WebFaultException(HttpStatusCode.Conflict);
            if (String.IsNullOrWhiteSpace(device.Name))
                throw new WebFaultException<ArgumentNullException>(new ArgumentNullException("Name can not be empty"), HttpStatusCode.BadRequest);

            Logic.Device logicDevice = new Logic.Device();
            logicDevice.Name = device.Name;
            logicDevice.MACAddress = mac;

            logicDevice.State = Logic.DeviceState.Offline;
            if (!String.IsNullOrWhiteSpace(device.IPAddress))
            {
                IPAddress ip = null;
                if (IPAddress.TryParse(device.IPAddress, out ip))
                {
                    if (IPAddress.None.Equals(ip) || !Manager.Instance.Current.Devices.Any(p => Object.Equals(ip, p.IPAddress)))
                    {
                        logicDevice.IPAddress = ip;
                    }
                    else
                    {
                        throw new WebFaultException<ArgumentException>(new ArgumentException("ip already used by another device"), HttpStatusCode.Conflict);
                    }
                }
                else
                {
                    throw new WebFaultException<ArgumentException>(new ArgumentException("invalid ip"), HttpStatusCode.BadRequest);
                }
            }
            else
            {
                logicDevice.IPAddress = IPAddress.None;
            }

            Manager.Instance.Current.AddDevice(logicDevice);
        }

        public void SaveDevice(string mac, Device device)
        {
            if (device == null)
                throw new WebFaultException(HttpStatusCode.BadRequest);

            PhysicalAddress macAddress;
            try
            {
                macAddress = PhysicalAddress.Parse(mac);
            }
            catch (FormatException ex)
            {
                throw new WebFaultException<FormatException>(ex, HttpStatusCode.BadRequest);
            }
            PhysicalAddress macAddressNew;
            try
            {
                macAddressNew = PhysicalAddress.Parse(device.MACAddress);
            }
            catch (FormatException ex)
            {
                throw new WebFaultException<FormatException>(ex, HttpStatusCode.BadRequest);
            }

            Logic.Device logicDevice = Manager.Instance.Current.Devices.FirstOrDefault(p => Object.Equals(p.MACAddress, macAddress));
            if (logicDevice == null)
                throw new WebFaultException(HttpStatusCode.NotFound);
            if (String.IsNullOrWhiteSpace(device.Name))
                throw new WebFaultException<ArgumentNullException>(new ArgumentNullException("Name can not be empty"), HttpStatusCode.BadRequest);

            if (!Object.Equals(macAddressNew, logicDevice.MACAddress)) //do we allow this?
            {
                if (Manager.Instance.Current.Devices.Any(p => Object.Equals(p.MACAddress, macAddressNew)))
                {
                    throw new WebFaultException<ArgumentException>(new ArgumentException("mac-address is already used by another device"), HttpStatusCode.Conflict);
                }
            }

            logicDevice.Name = device.Name;
            logicDevice.MACAddress = macAddressNew;
            //TODO: ip-address modification
        }

        public void DeleteDevice(string mac)
        {
            PhysicalAddress macAddress;
            try
            {
                macAddress = PhysicalAddress.Parse(mac);
            }
            catch (FormatException ex)
            {
                throw new WebFaultException<FormatException>(ex, HttpStatusCode.BadRequest);
            }

            Logic.Device device = Manager.Instance.Current.Devices.FirstOrDefault(p => Object.Equals(p.MACAddress, macAddress));
            if (device == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            Manager.Instance.Current.RemoveDevice(device);
        }
    }
}