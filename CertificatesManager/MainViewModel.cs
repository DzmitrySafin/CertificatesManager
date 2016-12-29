using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CertificatesManager.Tabs;

namespace CertificatesManager
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Bindings

        public List<TabViewModel> Tabs { get; set; }

        #endregion

        public MainViewModel()
        {
            Tabs = new List<TabViewModel> { new GenerateViewModel(), new StorageViewModel(), new SslcertViewModel() };
        }
    }
}
