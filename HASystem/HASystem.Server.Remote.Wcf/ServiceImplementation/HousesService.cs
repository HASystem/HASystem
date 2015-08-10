using AutoMapper;
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
    public class HousesService : IHousesService
    {
        static HousesService()
        {
            House.InitMapping();
        }

        public House GetHouse(string houseName)
        {
            if (String.IsNullOrWhiteSpace(houseName))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            return Mapper.Map<House>(Manager.Instance.GetHouseByName(houseName));
        }

        public void DeleteHouse(string houseName)
        {
            if (String.IsNullOrWhiteSpace(houseName))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.House house = Manager.Instance.GetHouseByName(houseName);
            if (house == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            if (house == Manager.Instance.Current)
                throw new WebFaultException(HttpStatusCode.Conflict);

            Manager.Instance.RemoveHouse(house);
        }

        public House[] GetHouses()
        {
            return Manager.Instance.GetAllHouses().Select(h => Mapper.Map<Logic.House, House>(h)).ToArray();
        }

        public House GetCurrent()
        {
            return Mapper.Map<House>(Manager.Instance.Current);
        }

        public void SetCurrent(string houseName)
        {
            if (String.IsNullOrWhiteSpace(houseName))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.House house = Manager.Instance.GetHouseByName(houseName);
            if (house == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            Manager.Instance.SetCurrentHouse(house);
        }

        public House Clone(string houseName, string cloneName)
        {
            if (String.IsNullOrWhiteSpace(houseName))
                throw new WebFaultException(HttpStatusCode.BadRequest);
            if (String.IsNullOrWhiteSpace(cloneName))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            Logic.House house = Manager.Instance.GetHouseByName(houseName);
            if (house == null)
                throw new WebFaultException(HttpStatusCode.NotFound);
            if (Manager.Instance.GetHouseByName(cloneName) != null)
                throw new WebFaultException(HttpStatusCode.Conflict);

            Manager.Instance.Clone(house, cloneName);

            return GetHouse(cloneName);
        }
    }
}