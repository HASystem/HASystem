using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Domain.Gates
{
    public interface IInput<T> : INotifyPropertyChanged
    {
        T Value { get; set; }
    }
}
