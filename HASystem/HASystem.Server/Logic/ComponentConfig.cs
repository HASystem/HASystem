using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public class ComponentConfig : Dictionary<string, string>
    {
        public string GetValue(string key, string def)
        {
            string value = null;
            if (TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return def;
            }
        }

        public int GetValue(string key, int def)
        {
            string value = null;
            if (TryGetValue(key, out value))
            {
                int valueI = 0;
                if (Int32.TryParse(base[key], out valueI))
                {
                    return valueI;
                }
                else
                {
                    return valueI;
                }
            }
            else
            {
                return def;
            }
        }
    }
}