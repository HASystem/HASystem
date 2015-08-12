using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Shared.Remote.Wcf.DataContracts
{
    [DataContract]
    public class Point : IExtensibleDataObject
    {
        [DataMember]
        public int X
        {
            get;
            set;
        }

        [DataMember]
        public int Y
        {
            get;
            set;
        }

        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }
    }
}