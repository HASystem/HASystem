using AutoMapper;
using HASystem.Shared.Remote.Wcf.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf
{
    public static class Mapping
    {
        static Mapping()
        {
            //local to plain
            Mapper.CreateMap<Logic.Device, Device>()
                .ForMember(p => p.Name, m => m.MapFrom(l => l.Name))
                .ForMember(p => p.State, m => m.MapFrom(l => l.State))
                .ForMember(p => p.MACAddress, m => m.MapFrom(l => l.MACAddress))
                .ForMember(p => p.IPAddress, m => m.MapFrom(l => l.IPAddress))
                ;

            Mapper.CreateMap<Logic.House, House>()
                .ForMember(p => p.Name, m => m.MapFrom(l => l.Name))
            ;

            Mapper.CreateMap<Logic.LogicComponent, Component>()
                .ForMember(p => p.Id, m => m.MapFrom(l => l.Id))
                .ForMember(p => p.ComponentType, m => m.MapFrom(l => l.ComponentType))
                .ForMember(p => p.Config, m => m.MapFrom(l => new Dictionary<string, string>(l.Config)))
                .ForMember(p => p.Position, m => m.MapFrom(l => new Point())) //TODO: map to position
                .ForMember(p => p.Connections, m => m.MapFrom(l => l.Outputs.SelectMany(o => o.Connections.Select(c => new ComponentConnection(l.Id, o.Index, c.Component.Id, c.Index))).ToArray()))
            ;
        }

        public static void Init()
        {
            //fake call to call the static constructor
        }
    }
}