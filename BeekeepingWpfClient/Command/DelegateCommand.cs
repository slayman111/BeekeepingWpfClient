using System;
using System.Windows.Input;

namespace BeekeepingWpfClient.Command;

public class DelegateCommand : ICommand
{
    private readonly Func<object, bool>? _canExecute;
    private readonly Action<object>? _execute;

    public DelegateCommand(Action<object>? execute, Func<object, bool>? canExecute = null)
    {
        (_execute, _canExecute) = (execute, canExecute);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute is null || _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        _execute?.Invoke(parameter);
    }
}
