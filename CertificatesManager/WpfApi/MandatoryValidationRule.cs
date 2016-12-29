using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using CertificatesManager.Properties;

namespace CertificatesManager.WpfApi
{
    class MandatoryValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            BindingExpression be = value as BindingExpression;
            var isEmpty = be != null ? string.IsNullOrWhiteSpace(be.DataItem.GetType().GetProperty(be.ParentBinding.Path.Path).GetValue(be.DataItem) as string) : string.IsNullOrWhiteSpace(value as string);

            return isEmpty ? new ValidationResult(false, Resources.ruleMandatory) : ValidationResult.ValidResult;
        }
    }
}
