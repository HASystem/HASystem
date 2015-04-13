using HASystem.Desktop.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Domain.Gates
{
    public class Output<T> : NotifyPropertyChangedBase, IOutput<T>
    {
        #region Fields
        private T value;
        #endregion

        #region Properties
        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
