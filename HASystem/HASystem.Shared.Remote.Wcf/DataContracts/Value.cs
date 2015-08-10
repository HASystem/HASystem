using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Shared.Remote.Wcf.DataContracts
{
    [DataContract]
    public class Value : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        //TODO: also a type-hint?

        [DataMember]
        public byte[] Content
        {
            get;
            set;
        }
    }
}