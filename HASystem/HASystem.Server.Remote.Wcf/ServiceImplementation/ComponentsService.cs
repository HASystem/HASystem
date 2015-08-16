using AutoMapper;
using HASystem.Server.Logic;
using HASystem.Shared.Remote.Wcf.DataContracts;
using HASystem.Shared.Remote.Wcf.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf.ServiceImplementation
{
    [HostedService("Components")]
    public class ComponentsService : IComponentsService
    {
        static ComponentsService()
        {
            Mapper.CreateMap<Logic.LogicComponent, Component>()
                .ForMember(p => p.Id, m => m.MapFrom(l => l.Id))
                .ForMember(p => p.ComponentType, m => m.MapFrom(l => l.ComponentType))
                .ForMember(p => p.Config, m => m.MapFrom(l => new Dictionary<string, string>(l.Config)))
                .ForMember(p => p.Position, m => m.MapFrom(l => new Point())) //TODO: map to position
                .ForMember(p => p.Connections, m => m.MapFrom(l => l.Outputs.SelectMany(o => o.Connections.Select(c => new ComponentConnection(l.Id, o.Index, c.Component.Id, c.Index))).ToArray()))
            ;
        }

        public string[] GetSupportedComponentTypes()
        {
            return Manager.Instance.ComponentsFactory.Components.Select(p => p.ToString()).ToArray();
        }
    }
}