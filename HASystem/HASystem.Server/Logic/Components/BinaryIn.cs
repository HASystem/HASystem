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
        private Timer timer;

        public override void UpdateOutput()
        {
            bool value = Config.GetBoolean("Value", false);
            Outputs[0].Value = new GenericValue<bool>(value);
        }

        public override void Init()
        {
            Outputs = new LogicOutput[] { new LogicOutput(this, 0, typeof(bool)) };
        }

        public BinaryIn()
        {
            //TODO: remove this debug code
            //toggle value
            timer = new Timer(new TimerCallback((o) =>
            {
                if (Config.GetBoolean("Value", false))
                {
                    Config["Value"] = false.ToString();
                }
                else
                {
                    Config["Value"] = true.ToString();
                }
                SetDirty();
            }), null, 1000, 1000);
        }
    }
}