using HASystem.Desktop.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Desktop.Domain.Gates
{
    public abstract class GateBase<TIn,TOut> : NotifyPropertyChangedBase
    {
        #region Fields
        private ObservableCollection<IInput<TIn>> inputs;
        private ObservableCollection<IOutput<TOut>> outputs;
        #endregion

        #region Properties
        public IReadOnlyList<IInput<TIn>> Inputs 
        { 
            get 
            { 
                return inputs; 
            } 
            private set 
            {
                inputs = value == null ? null : new ObservableCollection<IInput<TIn>>(value.ToList()); 
                OnPropertyChanged(); 
            } 
        }

        public IReadOnlyList<IOutput<TOut>> Outputs
        {
            get
            {
                return outputs;
            }
            private set
            {
                outputs = value == null ? null : new ObservableCollection<IOutput<TOut>>(value.ToList()); 
                OnPropertyChanged();
            }
        }
        #endregion

        #region Ctor
        public GateBase()
        {
            inputs = new ObservableCollection<IInput<TIn>>();
            outputs = new ObservableCollection<IOutput<TOut>>();

            inputs.CollectionChanged += InputsCollectionChanged;
            outputs.CollectionChanged += OutputsCollectionChanged;
        }
        #endregion

        #region Handle Events
        private void InputsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Add suppression context
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    HandleInputCollectionEventRegistration(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    HandleInputCollectionEventRegistration(e.NewItems, false);
                    break;
                default:
                    break;
            }

            Process();
        }

        private void HandleInputCollectionEventRegistration(System.Collections.IList list, bool register = true)
        {
            foreach (var item in list)
            {
                IInput<TIn> input = item as IInput<TIn>;
                if (input != null)
                {
                    if (register)
                    {
                        input.PropertyChanged += InputPropertyChanged;
                    }
                    else
                    {
                        input.PropertyChanged -= InputPropertyChanged;
                    }
                }
            }
        }

        private void InputPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Process();
        }

        private void OutputsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Methods
        protected abstract void Process();

        protected virtual void AddInput(IInput<TIn> input = default(IInput<TIn>))
        {
            inputs.Add(input);
        }

        protected virtual void RemoveInput(IInput<TIn> input)
        {
            inputs.Remove(input);
        }

        protected virtual void AddOutput(IOutput<TOut> output = default(IOutput<TOut>))
        {
            outputs.Add(output);
        }

        protected virtual void RemoveOuput(IOutput<TOut> output)
        {
            outputs.Remove(output);
        }

        protected virtual void AddInputPorts(int numberOfInputs)
        {
            AddPorts(numberOfInputs, Direction.Input);
        }

        protected virtual void AddOutputPorts(int numberOfOutputs)
        {
            AddPorts(numberOfOutputs, Direction.Output);
        }

        protected virtual void AddPorts(int numberOfPorts, Direction direction)
        {
            for (int i = 0; i < numberOfPorts; i++)
            {
                if (direction == Direction.Input)
                {
                    AddInput();
                }
                else
                {
                    AddOutput();
                }
            }
        }

        protected virtual void ClearInputs()
        {
            inputs.Clear();
        }

        protected virtual void ClearOutputs()
        {
            outputs.Clear();
        }
        #endregion
    }
}