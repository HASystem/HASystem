using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using System.Text;

using System.Threading.Tasks;

namespace HASystem.Shared.Remote.Wcf.DataContracts
{
    [DataContract]
    public class Device : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public string State
        {
            get;
            set;
        }

        [DataMember]
        public string MACAddress
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public string IPAddress
        {
            get;
            set;
        }
    }
}