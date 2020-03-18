using System;
using System.Collections.Generic;

namespace WpfApp_Client.Utility
{
    public class ExecutionManager
    {
        public Dictionary<Operation, Action> ActionExecute
        { get; set; }

        private Action login;
        private Action reconnectFromChat;
        private Action reconnectFromLogin;

        public ExecutionManager()
        {
            ActionExecute = new Dictionary<Operation, Action>(3);
        }

        public void PopulateActions(Action Login, Action ReconnectFromLogin, Action ReconnectFromChat)
        {
            login = Login;
            reconnectFromChat = ReconnectFromChat;
            reconnectFromLogin = ReconnectFromLogin;
        }

        public void PrepareExecution()
        {
            ActionExecute.Add(Operation.Login, login);
            ActionExecute.Add(Operation.ReconnectFromLogin, reconnectFromLogin);
            ActionExecute.Add(Operation.ReconnectFromChat, reconnectFromChat);
        }
    }
}