using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Application.Views
{
    public interface IView
    {
        object DataContext { get; set; }
    }
}
