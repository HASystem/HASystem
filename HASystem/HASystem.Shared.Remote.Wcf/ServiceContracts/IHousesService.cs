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
        #region House

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
        [WebInvoke(UriTemplate = "current", Method = "POST")]
        void SetCurrent(string houseName);

        [OperationContract]
        [WebInvoke(UriTemplate = "{houseName}/clone", Method = "POST")]
        House Clone(string houseName, string cloneName);

        #endregion House

        #region Components

        [OperationContract]
        [WebGet(UriTemplate = "{houseName}/components")]
        Component[] GetComponents(string houseName);

        [OperationContract]
        [WebGet(UriTemplate = "{houseName}/components/{id}")]
        Component GetSingleComponent(string houseName, string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "{houseName}/components", Method = "POST")]
        int CreateComponent(string houseName, Component component);

        [OperationContract]
        [WebInvoke(UriTemplate = "{houseName}/components/{id}", Method = "PUT")]
        void SaveComponent(string houseName, string id, Component component);

        [OperationContract]
        [WebInvoke(UriTemplate = "{houseName}/components/{id}", Method = "DELETE")]
        void DeleteComponent(string houseName, string id);

        [OperationContract]
        [WebGet(UriTemplate = "{houseName}/components/{id}/outputs/{index}/value")]
        Value GetOutputValue(string houseName, string id, string index);

        #endregion Components
    }
}