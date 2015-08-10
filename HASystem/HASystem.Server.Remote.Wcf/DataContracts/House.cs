using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Remote.Wcf.DataContracts
{
    [DataContract]
    public class House : IExtensibleDataObject
    {
        private static House()
        {
            Mapper.CreateMap<Logic.House, House>()
                .ForMember(p => p.Name, m => m.MapFrom(l => l.Name))
            ;
        }

        internal static void InitMapping()
        {
            //fake because we use the static ctor for init. but we need a call to the class because otherwise the map wouldn't created
        }

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
    }
}