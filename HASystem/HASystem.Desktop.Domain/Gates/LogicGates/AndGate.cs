using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Domain.Gates.LogicGates
{
    public class AndGate : GateBase<bool, bool>
    {
        #region Ctor
        public AndGate(int numberOfInputs = 2, int numberOfOutputs = 1)
        {
            SetNumberOfInputs(numberOfInputs);
            SetNumberOfOutputs(numberOfOutputs);
        }
        #endregion

        #region Methods
        private void SetNumberOfInputs(int numberOfInputs)
        {
            // TODO: Exception when not 2 inputs
            if (numberOfInputs < 2) { return; }

            ClearInputs();
            AddInputPorts(numberOfInputs);
        }

        private void SetNumberOfOutputs(int numberOfOutputs)
        {
            if (numberOfOutputs < 1) { return; }

            ClearOutputs();
            AddOutputPorts(numberOfOutputs);
        }

        protected override void Process()
        {
            foreach (var output in Outputs)
	        {
                output.Value = Inputs.All(input => input.Value);
	        }
        }

        protected override void ClearInputs()
        {
            // TODO: Check correct remove
            while (Inputs.Count > 2)
            {
                RemoveInput(Inputs.FirstOrDefault());
            }
        }
        #endregion
    }
}
