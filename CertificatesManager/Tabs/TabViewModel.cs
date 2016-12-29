using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CertificatesManager.Tabs
{
    abstract class TabViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(((MemberExpression)property.Body).Member.Name));
        }

        public abstract string Header { get; }
    }
}
