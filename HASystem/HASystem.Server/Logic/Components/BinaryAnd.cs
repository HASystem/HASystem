using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.Components
{
    [Component("{25EADD1C-5ED7-46EF-B178-1559747BAF27}")]
    internal class BinaryAnd : LogicComponent
    {
        public override void UpdateOutput()
        {
            bool value = true;
            foreach (LogicInput input in Inputs)
            {
                if (((GenericValue<bool>)input.Value) != true)
                {
                    value = false;
                    break;
                }
            }

            foreach (LogicOutput output in Outputs)
            {
                output.Value = new GenericValue<bool>(value);
            }
        }

        public override void Init()
        {
            Outputs = new LogicOutput[] { new LogicOutput(this, 0, typeof(bool)) };

            EnsureInputGates();
        }

        protected override void OnComponenentConfigChanged(ComponentConfig oldValue, ComponentConfig newValue)
        {
            EnsureInputGates();

            base.OnComponenentConfigChanged(oldValue, newValue);
        }

        private void EnsureInputGates()
        {
            //do we allow multiple input ports?
            int inputCount = Config.GetInt32("Inputs", 2);
            if (Inputs.Length != inputCount)
            {
                LogicInput[] inputs = new LogicInput[inputCount];
                for (int i = 0; i < inputs.Length; i++)
                {
                    inputs[i] = new LogicInput(this, i, typeof(bool));
                }
                Inputs = inputs;
            }
        }
    }
}