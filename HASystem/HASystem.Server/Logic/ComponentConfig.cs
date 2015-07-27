using System;
using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public class ComponentConfig : IDictionary<string, string>
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();

        public LogicComponent Component
        {
            get;
            private set;
        }

        public ComponentConfig(LogicComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            Component = component;
        }

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

        public int GetInt32(string key, int def)
        {
            string value = null;
            if (TryGetValue(key, out value))
            {
                int valueI = 0;
                if (Int32.TryParse(value, out valueI))
                {
                    return valueI;
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

        public bool GetBoolean(string key, bool def)
        {
            string value = null;
            if (TryGetValue(key, out value))
            {
                bool valueB = def;
                if (Boolean.TryParse(value, out valueB))
                {
                    return valueB;
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

        public void Add(string key, string value)
        {
            values.Add(key, value);
            Component.SetDirty();
        }

        public bool ContainsKey(string key)
        {
            return values.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return values.Keys; }
        }

        public bool Remove(string key)
        {
            bool result = values.Remove(key);
            if (result)
            {
                Component.SetDirty();
            }
            return result;
        }

        public bool TryGetValue(string key, out string value)
        {
            return values.TryGetValue(key, out value);
        }

        public ICollection<string> Values
        {
            get { return values.Values; }
        }

        public string this[string key]
        {
            get
            {
                return values[key];
            }
            set
            {
                if (Object.Equals(values[key], value))
                    return;
                values[key] = value;
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            ((ICollection<KeyValuePair<string, string>>)values).Add(item);
            Component.SetDirty();
        }

        public void Clear()
        {
            values.Clear();
            Component.SetDirty();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)values).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, string>>)values).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return values.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<string, string>>)values).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)values).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}