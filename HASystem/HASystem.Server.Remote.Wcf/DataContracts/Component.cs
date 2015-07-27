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
    public class Component : IExtensibleDataObject
    {
        static Component()
        {
            Mapper.CreateMap<Logic.LogicComponent, Component>()
                .ForMember(p => p.Id, m => m.MapFrom(l => l.Id))
                .ForMember(p => p.ComponentType, m => m.MapFrom(l => l.ComponentType))
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
        public int Id
        {
            get;
            set;
        }

        [DataMember]
        public string ComponentType
        {
            get;
            set;
        }

        [DataMember]
        private Dictionary<string, string> Config
        {
            get;
            set;
        }

        public Component()
        {
            Config = new Dictionary<string, string>();
        }
    }
}