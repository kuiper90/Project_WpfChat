using Xunit;
using WpfApp_Client;

namespace UnitTest_WpfChat
{
    public class UnitTest_ViewModelBase
    {
        [Fact]
        public void SetPropertyMethodShouldSetTheNewValue()
        {
            string value = "mockProperty";
            MockViewModel mockViewModel = new MockViewModel();
            Assert.Equal(null, mockViewModel.MockProperty);
            mockViewModel.MockProperty = value;
            Assert.Equal(mockViewModel.MockProperty, value);
        }

        [Fact]
        public void SetPropertyMethodShouldRaisePropertyRaised()
        {
            bool invoked = false;
            MockViewModel mockViewModel = new MockViewModel();
            mockViewModel.PropertyChanged +=
                (sender, propertyChangedEvent) =>
                {
                    if (propertyChangedEvent.PropertyName.Equals("MockProperty"))
                        invoked = true;
                };
            mockViewModel.MockProperty = "testMockProperty";
            Assert.True(invoked);
        }
    }
}
