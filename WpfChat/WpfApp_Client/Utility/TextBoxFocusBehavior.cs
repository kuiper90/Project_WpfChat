using System.Windows;
using System.Windows.Controls;

namespace WpfApp_Client.Utility
{
    class TextBoxFocusBehavior : DependencyObject
    {
        public static readonly DependencyProperty WatermarkText =
            DependencyProperty.RegisterAttached("WatermarkText",
            typeof(string),
            typeof(TextBoxFocusBehavior),
            //new UIPropertyMetadata(string.Empty, OnWatermarkTextChanged));
            new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(OnWatermarkTextChanged)));

        public static readonly DependencyProperty WatermarkEnabled =
            DependencyProperty.RegisterAttached("WatermarkEnabled",
                typeof(bool),
                typeof(TextBox),
                new UIPropertyMetadata(false, OnWatermarkEnabled));

        private static void OnWatermarkTextChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var inputTextBox = d as TextBox;
            if (inputTextBox != null)
            {
                inputTextBox.Text = (string)e.NewValue;
            }
        }

        private static void OnWatermarkEnabled(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox inputTextBox = sender as TextBox;
            if (inputTextBox != null)
            {
                bool isEnabled = (bool)e.NewValue;
                if (isEnabled)
                {
                    inputTextBox.GotFocus += InputTextBoxGotFocus;
                    inputTextBox.LostFocus += InputTextBoxLostFocus;
                }
                else
                {
                    inputTextBox.GotFocus -= InputTextBoxGotFocus;
                    inputTextBox.LostFocus -= InputTextBoxLostFocus;
                }
            }
        }

        private static void InputTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var inputTextBox = e.OriginalSource as TextBox;
            if (inputTextBox != null)
            {
                if (string.IsNullOrEmpty(inputTextBox.Text))
                    inputTextBox.Text = GetWatermarkText(inputTextBox);
            }
        }

        private static void InputTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            var inputTextBox = e.OriginalSource as TextBox;
            if (inputTextBox != null)
            {
                if (inputTextBox.Text == GetWatermarkText(inputTextBox))
                    inputTextBox.Text = string.Empty;
            }
        }

        public static string GetWatermarkText(DependencyObject obj)
            => (string)obj.GetValue(WatermarkText);

        public static void SetWatermarkText(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkText, value);
        }

        public static bool GetWatermarkEnabled(DependencyObject obj)
            => (bool)obj.GetValue(WatermarkEnabled);

        public static void SetWatermarkEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(WatermarkEnabled, value);
        }
    }
}