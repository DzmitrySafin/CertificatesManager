using System;
using System.Globalization;
using System.Windows.Data;

namespace CertificatesManager.WpfApi
{
    class PortNumberToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strVal = value?.ToString().TrimStart('-') ?? string.Empty;

            for (int i = 0; i < strVal.Length; i++)
            {
                if (!char.IsDigit(strVal, i))
                {
                    strVal = strVal.Substring(0, i);
                    break;
                }
            }

            uint intVal;
            uint.TryParse(strVal, out intVal);
            while (intVal > ushort.MaxValue) intVal /= 10;
            return intVal;
        }
    }
}
