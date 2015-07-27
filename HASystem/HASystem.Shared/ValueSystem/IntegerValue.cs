using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Shared.ValueSystem
{
    public class IntegerValue : GenericValue<Int32>
    {
        public IntegerValue(int value)
            : base(value)
        {
        }

        public IntegerValue()
            : base()
        {
        }
    }
}