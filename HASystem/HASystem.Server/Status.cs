using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HASystem.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Status" in both code and config file together.
    public class Status : IStatus
    {
        public string GetStatus()
        {
            return "running";
        }
    }
}
