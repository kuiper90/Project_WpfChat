using System.Collections.ObjectModel;

namespace WpfApp_Client
{
    public class ChatProperty : ObservableCollection<string>
    {
        public ChatProperty()//: base()
        { }

        public ChatProperty(string loginMessage)
        { }
    }
}