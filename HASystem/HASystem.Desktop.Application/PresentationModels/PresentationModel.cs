using HASystem.Desktop.Application.Views;
using HASystem.Desktop.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Application.PresentationModels
{
    public abstract class PresentationModel<T> : NotifyPropertyChangedBase where T : IView
    {
        #region fields
        protected T view;
        #endregion

        #region properties
        public T View
        {
            get { return view; }
            protected set
            {
                view = value;
                if (view != null) { view.DataContext = this; }

                OnPropertyChanged();
            }
        }
        #endregion
    }
}
