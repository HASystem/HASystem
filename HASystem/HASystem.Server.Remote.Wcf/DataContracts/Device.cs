using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using System.Text;

using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf.DataContracts
{
    [DataContract]
    public class Device : IExtensibleDataObject
    {
        static Device()
        {
            //local to plain
            Mapper.CreateMap<Logic.Device, Device>()
                .ForMember(p => p.Name, m => m.MapFrom(l => l.Name))
                .ForMember(p => p.State, m => m.MapFrom(l => l.State))
                .ForMember(p => p.MACAddress, m => m.MapFrom(l => l.MACAddress))
                .ForMember(p => p.IPAddress, m => m.MapFrom(l => l.IPAddress))
                ;
        }

        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string State
        {
            get;
            set;
        }

        public Logic.DeviceState RealState
        {
            get
            {
                Logic.DeviceState value = Logic.DeviceState.Reserved;
                if (Enum.TryParse<Logic.DeviceState>(State, out value))
                {
                    return value;
                }
                return Logic.DeviceState.Reserved;
            }
            set
            {
                State = value.ToString();
            }
        }

        [DataMember]
        public string MACAddress
        {
            get;
            set;
        }

        [DataMember]
        public string IPAddress
        {
            get;
            set;
        }
    }
}