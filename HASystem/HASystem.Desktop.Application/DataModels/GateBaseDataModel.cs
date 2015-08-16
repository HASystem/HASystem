using HASystem.Desktop.Domain;
using HASystem.Desktop.Utilities;

namespace HASystem.Desktop.Application.DataModels
{
    public class GateBaseDataModel : NotifyPropertyChangedBase
    {
        #region fields
        private IGraphicalControl graphicalControl;
        private IDomainObject domainObject;
        #endregion

        #region properties
        public IGraphicalControl GraphicalControl
        {
            get { return graphicalControl; }
            set
            {
                graphicalControl = value;
                OnPropertyChanged();
            }
        }

        public IDomainObject DomainObject
        {
            get { return domainObject; }
            set
            {
                domainObject = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
