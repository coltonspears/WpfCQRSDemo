using System;
using System.Windows.Input;

namespace WpfCQRSDemo.Helpers
{
    public class AsyncRelayCommand : IRaiseCanExecuteChanged
    {
        private readonly Func<System.Threading.Tasks.Task> _execute;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<System.Threading.Tasks.Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (_canExecute == null || _canExecute());
        }

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;
            _isExecuting = true;
            RaiseCanExecuteChanged();
            try
            {
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class AsyncRelayCommand<T> : IRaiseCanExecuteChanged
    {
        private readonly Func<T, System.Threading.Tasks.Task> _execute;
        private readonly Predicate<T> _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<T, System.Threading.Tasks.Task> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (_canExecute == null || _canExecute((T)parameter));
        }

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;
            _isExecuting = true;
            RaiseCanExecuteChanged();
            try
            {
                await _execute((T)parameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
