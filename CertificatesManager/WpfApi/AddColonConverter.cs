using System;
using System.Globalization;
using System.Windows.Data;

namespace CertificatesManager.WpfApi
{
    class AddColonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string ?? string.Empty) + ":";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
