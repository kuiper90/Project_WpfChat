using System;
using System.Windows;
using WpfApp_Client.Utility;

namespace WpfApp_Client
{
    public class LoginViewModel : ViewModelBase
    {
        private string name;

        private string loginMessage;

        private User currentUser;

        private OperationManager loginManager;

        private string loginPlaceholderText = "Please, insert username...";

        public string LoginPlaceholderText
        {
            get => loginPlaceholderText;
            set
            {
                SetProperty(ref loginPlaceholderText, value);
            }
        }

        private DelegateCommand loginCommand;

        public string LoginMessage
        {
            get => loginMessage;
            set
            {
                SetProperty(ref loginMessage, value);
            }
        }

        public string UserName
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                loginCommand.InvokeCanExecuteChanged();
            }
        }

        public ClientState UserState
        {
            get => currentUser.ConnectionState;
            set
            {
                if (currentUser.CurrentClient.CheckServersGreeting())
                    currentUser.ConnectionState++;
            }
        }

        public DelegateCommand LoginCommand
        {
            get => loginCommand;
            set
            {
                loginCommand = value;
            }
        }

        public LoginViewModel()
        {
            currentUser = new User();
            currentUser.GetToStartClient();
            currentUser.CurrentClient.Reconnect = TryReconnect;
            loginCommand = new DelegateCommand(LoginExecute, LoginCanExecute);
            loginManager = new OperationManager(currentUser, new ExecutionManager());
        }

        public LoginViewModel(User user)
        {
            currentUser = user;
            loginCommand = new DelegateCommand(LoginExecute, LoginCanExecute);
            loginManager = new OperationManager(currentUser, new ExecutionManager());
            UpdateConnectionState();
        }

        public LoginViewModel(string errorMessage) : base()
        {
            LoginMessage = errorMessage;
        }

        private void UpdateConnectionState()
        {
            if (currentUser.CurrentClient.CheckServersGreeting())
                currentUser.ConnectionState++;
        }

        private bool LoginCanExecute(object arg)
            => !string.IsNullOrWhiteSpace(UserName)
                && UserName != LoginPlaceholderText
                && currentUser.ConnectionState == ClientState.Connected;

        public void LoginExecute(object obj)
        {
            if (!currentUser.CurrentClient.HandleLogin(UserName))
                loginMessage = "This username is already assigned to another user.";
            else
            {
                currentUser.ConnectionState = ClientState.Connected;
                OnPropertyChanged("UserState");
                loginMessage = "You are online.";
                LoadChatWindow();
            }
        }

        private void TryReconnect()
        {
            try
            {
                currentUser.ConnectionState = ClientState.Disconnected;
                loginMessage = "Connection lost. To reconnect, click Login.";
                loginManager.Execute(Operation.ReconnectFromLogin);
            }
            catch (Exception ex)
            {
                loginMessage = "Cannot connect to server.";
            }
        }

        private void LoadChatWindow()
        {
            ChatWindow chatWindow = new ChatWindow(currentUser);
            Window loginWindow = App.Current.MainWindow;
            App.Current.MainWindow = chatWindow;
            loginWindow.Close();
            chatWindow.Show();
        }
    }
}