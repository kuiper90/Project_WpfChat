using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfApp_Client
{
    public class ChatUsers : ObservableCollection<string>
    {
        private User currentUser;

        private List<User> usersList;

        public User CurrentUser
        {
            get => currentUser;
        }

        public List<User> GetUsersList()
        {
            List<User> usersList = null;
            return usersList;
        }
    }
}