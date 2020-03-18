using System.Windows;
using System.Windows.Controls;

namespace WpfApp_Client.Utility
{
    //https://marcominerva.wordpress.com/2014/09/30/scrolltobottom-behavior-for-listview-in-mvvm-based-universal-windows-apps/
    public class AutoScrollBehavior : DependencyObject
    {
        public static readonly DependencyProperty AutoScrollProperty = 
            DependencyProperty.RegisterAttached("AutoScroll",
            typeof(bool),
            typeof(AutoScrollBehavior), 
            new UIPropertyMetadata(false, AutoScrollPropertyChanged));

        public static void AutoScrollPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var scrollViewer = obj as ScrollViewer;

            if (scrollViewer != null && (bool)args.NewValue)
            {
                scrollViewer.ScrollChanged += OnAutoScrollChanged;
                scrollViewer.ScrollToEnd();
            }
            else
                scrollViewer.ScrollChanged -= OnAutoScrollChanged;
        }

        private static void OnAutoScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0)
            {
                var scrollViewer = sender as ScrollViewer;
                scrollViewer?.ScrollToBottom();
            }
        }

        public static bool GetAutoScroll(DependencyObject obj)
            => (bool)obj.GetValue(AutoScrollProperty);

        public static void SetAutoScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollProperty, value);
        }
    }
}