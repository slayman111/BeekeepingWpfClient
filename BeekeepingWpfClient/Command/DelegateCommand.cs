using System;
using System.Windows.Input;

namespace BeekeepingWpfClient.Command;

public class DelegateCommand(Action<object>? execute, Func<object, bool>? canExecute = null) : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => canExecute is null || canExecute(parameter);

    public void Execute(object? parameter) => execute?.Invoke(parameter);
}
