using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.Components
{
    internal class BinaryAnd : LogicComponent
    {
        public override Guid ComponentType
        {
            get { return Guid.NewGuid(); } //TODO: valid implementation
        }

        public override void Update()
        {
            EnsureGates();

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
            EnsureGates();
        }

        private void EnsureGates()
        {
            int inputCount = Config.GetValue("Inputs", 2);
            if (Inputs.Length != inputCount)
            {
                LogicInput[] inputs = new LogicInput[inputCount];
                for (int i = 0; i < inputs.Length; i++)
                {
                    inputs[i] = new LogicInput(this, i, typeof(bool));
                }
                Inputs = inputs;
            }

            int outputCount = Config.GetValue("Outputs", 1);
            if (Outputs.Length != outputCount)
            {
                LogicOutput[] outputs = new LogicOutput[outputCount];
                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i] = new LogicOutput(this, i, typeof(bool));
                }
                Outputs = outputs;
            }
        }
    }
}