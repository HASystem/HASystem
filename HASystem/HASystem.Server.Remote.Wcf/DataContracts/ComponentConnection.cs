using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf.DataContracts
{
    [DataContract]
    public class ComponentConnection : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        [DataMember]
        public int From
        {
            get;
            set;
        }

        [DataMember]
        public int FromPort
        {
            get;
            set;
        }

        [DataMember]
        public int To
        {
            get;
            set;
        }

        [DataMember]
        public int ToPort
        {
            get;
            set;
        }

        public ComponentConnection()
        {
        }

        public ComponentConnection(int from, int fromPort, int to, int toPort)
        {
            From = from;
            FromPort = fromPort;
            To = to;
            ToPort = toPort;
        }
    }
}