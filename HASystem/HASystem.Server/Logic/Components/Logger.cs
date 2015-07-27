using HASystem.Shared.ValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.Components
{
    [Component("{1D60EFC4-AE2E-4806-97B5-1F17F69692CD}")]
    public class Logger : LogicComponent
    {
        public override void Init()
        {
            Inputs = new LogicInput[] { new LogicInput(this, 0, typeof(Value)) };
        }

        public override void UpdateOutput()
        {
            System.Diagnostics.Trace.WriteLine(Inputs[0].Value);
        }
    }
}