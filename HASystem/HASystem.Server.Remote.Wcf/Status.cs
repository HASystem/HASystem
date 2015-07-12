using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf
{
    [ServiceContract]
    public class Status : IStatus
    {
        [OperationContract]
        [WebGet(UriTemplate = "/")]
        public string GetStatus()
        {
            return "running";
        }
    }
}