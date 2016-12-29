using System.Windows;
using System.Windows.Controls;

namespace CertificatesManager.WpfApi
{
    static class PasswordHelper
    {
        #region Password

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static string GetPassword(DependencyObject dependency)
        {
            return (string)dependency.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dependency, string value)
        {
            dependency.SetValue(PasswordProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null) return;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        #endregion

        #region IsAttached

        public static readonly DependencyProperty IsAttachedProperty = DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, OnPasswordPropertyAttached));

        public static bool GetIsAttached(DependencyObject dependency)
        {
            return (bool)dependency.GetValue(IsAttachedProperty);
        }

        public static void SetIsAttached(DependencyObject dependency, bool value)
        {
            dependency.SetValue(IsAttachedProperty, value);
        }

        private static void OnPasswordPropertyAttached(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null) return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        #endregion

        #region IsUpdating

        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));

        private static bool GetIsUpdating(DependencyObject dependency)
        {
            return (bool)dependency.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dependency, bool value)
        {
            dependency.SetValue(IsUpdatingProperty, value);
        }

        #endregion

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null) return;

            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}
