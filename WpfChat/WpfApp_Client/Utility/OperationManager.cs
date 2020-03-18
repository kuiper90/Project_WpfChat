using System;
using System.Windows;

namespace WpfApp_Client.Utility
{
    public enum Operation
    {
        Login,
        ReconnectFromLogin,
        ReconnectFromChat
    }

    public class OperationManager
    {
        private User thisUser;

        private readonly ExecutionManager thisExecutionManager;

        public OperationManager(User CurrentUser, ExecutionManager executionManager)
        {
            thisUser = CurrentUser;
            thisExecutionManager = executionManager;
            thisExecutionManager.PopulateActions(Login, ReconnectFromLogin, ReconnectFromChat);
            thisExecutionManager.PrepareExecution();
        }

        private void Login()
        {
            LoginWindow loginWindow = new LoginWindow();
            Window chatWindow = App.Current.MainWindow;
            chatWindow.Close();
            App.Current.MainWindow = loginWindow;
            loginWindow.Show();
        }

        private void ReconnectFromChat()
        {
            string name = thisUser.CurrentClient.CurrentIndividual.Name;
            thisUser.CurrentClient.DisconnectAction();
            thisUser.CurrentClient.HandleReconnect(name);
        }

        private void ReconnectFromLogin()
        {
            string name = thisUser.CurrentClient.CurrentIndividual.Name;
            thisUser.CurrentClient.DisconnectAction();
            thisUser.CurrentClient.HandleReconnect(name);
        }

        public void Execute(Operation operation)
        {
            try
            {
                if (thisExecutionManager.ActionExecute.ContainsKey(operation))
                    thisExecutionManager.ActionExecute[operation]();
            }
            catch (Exception ex)
            {
                thisUser.CurrentClient.CloseSocket();
                throw new Exception("Cannot connect to server.");
            }
        }
    }
}