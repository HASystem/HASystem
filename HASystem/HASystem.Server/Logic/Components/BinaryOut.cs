using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.Components
{
    [Component("{52F84FB3-83D8-404D-B5E8-881A5C709A9F}")]
    internal class BinaryOut : LogicComponent
    {
        public override void UpdateOutput()
        {
        }

        public override void Init()
        {
            Inputs = new LogicInput[] { new LogicInput(this, 0, typeof(bool)) };
        }
    }
}