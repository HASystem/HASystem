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
    public interface IDevicesService
    {
        [OperationContract]
        [WebGet(UriTemplate = "")]
        Device[] GetAllDevices();

        [OperationContract]
        [WebGet(UriTemplate = "{mac}")]
        Device GetDevice(string mac);

        [OperationContract]
        [WebInvoke(UriTemplate = "", Method = "POST")]
        void CreateDevice(Device device);

        [OperationContract]
        [WebInvoke(UriTemplate = "{mac}", Method = "PUT")]
        void SaveDevice(string mac, Device device);

        [OperationContract]
        [WebInvoke(UriTemplate = "{mac}", Method = "DELETE")]
        void DeleteDevice(string mac);
    }
}