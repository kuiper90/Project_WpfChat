using WpfApp_Client;

namespace UnitTest_WpfChat
{
    public class MockViewModel : ViewModelBase
    {
        private string mockProperty;

        public string MockProperty
        {
            get => this.mockProperty;
            set
            {
                this.SetProperty(ref mockProperty, value);
            }
        }
    }
}