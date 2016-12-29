using System;
using System.Windows.Input;

namespace CertificatesManager.WpfApi
{
    class RelayCommand : ICommand
    {
        #region Private

        private readonly Func<bool> _canExecute;

        private readonly Action _command;

        #endregion

        #region ICommand

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _command.Invoke();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        public RelayCommand(Action command, Func<bool> canExecute = null)
        {
            _command = command;
            _canExecute = canExecute;
        }
    }

    class RelayCommand<T> : ICommand
    {
        #region Private

        private readonly Predicate<T> _canExecute;

        private readonly Action<T> _command;

        #endregion

        #region ICommand

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke((T)parameter);
        }

        public void Execute(object parameter)
        {
            _command.Invoke((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        public RelayCommand(Action<T> command, Predicate<T> canExecute = null)
        {
            _command = command;
            _canExecute = canExecute;
        }
    }
}
