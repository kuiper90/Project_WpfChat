using System;
using System.Windows;
using WpfApp_Client.Utility;

namespace WpfApp_Client
{
    public class ChatMainViewModel : ViewModelBase
    {
        private User currentUser;

        private OperationManager loginManager;

        public User CurrentUser
        {
            get => currentUser;
            set
            {
                SetProperty(ref currentUser, value);
            }
        }

        private DelegateCommand exitCommand;

        public DelegateCommand ExitCommand
        {
            get => exitCommand ?? new DelegateCommand(Exit);
        }

        private void Exit(object arg)
        {
            currentUser.ConnectionState--;
            OnPropertyChanged("UserState");
            Application.Current.Shutdown();
        }

        private DelegateCommand closingCommand;

        public DelegateCommand ClosingCommand
        {
            get => closingCommand ?? new DelegateCommand(ClosingExecute, ClosingCanExecute);
        }

        private void ClosingExecute(object arg)
        {
            if (currentUser.ConnectionState != ClientState.Disconnected)
            {
                currentUser.CurrentClient.SendMessage("/exit");
            }
        }

        private bool ClosingCanExecute(object arg)
        => (currentUser.ConnectionState == ClientState.Disconnected)
                || (MessageBox.Show("Please, confirm your action.",
                    "Confirm",
                    MessageBoxButton.OKCancel) == MessageBoxResult.OK);

        public InputViewModel InputViewModel
        { get; set; }

        public ChatHistoryViewModel ChatHistoryViewModel
        { get; set; }

        public UsersListViewModel UsersListViewModel
        { get; set; }

        private void HandleMessages(string serverMessage)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                var rez = currentUser.CurrentClient.GetMessageType(serverMessage);
                MessageType messageType = rez.Item1;
                string message = rez.Item2;
                //messageType.TypeAct(message, null, UsersListViewModel.LoggedInUsers);
                //messageType.TypeAct(message, ChatHistoryViewModel.ChatHistory, null);
                messageType.TypeAct(message, ChatHistoryViewModel.ChatHistory, UsersListViewModel.LoggedInUsers);
            });
        }

        private void TryReconnect()
        {
            try
            {
                currentUser.ConnectionState = ClientState.Disconnected;
                UsersListViewModel.LoggedInUsers.Clear();
                if (MessageBox.Show("Connection lost. Reconnect?",
                                    "Confirm",
                                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    loginManager.Execute(Operation.ReconnectFromChat);
                else
                    loginManager.Execute(Operation.Login);
            }
            catch (Exception ex)
            {
                InputViewModel.LoadLoginWindow(ex.Message);
            }
        }

        public void ReLoadLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
        }

        public ChatMainViewModel(User CurrentUser)
        {
            UsersListViewModel = new UsersListViewModel(CurrentUser);
            ChatHistoryViewModel = new ChatHistoryViewModel(CurrentUser);
            InputViewModel = new InputViewModel(CurrentUser);
            currentUser = CurrentUser;
            CurrentUser.CurrentClient.HandleMessages = HandleMessages;
            CurrentUser.CurrentClient.CreateThread();
            CurrentUser.CurrentClient.Reconnect = TryReconnect;
            loginManager = new OperationManager(CurrentUser, new ExecutionManager());            
        }
    }
}