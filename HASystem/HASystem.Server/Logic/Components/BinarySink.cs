using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.Components
{
    internal class BinarySink : LogicComponent
    {
        public override Guid ComponentType
        {
            get { return Guid.NewGuid(); } //TODO: valid implementation
        }

        public override void Update()
        {
            EnsureGates();

            foreach (LogicInput input in Inputs)
            {
                if ((GenericValue<bool>)input.Value)
                {
                    System.Diagnostics.Debug.WriteLine(true);
                }
            }
        }

        public override void Init()
        {
            EnsureGates();
        }

        public void EnsureGates()
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
        }
    }
}