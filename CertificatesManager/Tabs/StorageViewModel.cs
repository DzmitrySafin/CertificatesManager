using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using CertificatesManager.Properties;

namespace CertificatesManager.Tabs
{
    class StorageViewModel : TabViewModel
    {
        #region TabViewModel

        public override string Header => Resources.tabStorage;

        #endregion

        #region Bindings

        public ObservableCollection<X509Certificate2> Certificates { get; set; } = new ObservableCollection<X509Certificate2>();

        private X509Certificate2 _selectedCertificate;
        public X509Certificate2 SelectedCertificate
        {
            get { return _selectedCertificate; }
            set
            {
                _selectedCertificate = value;
                OnPropertyChanged();
                Thumbprint = _selectedCertificate?.Thumbprint;
            }
        }

        private string _thumbprint;
        public string Thumbprint
        {
            get { return _thumbprint; }
            set
            {
                if (_thumbprint != value)
                {
                    _thumbprint = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<StoreName> StoreNames { get; private set; }

        private StoreName _selectedStore;
        public StoreName SelectedStore
        {
            get { return _selectedStore; }
            set
            {
                if (_selectedStore != value)
                {
                    _selectedStore = value;
                    OnPropertyChanged();
                    RefreshCertificatesList();
                }
            }
        }

        public List<StoreLocation> StoreLocations { get; private set; }

        private StoreLocation _selectedLocation;
        public StoreLocation SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                if (_selectedLocation != value)
                {
                    _selectedLocation = value;
                    OnPropertyChanged();
                    RefreshCertificatesList();
                }
            }
        }

        #endregion

        public StorageViewModel()
        {
            StoreNames = new List<StoreName>(Enum.GetValues(typeof(StoreName)).Cast<StoreName>().ToList());
            StoreLocations = new List<StoreLocation>(Enum.GetValues(typeof(StoreLocation)).Cast<StoreLocation>().ToList());

            _selectedStore = StoreName.My;
            _selectedLocation = StoreLocation.CurrentUser;

            RefreshCertificatesList();
        }

        private void RefreshCertificatesList()
        {
            Certificates.Clear();

            try
            {
                var store = new X509Store(SelectedStore, SelectedLocation);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                foreach (X509Certificate2 cert in store.Certificates.Cast<X509Certificate2>()) Certificates.Add(cert);
                store.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
