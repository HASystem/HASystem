using System;
using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        public Guid Guid
        {
            get;
            private set;
        }

        public ComponentAttribute(Guid guid)
        {
            Guid = guid;
        }

        public ComponentAttribute(string guid)
            : this(Guid.Parse(guid))
        {
        }
    }
}