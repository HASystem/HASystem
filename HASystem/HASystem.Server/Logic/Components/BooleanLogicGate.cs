using HASystem.Shared.ValueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.Components
{
    public abstract class BooleanLogicGate : LogicComponent
    {
        public bool Input1
        {
            get
            {
                return (BooleanValue)Inputs[0].Value;
            }
        }

        public bool Input2
        {
            get
            {
                return (BooleanValue)Inputs[0].Value;
            }
        }

        public bool Output
        {
            get
            {
                return (BooleanValue)Outputs[0].Value;
            }
            set
            {
                Outputs[0].Value = new BooleanValue(value);
            }
        }

        public override void Init()
        {
            Outputs = new LogicOutput[] { new LogicOutput(this, 0, typeof(BooleanValue)) };
            Inputs = new LogicInput[] {
                new LogicInput(this, 0, typeof(BooleanValue)),
                new LogicInput(this, 1, typeof(BooleanValue))
            };
        }
    }
}