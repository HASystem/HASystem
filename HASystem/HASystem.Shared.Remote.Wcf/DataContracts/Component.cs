using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Shared.Remote.Wcf.DataContracts
{
    [DataContract]
    public class Component : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        [DataMember]
        public int Id
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public string ComponentType
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public Dictionary<string, string> Config
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public Point Position
        {
            get;
            set;
        }

        [DataMember(IsRequired = false)]
        public ComponentConnection[] Connections
        {
            get;
            set;
        }
    }
}