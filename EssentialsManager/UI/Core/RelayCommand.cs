﻿using System.Windows.Input;

namespace UI.Core;

public class RelayCommand : ICommand
{
    private Predicate<object> _canExecute;
    private Action<object> _execute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        _canExecute = canExecute;
        _execute = execute;
    }
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    
    public bool CanExecute(object parameter)
    {
        return _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}