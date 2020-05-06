using System;

namespace Tamagotchi
{
    public class RelayCommand<T> : RelayCommand
    {
        private Action<T> _execute;
        private Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public override void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}