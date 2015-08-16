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
            Mapping.Init();
        }

        public string[] GetSupportedComponentTypes()
        {
            return Manager.Instance.ComponentsFactory.Components.Select(p => p.ToString()).ToArray();
        }
    }
}