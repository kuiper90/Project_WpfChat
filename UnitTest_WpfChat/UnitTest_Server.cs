using Chat;
using Chat.Common;
using Chat.Common.Test;
using Server_Project;
using Xunit;

namespace UnitTest_WpfChat
{
    public class UnitTest_Server
    {
        [Fact]
        public void If_NotLoggedIn_Person_Sends_Msg_Server_Answers_With_Broadcast()
        {
            MockSocketProxy serverMockSocket = new MockSocketProxy();
            Server testServer = new Server(serverMockSocket);
            Room testRoom = testServer.GetRoom();
            MockSocketProxy mockSocketOne = new MockSocketProxy();
            MockSocketProxy mockSocketTwo = new MockSocketProxy();
            NormalPerson testPersonOne = new NormalPerson(new Person(mockSocketOne));
            NormalPerson testPersonTwo = new NormalPerson(new Person(mockSocketTwo));
            mockSocketOne.internalReceiveBuffer.Add(new Message("testUserNameOne").WrapData());
            mockSocketTwo.internalReceiveBuffer.Add(new Message("testUserNameTwo").WrapData());
            mockSocketOne.internalReceiveBuffer.Add(new Message("test").WrapData());
            mockSocketOne.internalReceiveBuffer.Add(new Message("/exit").WrapData());
            mockSocketTwo.internalReceiveBuffer.Add(new Message("/exit").WrapData());
            testRoom.Login(testPersonOne);
            testRoom.Login(testPersonTwo);
            testServer.HandleCommunication(testPersonOne, () => testPersonOne.ReceiveMessage());
            testServer.HandleCommunication(testPersonTwo, () => testPersonTwo.ReceiveMessage());
            Assert.Equal("M03testUserNameOne : test", mockSocketTwo.sentMessage[4]);
            Assert.Equal("M01testUserNameOne is offline.", mockSocketTwo.sentMessage[5]);
        }

        [Fact]
        public void If_LoggedIn_Person_Sends_Msg_Server_Answers_With_Broadcast()
        {
            MockSocketProxy serverMockSocket = new MockSocketProxy();
            Server testServer = new Server(serverMockSocket);
            Room testRoom = testServer.GetRoom();
            MockSocketProxy mockSocketOne = new MockSocketProxy();
            MockSocketProxy mockSocketTwo = new MockSocketProxy();
            NormalPerson testPersonOne = new NormalPerson(new Person("testUserNameOne", mockSocketOne));
            NormalPerson testPersonTwo = new NormalPerson(new Person("testUserNameTwo", mockSocketTwo));
            mockSocketOne.internalReceiveBuffer.Add(new Message("test").WrapData());
            mockSocketOne.internalReceiveBuffer.Add(new Message("/exit").WrapData());
            mockSocketTwo.internalReceiveBuffer.Add(new Message("/exit").WrapData());
            testRoom.Join(testPersonOne);
            testRoom.Join(testPersonTwo);
            testServer.HandleCommunication(testPersonOne, () => testPersonOne.ReceiveMessage());
            testServer.HandleCommunication(testPersonTwo, () => testPersonTwo.ReceiveMessage());
            Assert.Equal("M03testUserNameOne : test", mockSocketTwo.sentMessage[0]);
            Assert.Equal("M01testUserNameOne is offline.", mockSocketTwo.sentMessage[1]);
        }
    }
}