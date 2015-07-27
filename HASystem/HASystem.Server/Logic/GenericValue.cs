using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.Logic
{
    public class GenericValue<T> : Value
    {
        public T Value
        {
            get;
            private set;
        }

        public GenericValue(T value)
        {
            this.Value = value;
        }

        public GenericValue()
        {
        }

        public static implicit operator GenericValue<T>(T value)
        {
            return new GenericValue<T>(value);
        }

        public static implicit operator T(GenericValue<T> genericValue)
        {
            if (Object.ReferenceEquals(genericValue, null))
                return default(T);
            return genericValue.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is T)
            {
                return Object.Equals(Value, obj);
            }
            GenericValue<T> otherGV = obj as GenericValue<T>;
            if (otherGV != null)
            {
                return Object.Equals(Value, otherGV.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (Value == null)
                return 0;
            return Value.GetHashCode();
        }

        public static bool operator ==(GenericValue<T> a, GenericValue<T> b)
        {
            if (Object.ReferenceEquals(a, null))
            {
                if (Object.ReferenceEquals(b, null))
                {
                    return true;
                }
                else
                {
                    return Object.ReferenceEquals(b.Value, null);
                }
            }
            if (Object.ReferenceEquals(b, null))
            {
                return Object.ReferenceEquals(a.Value, null);
            }

            return Object.Equals(a.Value, b.Value);
        }

        public static bool operator !=(GenericValue<T> a, GenericValue<T> b)
        {
            return !(a == b);
        }

        public static bool operator ==(T a, GenericValue<T> b)
        {
            if (Object.ReferenceEquals(b, null))
            {
                return Object.ReferenceEquals(a, null);
            }
            if (Object.ReferenceEquals(a, null))
            {
                return Object.ReferenceEquals(b.Value, null);
            }

            return Object.Equals(a, b.Value);
        }

        public static bool operator !=(T a, GenericValue<T> b)
        {
            return !(a == b);
        }

        public static bool operator ==(GenericValue<T> a, T b)
        {
            if (Object.ReferenceEquals(a, null))
            {
                return Object.ReferenceEquals(b, null);
            }
            if (Object.ReferenceEquals(b, null))
            {
                return Object.ReferenceEquals(a.Value, null);
            }

            return Object.Equals(a.Value, b);
        }

        public static bool operator !=(GenericValue<T> a, T b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            if (Value == null)
                return "";
            return Value.ToString();
        }
    }
}