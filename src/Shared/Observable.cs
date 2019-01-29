using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shared
{
    public abstract class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string property = null)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            RaisePropertyChanged(property);
            return true;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {

        }

        protected void RaisePropertyChanged(string property)
        {
            var eventArgs = new PropertyChangedEventArgs(property);
            OnPropertyChanged(eventArgs);
            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
