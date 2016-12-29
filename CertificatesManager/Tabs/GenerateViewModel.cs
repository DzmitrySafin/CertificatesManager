using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using CertificatesManager.Properties;
using CertificatesManager.WpfApi;
using Microsoft.Win32;

namespace CertificatesManager.Tabs
{
    class GenerateViewModel : TabViewModel
    {
        private X509Certificate2 _certificate;

        #region TabViewModel

        public override string Header => Resources.tabGenerate;

        #endregion

        #region Bindings

        public string Subject { get; set; }

        private DateTime _validFrom;
        public DateTime ValidFrom
        {
            get { return _validFrom; }
            set
            {
                if (_validFrom != value)
                {
                    _validFrom = value;
                    RaisePropertyChanged(() => MinimumValidTo);
                }
            }
        }

        private DateTime _validTo;
        public DateTime ValidTo
        {
            get { return _validTo; }
            set
            {
                if (_validTo != value)
                {
                    _validTo = value;
                    RaisePropertyChanged(() => MaximumValidFrom);
                }
            }
        }

        public DateTime MinimumValidFrom => DateTime.Now.Date;

        public DateTime MaximumValidFrom => ValidTo.AddDays(-1);

        public DateTime MinimumValidTo => ValidFrom.AddDays(1);

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
                }
            }
        }

        public string Password { get; set; }

        #endregion

        public GenerateViewModel()
        {
            _validFrom = DateTime.Now.Date;
            _validTo = _validFrom.AddYears(1);

            StoreNames = new List<StoreName>(Enum.GetValues(typeof(StoreName)).Cast<StoreName>().ToList());
            StoreLocations = new List<StoreLocation>(Enum.GetValues(typeof(StoreLocation)).Cast<StoreLocation>().ToList());

            _selectedStore = StoreName.My;
            _selectedLocation = StoreLocation.CurrentUser;
        }

        #region Commands

        private ICommand _generateCommand;
        public ICommand GenerateCommand => _generateCommand ?? (_generateCommand = new RelayCommand(GenerateCertificate, () => !string.IsNullOrWhiteSpace(Subject)));

        private ICommand _installCommand;
        public ICommand InstallCommand => _installCommand ?? (_installCommand = new RelayCommand(InstallCertificate, () => _certificate != null));

        private ICommand _exportCommand;
        public ICommand ExportCommand => _exportCommand ?? (_exportCommand = new RelayCommand(ExportCertificate, () => _certificate != null));

        #endregion

        private void GenerateCertificate()
        {
            Cursor cursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;

            _certificate = CertificateHelper.GenerateCertificate(Subject, ValidFrom, ValidTo);
            Thumbprint = _certificate.Thumbprint;

            Mouse.OverrideCursor = cursor;
        }

        private void InstallCertificate()
        {
            var store = new X509Store(SelectedStore, SelectedLocation);
            store.Open(OpenFlags.ReadWrite);
            store.Add(_certificate);
            store.Close();
        }

        private void ExportCertificate()
        {
            var sfd = new SaveFileDialog
            {
                CreatePrompt = false,
                OverwritePrompt = true,
                AddExtension = true,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = _certificate.FriendlyName,
                Filter = "Personal Information Exchange (*.pfx)|*.pfx|DER Encoded Binary X.509 (*.cer)|*.cer|All files (*.*)|*.*",
                FilterIndex = 1,
                Title = Resources.titleSelectExportCertificate,
                ValidateNames = true
            };

            if (sfd.ShowDialog() == true)
            {
                File.WriteAllBytes(sfd.FileName, _certificate.Export(sfd.FilterIndex == 1 ? X509ContentType.Pfx : X509ContentType.Cert, Password));
            }
        }
    }
}
