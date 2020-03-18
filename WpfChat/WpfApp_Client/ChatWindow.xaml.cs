using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WpfApp_Client
{
    public partial class ChatWindow : Window
    {
        public ChatWindow(User currentUser)
        {
            this.InitializeComponent();
            base.DataContext = new ChatMainViewModel(currentUser);
        }

        private void ChatUI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            chatGrid.Focus();
        }

        void UpdateBinding(object sender, RoutedEventArgs e)
        {
           BindingOperations.GetBindingExpression((TextBox)sender, TextBox.TextProperty)?.UpdateSource();
        }

        private void OnStartup(object sender, RoutedEventArgs e)
        {
            inputTextBox.Focus();
            chatGrid.Focus();
            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.TextChangedEvent,
                new RoutedEventHandler(UpdateBinding));
        }
    }
}