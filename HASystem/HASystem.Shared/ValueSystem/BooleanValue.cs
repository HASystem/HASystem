using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Shared.ValueSystem
{
    public class BooleanValue : GenericValue<bool>
    {
        public BooleanValue(bool value)
            : base(value)
        {
        }

        public BooleanValue()
            : base()
        {
        }
    }
}