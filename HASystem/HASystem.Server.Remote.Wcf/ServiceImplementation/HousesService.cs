using AutoMapper;
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
    [HostedService("Houses")]
    public class HousesService : IHousesService
    {
        static HousesService()
        {
            Mapping.Init();
        }

        private Logic.House GetHouseByName(string houseName)
        {
            if (String.IsNullOrWhiteSpace(houseName))
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }
            Logic.House house;
            if (houseName == "current")
            {
                house = Manager.Instance.Current;
            }
            else
            {
                house = Manager.Instance.GetHouseByName(houseName);
            }
            if (house == null)
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }
            return house;
        }

        #region House

        public House GetHouse(string houseName)
        {
            return Mapper.Map<House>(GetHouseByName(houseName));
        }

        public void DeleteHouse(string houseName)
        {
            Logic.House house = GetHouseByName(houseName);

            if (house == Manager.Instance.Current)
                throw new WebFaultException(HttpStatusCode.Conflict);

            Manager.Instance.RemoveHouse(house);
        }

        public House[] GetHouses()
        {
            return Manager.Instance.GetAllHouses().Select(h => Mapper.Map<Logic.House, House>(h)).ToArray();
        }

        public void SetCurrent(string houseName)
        {
            Logic.House house = GetHouseByName(houseName);

            Manager.Instance.SetCurrentHouse(house);
        }

        public House Clone(string houseName, string cloneName)
        {
            if (String.IsNullOrWhiteSpace(cloneName))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.House house = GetHouseByName(houseName);
            if (house == null)
                throw new WebFaultException(HttpStatusCode.NotFound);
            if (Manager.Instance.GetHouseByName(cloneName) != null)
                throw new WebFaultException(HttpStatusCode.Conflict);

            Manager.Instance.Clone(house, cloneName);

            return GetHouse(cloneName);
        }

        #endregion House

        #region Components

        public Component[] GetComponents(string houseName)
        {
            return GetHouseByName(houseName).Components.Select(l => Mapper.Map<Logic.LogicComponent, Component>(l)).ToArray();
        }

        public Component GetSingleComponent(string houseName, string id)
        {
            int idI = 0;
            if (!Int32.TryParse(id, out idI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.LogicComponent component = GetHouseByName(houseName).Components.FirstOrDefault(p => p.Id == idI);
            if (component == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            return Mapper.Map<Component>(component);
        }

        public int CreateComponent(string houseName, Component component)
        {
            Logic.House house = GetHouseByName(houseName);

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
                Logic.ComponentConfig config = new Logic.ComponentConfig(logicComponent, new Dictionary<string, string>(component.Config));
                logicComponent.Config = config;
            }

            //TODO: save position

            house.AddComponent(logicComponent);

            return logicComponent.Id;
        }

        public void SaveComponent(string houseName, string id, Component component)
        {
            Logic.House house = GetHouseByName(houseName);
            if (component == null)
                throw new WebFaultException(HttpStatusCode.BadRequest);

            int idI = 0;
            if (!Int32.TryParse(id, out idI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            if (component.Id != idI)
                throw new WebFaultException(HttpStatusCode.BadRequest); //we don't allow to modify the id

            Logic.LogicComponent logicComponent = house.Components.FirstOrDefault(p => p.Id == idI);
            if (logicComponent == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            if (component.Config != null)
            {
                //change config
                Logic.ComponentConfig config = new Logic.ComponentConfig(logicComponent, new Dictionary<string, string>(component.Config));
                logicComponent.Config = config;
            }
            if (component.Position != null)
            {
                //TODO: save position
            }

            //TODO: do we allow connections modifications here?
        }

        public void DeleteComponent(string houseName, string id)
        {
            int idI = 0;
            if (!Int32.TryParse(id, out idI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.LogicComponent logicComponent = GetHouseByName(houseName).Components.FirstOrDefault(p => p.Id == idI);
            if (logicComponent == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            Manager.Instance.Current.RemoveComponent(logicComponent);
        }

        public Value GetOutputValue(string houseName, string id, string index)
        {
            int idI = 0;
            if (!Int32.TryParse(id, out idI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            int indexI = 0;
            if (!Int32.TryParse(index, out indexI))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.LogicComponent logicComponent = GetHouseByName(houseName).Components.FirstOrDefault(p => p.Id == idI);
            if (logicComponent == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            Logic.LogicOutput output = logicComponent.Outputs.FirstOrDefault(p => p.Index == indexI);
            if (output == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            var value = output.Value;

            return new Value() { Content = new byte[1024] };
        }

        #endregion Components
    }
}