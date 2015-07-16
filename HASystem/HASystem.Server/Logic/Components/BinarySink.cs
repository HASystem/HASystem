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

            for (int i = 0; i < Inputs.Length; i++)
            {
                LogicInput input = Inputs[i];
                if ((GenericValue<bool>)input.Value)
                {
                    System.Diagnostics.Debug.WriteLine(i + " " + true);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(i + " " + false);
                }
            }
        }

        public override void Init()
        {
            EnsureGates();
        }

        public void EnsureGates()
        {
            int inputCount = Config.GetValue("Inputs", 1);
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