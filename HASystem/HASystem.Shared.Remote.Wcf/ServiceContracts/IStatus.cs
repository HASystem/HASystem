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
    public interface IStatus
    {
        [OperationContract]
        [WebGet(UriTemplate = "")]
        string GetStatus();
    }
}