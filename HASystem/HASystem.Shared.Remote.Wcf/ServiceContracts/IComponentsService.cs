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
    public interface IComponentsService
    {
        [OperationContract]
        [WebGet(UriTemplate = "")]
        Component[] GetComponents();

        [OperationContract]
        [WebGet(UriTemplate = "{id}")]
        Component GetSingleComponent(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "", Method = "POST")]
        int CreateComponent(Component component);

        [OperationContract]
        [WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        void SaveComponent(string id, Component component);

        [OperationContract]
        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        void DeleteComponent(string id);

        [OperationContract]
        [WebGet(UriTemplate = "supported")]
        string[] GetSupportedComponentTypes();

        [OperationContract]
        [WebGet(UriTemplate = "{id}/outputs/{index}/value")]
        Value GetOutputValue(string id, string index);
    }
}