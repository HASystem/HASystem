using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Threading.Tasks;

namespace HASystem.Server.Logic.Components
{
    [Component("{6B24A57E-F347-4733-B788-1A504F99819C}")]
    public class BinaryIn : LogicComponent
    {
        private int current = 0;

        public override void Update()
        {
        }

        public override void Init()
        {
            Outputs = new LogicOutput[] { new LogicOutput(this, 0, typeof(bool)) };
        }

        public BinaryIn()
        {
            //TODO: remove this debug code
            //toggle value
            Timer timer = new Timer(new TimerCallback((o) =>
            {
                if ((GenericValue<bool>)Outputs[0].Value)
                {
                    Outputs[current].Value = new GenericValue<bool>(false);
                }
                else
                {
                    Outputs[current].Value = new GenericValue<bool>(true);
                }
            }), null, 1000, 1000);
        }
    }
}