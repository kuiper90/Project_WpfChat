namespace WpfApp_Client
{
    public class ChatHistoryViewModel : ViewModelBase
    {
        private User currentUser;

        private ChatProperty chatHistory;

        public ChatProperty ChatHistory
        {
            get => chatHistory;
            set
            {
                SetProperty(ref chatHistory, value);
            }
        }

        public ChatHistoryViewModel(User currentUser)
        {
            chatHistory = new ChatProperty();
            this.currentUser = currentUser;
        }
    }
}