﻿using System;
using System.Windows.Input;
using WpfApp_Client.Properties;

namespace WpfApp_Client
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;

        private readonly Action executeMethod;

        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecute = canExecuteMethod;
        }

        public DelegateCommand(Action executeMethod)
            : this(executeMethod, (obj) => true)
        { }

        public DelegateCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", Resources.DelegateCommandDelegatesCannotBeNull);
            execute = executeMethod;
            canExecute = canExecuteMethod;
        }

        public DelegateCommand(Action<object> execute)
            : this(execute, (obj) => true)
        { }

        public void Execute(object parameter) //=> execute(parameter);
        {
            if (this.execute != null)
                this.execute((object)parameter);
        }

        public bool CanExecute(object parameter)
           => canExecute == null ? true : canExecute.Invoke(parameter);

        public void InvokeCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}