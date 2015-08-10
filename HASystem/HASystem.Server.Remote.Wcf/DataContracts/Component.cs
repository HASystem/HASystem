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
                .ForMember(p => p.Config, m => m.MapFrom(l => new Dictionary<string, string>(l.Config)))
                .ForMember(p => p.Position, m => m.MapFrom(l => new Point())) //TODO: map to position
                .ForMember(p => p.Connections, m => m.MapFrom(l => l.Outputs.SelectMany(o => o.Connections.Select(c => new ComponentConnection(l.Id, o.Index, c.Component.Id, c.Index))).ToArray()))
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

        [DataMember(IsRequired = false)]
        public string ComponentType
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public Dictionary<string, string> Config
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public Point Position
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public ComponentConnection[] Connections
        {
            get;
            set;
        }
    }
}