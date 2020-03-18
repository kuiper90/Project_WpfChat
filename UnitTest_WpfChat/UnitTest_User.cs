using Chat;
using Chat.Common;
using Chat.Common.Test;
using WpfApp_Client;
using Xunit;

namespace UnitTest_WpfChat
{
    public class UnitTest_User
    {
        [Fact]
        public void UpdateConnectionStateShouldChangeConnectionStateWhenServerGreetsClient()
        {
            MockSocketProxy mockSocket = new MockSocketProxy();
            Client testClient = new Client(mockSocket);
            testClient.CurrentIndividual = new NormalPerson(new Person(mockSocket));
            User testUser = new User();
            testUser.CurrentClient = testClient;
            mockSocket.internalReceiveBuffer.Add(new Message("S00").WrapData());

            testUser.UpdateConnectionState();

            Assert.Equal(ClientState.Connected, testUser.ConnectionState);
        }
    }
}
