using System;
using WpfApp_Client;
using Xunit;

namespace UnitTest_WpfChat
{
    public class UnitTest_DelegateCommand
    {
        public void DoNothing(object param)
        { }

        [Fact]
        public void NonGenericDelegateCommandExecuteShouldInvokeExecuteAction()
        {
            bool executed = false;
            var command = new DelegateCommand((a) => { executed = true; });
            command.Execute(new SomeObject());
            Assert.True(executed);
        }

        [Fact]
        public void NonGenericDelegateCommandCanExecuteShouldInvokeCanExecuteFunc()
        {
            bool invoked = false;
            var command = new DelegateCommand((a) => { }, (a) => { invoked = true; return true; });

            bool canExecute = command.CanExecute(new SomeObject());
            Assert.True(invoked);
            Assert.True(canExecute);
        }

        [Fact]
        public void NonGenericDelegateCommandCanExecuteShouldInvokeCanExecuteFuncFalse()
        {
            bool invoked = false;
            var command = new DelegateCommand((a) => { }, (a) => { invoked = true; return false; });

            bool canExecute = command.CanExecute(new SomeObject());
            Assert.True(invoked);
            Assert.False(canExecute);
        }

        [Fact]
        public void NonGenericDelegateCommandShouldDefaultCanExecuteToTrue()
        {
            DelegateCommand command = new DelegateCommand((a) => { });
            Assert.True(command.CanExecute(new SomeObject()));
        }

        [Fact]
        public void NonGenericDelegateThrowsIfDelegatesAreNull()
        {
            DelegateCommand command; // = new DelegateCommand(null, null);
            Action act = () => command = new DelegateCommand(null, null);
            var exception = Record.Exception(act);
            Assert.IsType(typeof(System.ArgumentNullException), exception);
            Assert.True(exception.Message.Contains("DelegateCommand delegates cannot be null."));
        }

        [Fact]
        public void NonGenericDelegateCommandThrowsIfExecuteDelegateIsNull()
        {
            var exception = Record.Exception(() => {
            DelegateCommand command = new DelegateCommand(null);
            });
            Assert.IsType(typeof(System.ArgumentNullException), exception);
            Assert.True(exception.Message.Contains("DelegateCommand delegates cannot be null."));
        }
    }

    internal class SomeObject
    { }
}