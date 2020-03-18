using System;
using System.ComponentModel;
using System.Windows;

namespace WpfApp_Client.Utility
{
    class WindowCloseBehavior : DependencyObject
    {
        public static readonly DependencyProperty ClosingProperty
            = DependencyProperty.RegisterAttached(
                "Closing",
                typeof(DelegateCommand),
                typeof(WindowCloseBehavior),
                new UIPropertyMetadata(new PropertyChangedCallback(ClosingChanged)));

        public static readonly DependencyProperty CancelClosingProperty
            = DependencyProperty.RegisterAttached(
                "CancelClosing",
                typeof(DelegateCommand),
                typeof(WindowCloseBehavior));

        public static readonly DependencyProperty ClosedProperty
            = DependencyProperty.RegisterAttached(
                "Closed",
                typeof(DelegateCommand),
                typeof(WindowCloseBehavior),
                new UIPropertyMetadata(new PropertyChangedCallback(ClosedChanged)));

        private static void ClosedChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ChatWindow window = target as ChatWindow;
            if (window != null)
            {
                if (e.NewValue != null)
                    window.Closed += WindowClosed;
                else
                    window.Closed -= WindowClosed;
            }
        }

        private static void ClosingChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Window window = target as Window;
            if (window != null)
            {
                if (e.NewValue != null)
                    window.Closing += WindowClosing;
                else
                    window.Closing -= WindowClosing;
            }
        }

        public static DelegateCommand GetClosed(DependencyObject obj)
        {
            return (DelegateCommand)obj.GetValue(ClosedProperty);
        }

        public static DelegateCommand GetClosing(DependencyObject obj)
            => (DelegateCommand)obj.GetValue(ClosingProperty);

        public static void SetClosing(DependencyObject obj, DelegateCommand value)
        {
            obj.SetValue(ClosingProperty, value);
        }

        public static DelegateCommand GetCancelClosing(DependencyObject obj)
            => (DelegateCommand)obj.GetValue(CancelClosingProperty);

        public static void SetCancelClosing(DependencyObject obj, DelegateCommand value)
        {
            obj.SetValue(CancelClosingProperty, value);
        }

        static void WindowClosing(object sender, CancelEventArgs e)
        {
            DelegateCommand closing = GetClosing(sender as Window);
            if (closing != null)
            {
                if (closing.CanExecute(null))
                    closing.Execute(null);
                else
                {
                    DelegateCommand cancelClosing = GetCancelClosing(sender as Window);
                    if (cancelClosing != null)
                        cancelClosing.Execute(null);
                    e.Cancel = true;
                }
            }
        }

        static void WindowClosed(object sender, EventArgs e)
        {
            DelegateCommand closed = GetClosed(sender as Window);
            if (closed != null)
                closed.Execute(null);
        }
    }
}