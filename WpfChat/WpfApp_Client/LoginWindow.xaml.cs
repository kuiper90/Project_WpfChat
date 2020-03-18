using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WpfApp_Client
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            base.DataContext = new LoginViewModel();
        }

        public LoginWindow(string errorMessage)
        {
            InitializeComponent();
            base.DataContext = new LoginViewModel(errorMessage);
        }

        public LoginWindow(User user)
        {
            InitializeComponent();
            base.DataContext = new LoginViewModel(user);
        }

        private void LoginUI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            loginPanel.Focus();
        }

        void UpdateBinding(object sender, RoutedEventArgs e)
        {
            BindingOperations.GetBindingExpression((TextBox)sender, TextBox.TextProperty)?.UpdateSource();
        }

        private void OnStartup(object sender, RoutedEventArgs e)
        {
            loginTextBox.Focus();
            loginPanel.Focus();
            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.TextChangedEvent,
                new RoutedEventHandler(UpdateBinding));
        }
    }
}