namespace WpfApp_Client
{
    public class UsersListViewModel : ViewModelBase
    {
        private User currentUser;

        private ChatProperty loggedInUsers;

        public User CurrentUser
        { get => currentUser; }

        public ChatProperty LoggedInUsers
        {
            get => loggedInUsers;
            private set
            {
                SetProperty(ref loggedInUsers, value);
            }
        }

        public UsersListViewModel(User currentUser)
        {
            this.loggedInUsers = new ChatProperty();
            this.currentUser = currentUser;
        }
    }
}