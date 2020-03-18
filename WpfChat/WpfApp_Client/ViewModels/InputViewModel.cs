using System.Net.Sockets;
using System.Windows;

namespace WpfApp_Client
{
    public class InputViewModel : ViewModelBase
    {
        private User currentUser;

        private DelegateCommand sendCommand;

        private DelegateCommand enterCommand;

        private string inputMessage;

        private string inputPlaceholderText = "Type a message";

        public string InputPlaceholderText
        {
            get => inputPlaceholderText;
            set
            {
                SetProperty(ref inputPlaceholderText, value);
            }
        }

        public DelegateCommand SendCommand
        {
            get => sendCommand;
            set
            {
                sendCommand = value;
            }
        }

        public DelegateCommand EnterCommand
        {
            get => enterCommand;
            set
            {
                enterCommand = value;
            }
        }

        public string Input
        {
            get => inputMessage;
            set
            {
                SetProperty(ref inputMessage, value);
                sendCommand.InvokeCanExecuteChanged();
            }
        }

        public InputViewModel(User currentUser)
        {
            inputMessage = "";
            sendCommand = new DelegateCommand(SendExecute, SendCanExecute);
            enterCommand = new DelegateCommand(EnterExecute, EnterCanExecute);
            this.currentUser = currentUser;
        }

        private bool EnterCanExecute(object arg)
            => SendCanExecute(arg);

        public void EnterExecute(object obj)
        {
            try
            {
                SwitchHandler();
                Input = "";
            }
            catch (SocketException se)
            {
                LoadLoginWindow("Cannot connect to server.");
            }
        }

        private void SwitchHandler()
        {
            if (inputMessage != "/exit")
            {
                currentUser.CurrentClient.SendMessage(inputMessage);
            }
            else if (MessageBox.Show("Please, confirm your action.",
                "Confirm",
                MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                currentUser.ConnectionState = ClientState.LoggedOut;
                currentUser.CurrentClient.SendMessage(inputMessage);
            }
        }

        private bool SendCanExecute(object arg)
            => !string.IsNullOrWhiteSpace(Input)
                && Input != InputPlaceholderText
                && currentUser.ConnectionState == ClientState.LoggedIn;

        public void SendExecute(object obj)
        {
            try
            {
                SwitchHandler();
                Input = InputPlaceholderText;
            }
            catch (SocketException se)
            {
                LoadLoginWindow("Cannot connect to server.");
            }
        }

        public void LoadLoginWindow(string exceptionMessage)
        {
            LoginWindow loginWindow = new LoginWindow(exceptionMessage);
            CloseChatWindow();
            App.Current.MainWindow = loginWindow;
            loginWindow.Show();
        }

        public void CloseChatWindow()
        {
            Window chatWindow = App.Current.MainWindow;
            chatWindow.Close();
        }
    }
}