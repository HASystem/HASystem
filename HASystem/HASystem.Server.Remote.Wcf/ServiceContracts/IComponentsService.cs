using HASystem.Server.Remote.Wcf.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf.ServiceContracts
{
    [ServiceContract]
    public interface IComponentsService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/")]
        Component[] GetComponents();

        [OperationContract]
        [WebGet(UriTemplate = "/{id}")]
        Component GetSingleComponent(string id);

        [OperationContract]
        [WebGet(UriTemplate = "/supported")]
        string[] GetSupportedComponentTypes();
    }
}