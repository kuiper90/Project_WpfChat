using Chat.Common.Test;

namespace WpfApp_Client
{
    public class User : ViewModelBase
    {
        private Client currentClient;

        private ClientState clientState;

        public Client CurrentClient
        {
            get => currentClient;
            set => currentClient = value;
        }

        public ClientState ConnectionState
        {
            get => clientState;
            set => SetProperty(ref clientState, value);
        }

        public User()
        { }

        public void UpdateConnectionState()
        {
            if (this.CurrentClient.CheckServersGreeting())
                this.ConnectionState++;
        }

        public string GetUserName()
            => currentClient.CurrentIndividual.Name;

        public void GetToStartClient()
        {
            CurrentClient = new Client();
            CurrentClient.StartClient();
            UpdateConnectionState();
        }
    }
}