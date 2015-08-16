using HASystem.Desktop.Application.DataModels;
using HASystem.Desktop.Application.Views;
using HASystem.Desktop.Domain.Gates;
using HASystem.Desktop.Domain.Gates.LogicGates;
using HASystem.Desktop.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HASystem.Desktop.Application.PresentationModels
{
    [Export(typeof(MainPresentationModel)), PartCreationPolicy(CreationPolicy.Shared)]
    public class MainPresentationModel : PresentationModel<IMainView>
    {
        #region fields
        private GateBaseDataModel selectedGate;
        #endregion

        #region properties
        public ICommand AddGateCommand { get; private set; }
        public ICommand RemoveGateCommand { get; private set; }
        public GateBaseDataModel SelectedGate {
            get { return selectedGate; }
            set
            {
                selectedGate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<GateBaseDataModel> Gates { get; private set; }
        #endregion

        #region ctor
        [ImportingConstructor]
        public MainPresentationModel(IMainView view)
        {
            View = view;
            Gates = new ObservableCollection<GateBaseDataModel>();

            AddGateCommand = new DelegateCommand(AddGate);
            RemoveGateCommand = new DelegateCommand(RemoveGate);
        }
        #endregion

        #region methods
        private void AddGate(object o)
        {
            Gates.Add(new GateBaseDataModel());
        }

        private void RemoveGate(object o)
        {
            //Gates.Remove(o);
        }
        #endregion
    }
}
