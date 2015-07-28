using AutoMapper;
using HASystem.Server.Logic;
using HASystem.Server.Remote.Wcf.DataContracts;
using HASystem.Server.Remote.Wcf.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
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
            int idI = 0;
            if (!Int32.TryParse(id, out idI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            LogicComponent component = Manager.Instance.House.Components.Where(p => p.Id == idI).FirstOrDefault();
            if (component == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            return Mapper.Map<DataContracts.Component>(component);
        }

        public string[] GetSupportedComponentTypes()
        {
            return Manager.Instance.ComponentsFactory.Components.Select(p => p.ToString()).ToArray();
        }

        public int CreateComponent(Component component)
        {
            if (component == null)
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Guid componentType;
            if (!Guid.TryParse(component.ComponentType, out componentType))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.LogicComponent logicComponent;
            try
            {
                logicComponent = Manager.Instance.ComponentsFactory.CreateComponent(componentType);
            }
            catch (ArgumentException ex)
            {
                throw new WebFaultException<ArgumentException>(ex, HttpStatusCode.BadRequest);
            }

            if (component.Config != null)
            {
                ComponentConfig config = new ComponentConfig(logicComponent, new Dictionary<string, string>(component.Config));
                logicComponent.Config = config;
            }

            //TODO: save position

            Manager.Instance.House.AddComponent(logicComponent);

            return logicComponent.Id;
        }

        public void SaveComponent(string id, Component component)
        {
            if (component == null)
                throw new WebFaultException(HttpStatusCode.BadRequest);

            int idI = 0;
            if (!Int32.TryParse(id, out idI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            if (component.Id != idI)
                throw new WebFaultException(HttpStatusCode.BadRequest); //we don't allow to modify the id

            Logic.LogicComponent logicComponent = Manager.Instance.House.Components.Where(p => p.Id == idI).FirstOrDefault();
            if (logicComponent == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            if (component.Config != null)
            {
                //change config
                ComponentConfig config = new ComponentConfig(logicComponent, new Dictionary<string, string>(component.Config));
                logicComponent.Config = config;
            }
            if (component.Position != null)
            {
                //TODO: save position
            }

            //TODO: do we allow connections modifications here?
        }

        public void DeleteComponent(string id)
        {
            int idI = 0;
            if (!Int32.TryParse(id, out idI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.LogicComponent logicComponent = Manager.Instance.House.Components.Where(p => p.Id == idI).FirstOrDefault();
            if (logicComponent == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            Manager.Instance.House.RemoveComponent(logicComponent);
        }

        public Value GetOutputValue(string id, string index)
        {
            int idI = 0;
            if (!Int32.TryParse(id, out idI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            int indexI = 0;
            if (!Int32.TryParse(index, out indexI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.LogicComponent logicComponent = Manager.Instance.House.Components.Where(p => p.Id == idI).FirstOrDefault();
            if (logicComponent == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            Logic.LogicOutput output = logicComponent.Outputs.Where(p => p.Index == indexI).FirstOrDefault();
            if (output == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            var value = output.Value;

            return new Value() { Content = new byte[1024] };
        }
    }
}