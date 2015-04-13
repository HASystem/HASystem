using HASystem.Desktop.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Domain.Gates
{
    public class Connection<TIn, TOut> : NotifyPropertyChangedBase
    {
        #region Fields
        private IInput<TIn> input;
        private IOutput<TOut> output;
        #endregion

        #region Properties
        public IInput<TIn> Input 
        { 
            get 
            { 
                return input; 
            }
            set
            {
                input = value;
                OnPropertyChanged();
            }
        }

        public IOutput<TOut> Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
