using HASystem.Shared.ValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.Components
{
    [Component("{25EADD1C-5ED7-46EF-B178-1559747BAF27}")]
    public class BinaryAnd : BooleanLogicGate
    {
        public override void UpdateOutput()
        {
            Output = Input1 && Input2;
        }
    }
}