using HASystem.Desktop.Application.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Application.PresentationModels
{
    public abstract class PresentationModel<T> where T : IView
    {
        #region properties
        public T View { get; protected set; }
        #endregion
    }
}
