using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HASystem.Server.Logic.Components
{
    public class BinarySource : LogicComponent
    {
        private int current = 0;

        public override Guid ComponentType
        {
            get { return Guid.NewGuid(); } //TODO: valid implementation
        }

        public override void Update()
        {
            EnsureGates();

            Outputs[current].Value = new GenericValue<bool>(false);
            current++;
            if (current >= Outputs.Length)
            {
                current = 0;
            }
            Outputs[current].Value = new GenericValue<bool>(true);
        }

        public override void Init()
        {
            EnsureGates();
        }

        public BinarySource()
        {
            Timer timer = new Timer(new TimerCallback((o) => { Update(); }), null, 1000, 5000);
        }

        public void EnsureGates()
        {
            int outputCount = Config.GetValue("Outputs", 2);
            if (Outputs.Length != outputCount)
            {
                LogicOutput[] outputs = new LogicOutput[outputCount];
                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i] = new LogicOutput(this, i, typeof(bool));
                }
                Outputs = outputs;
                current = 0;
            }
        }
    }
}