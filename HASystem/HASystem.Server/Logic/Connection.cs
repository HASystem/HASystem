using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public class Connection
    {
        public LogicOutput From
        {
            get;
            private set;
        }

        public LogicInput To
        {
            get;
            private set;
        }
    }
}