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
        #region properties
        public ICommand AddGateCommand { get; private set; }
        public ObservableCollection<GateBase<bool, bool>> Gates { get; private set; }
        #endregion

        #region ctor
        [ImportingConstructor]
        public MainPresentationModel(IMainView view)
        {
            View = view;
            Gates = new ObservableCollection<GateBase<bool, bool>>();

            AddGateCommand = new DelegateCommand(AddGate);
        }
        #endregion

        #region methods
        private void AddGate(object o)
        {
            Gates.Add(new AndGate());
        }
        #endregion
    }
}
