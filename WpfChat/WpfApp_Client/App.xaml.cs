using System;
using System.Windows;

namespace WpfApp_Client
{
    public partial class App : Application
    {
        void AppStartup(object sender, StartupEventArgs e)
        {
            bool startMinimized = false;
            for (int i = 0; i != e.Args.Length; ++i)
            {
                if (e.Args[i] == "/StartMinimized")
                {
                    startMinimized = true;
                }
            }
            // Create main application window, starting minimized if specified
            LoginWindow loginWindow = new LoginWindow();
            if (startMinimized)
            {
                loginWindow.WindowState = WindowState.Minimized;
            }
            loginWindow.Show();
        }

        public App()
        {
            Console.WriteLine("Client on.");
        }
    }
}