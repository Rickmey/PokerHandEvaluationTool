using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PokerEvaluationToolUI
{
    public interface IViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// If the model is frozen values won't be updated.
        /// </summary>
        bool Frozen { get; set; }

        /// <summary>
        /// Sets value and updates bindings
        /// </summary>
        /// <typeparam name="T">Type of property</typeparam>
        /// <param name="property">Reference to the property</param>
        /// <param name="value">New value of the property</param>
        /// <param name="propertyName">Optional binding name of the propety. If not specified the propety name is used</param>
        void SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null);
    }

    public abstract class ViewModelBase : IViewModelBase
    {
        bool _frozen = false;
        public bool Frozen
        {
            get => _frozen;
            set
            {
                if (_frozen == value)
                    return;
                if (value)
                    OnPropertyChanged("");
                _frozen = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (Frozen)
                return;
            if (property != null)
            {
                if (property.Equals(value))
                    return;
            }
            property = value;
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
