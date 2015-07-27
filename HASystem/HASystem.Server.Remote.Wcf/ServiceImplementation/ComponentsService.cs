using AutoMapper;
using HASystem.Server.Remote.Wcf.DataContracts;
using HASystem.Server.Remote.Wcf.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf.ServiceImplementation
{
    public class ComponentsService : IComponentsService
    {
        static ComponentsService()
        {
            Component.InitMapping();
        }

        public Component[] GetComponents()
        {
            return Manager.Instance.House.Components.Select(l => Mapper.Map<Logic.LogicComponent, Component>(l)).ToArray();
        }

        public DataContracts.Component GetSingleComponent(string id)
        {
            throw new NotImplementedException();
        }

        public string[] GetSupportedComponentTypes()
        {
            return Manager.Instance.ComponentsFactory.Components.Select(p => p.ToString()).ToArray();
        }
    }
}