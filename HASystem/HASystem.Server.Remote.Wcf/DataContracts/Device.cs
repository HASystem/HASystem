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

        internal static void InitMapping()
        {
            //fake because we use the static ctor for init. but we need a call to the class because otherwise the map wouldn't created
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

        [DataMember(IsRequired = false)]
        public string State
        {
            get;
            set;
        }

        [DataMember]
        public string MACAddress
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public string IPAddress
        {
            get;
            set;
        }
    }
}