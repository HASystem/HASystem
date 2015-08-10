using HASystem.Shared.Remote.Wcf.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Shared.Remote.Wcf.ServiceContracts
{
    [ServiceContract]
    public interface IHousesService
    {
        [OperationContract]
        [WebGet(UriTemplate = "{houseName}")]
        House GetHouse(string houseName);

        [OperationContract]
        [WebInvoke(UriTemplate = "{houseName}", Method = "DELETE")]
        void DeleteHouse(string houseName);

        [OperationContract]
        [WebInvoke(UriTemplate = "", Method = "GET")]
        House[] GetHouses();

        [OperationContract]
        [WebGet(UriTemplate = "current")]
        House GetCurrent();

        [OperationContract]
        [WebInvoke(UriTemplate = "current", Method = "POST")]
        void SetCurrent(string houseName);

        [OperationContract]
        [WebInvoke(UriTemplate = "{houseName}/clone", Method = "POST")]
        House Clone(string houseName, string cloneName);
    }
}