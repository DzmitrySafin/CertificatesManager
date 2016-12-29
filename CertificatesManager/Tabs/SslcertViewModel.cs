using System.Diagnostics;
using System.Windows.Input;
using CertificatesManager.Properties;
using CertificatesManager.WpfApi;

namespace CertificatesManager.Tabs
{
    class SslcertViewModel : TabViewModel
    {
        #region TabViewModel

        public override string Header => Resources.tabSslcert;

        #endregion

        #region Bindings

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

        private string _applicationGuid;
        public string ApplicationGuid
        {
            get { return _applicationGuid; }
            set
            {
                if (_applicationGuid != value)
                {
                    _applicationGuid = value;
                    OnPropertyChanged();
                }
            }
        }

        private ushort _portNumber;
        public ushort PortNumber
        {
            get { return _portNumber; }
            set
            {
                if (_portNumber != value)
                {
                    _portNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _detailsText;
        public string DetailsText
        {
            get { return _detailsText; }
            set
            {
                if (_detailsText != value)
                {
                    _detailsText = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        //public SslcertViewModel()
        //{
        //    // no need for now
        //}

        #region Commands

        private ICommand _addCommand;
        public ICommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(AddSsl, () => PortNumber > 0 && !string.IsNullOrWhiteSpace(ApplicationGuid) && !string.IsNullOrWhiteSpace(Thumbprint)));

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(DeleteSsl, () => PortNumber > 0));

        private ICommand _detailsCommand;
        public ICommand DetailsCommand => _detailsCommand ?? (_detailsCommand = new RelayCommand(DetailsSsl, () => PortNumber > 0));

        #endregion

        private void AddSsl()
        {
            ExecuteNetshCommand($"http delete sslcert ipport=0.0.0.0:{PortNumber}");
            DetailsText = ExecuteNetshCommand($"http add sslcert ipport=0.0.0.0:{PortNumber} certhash={Thumbprint} appid={{{ApplicationGuid}}}");
        }

        private void DeleteSsl()
        {
            DetailsText = ExecuteNetshCommand($"http delete sslcert ipport=0.0.0.0:{PortNumber}");
        }

        private void DetailsSsl()
        {
            DetailsText = ExecuteNetshCommand($"http show sslcert ipport=0.0.0.0:{PortNumber}");
        }

        private static string ExecuteNetshCommand(string arguments)
        {
            var processStartInfo = new ProcessStartInfo("netsh.exe")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                Arguments = arguments,
                RedirectStandardOutput = true
            };

            string output = string.Empty;
            Process process = Process.Start(processStartInfo);
            if (process != null)
            {
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }

            return output;
        }
    }
}
