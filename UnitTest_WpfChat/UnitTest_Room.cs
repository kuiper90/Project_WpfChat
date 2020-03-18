using Chat;
using Chat.Common;
using Chat.Common.Test;
using Server_Project;
using Xunit;

namespace UnitTest_WpfChat
{
    public class UnitTest_Room
    {
        [Fact]
        public void If_NewClient_Server_AskFor_Username()
        {
            MockSocketProxy serverMockSocket = new MockSocketProxy();
            Server testServer = new Server(serverMockSocket);

            MockSocketProxy mockSocket = new MockSocketProxy();
            mockSocket.internalReceiveBuffer.Add(new Message("testUserName").WrapData());
            testServer.GetRoom().Login(new NormalPerson(new Person(mockSocket)));

            Assert.Equal("S00", mockSocket.sentMessage[0]);
            Assert.Equal("S01", mockSocket.sentMessage[1]);
        }

        [Fact]
        public void If_Duplicate_UserName_Server_AsksFor_AnotherUsername()
        {
            MockSocketProxy serverMockSocket = new MockSocketProxy();
            Server testServer = new Server(serverMockSocket);
            Room testRoom = testServer.GetRoom();

            MockSocketProxy mockSocketOne = new MockSocketProxy();
            NormalPerson testPersonOne = new NormalPerson(new Person(mockSocketOne));
            mockSocketOne.internalReceiveBuffer.Add(new Message("testUserNameOne").WrapData());
            testRoom.Login(testPersonOne);

            MockSocketProxy mockSocketTwo = new MockSocketProxy();
            NormalPerson testPersonTwo = new NormalPerson(new Person(mockSocketTwo));
            mockSocketTwo.internalReceiveBuffer.Add(new Message("testUserNameOne").WrapData());
            mockSocketTwo.internalReceiveBuffer.Add(new Message("testUserNameTwo").WrapData());
            testRoom.Login(testPersonTwo);

            Assert.Equal("S00", mockSocketTwo.sentMessage[0]);
            Assert.Equal("E00", mockSocketTwo.sentMessage[1]);
        }

        [Fact]
        public void If_OnlyConnected_Person_Sends_Exit_Msg_Room_IsEmptied()
        {
            MockSocketProxy serverMockSocket = new MockSocketProxy();
            Server testServer = new Server(serverMockSocket);
            Room testRoom = testServer.GetRoom();
            MockSocketProxy mockSocket = new MockSocketProxy();
            NormalPerson testPerson = new NormalPerson(new Person("testUserName", mockSocket));

            testRoom.Join(testPerson);
            mockSocket.internalReceiveBuffer.Add(new Message("/exit").WrapData());
            testServer.HandleCommunication(testPerson, () => testPerson.ReceiveMessage());

            Assert.True(testRoom.IsRoomEmpty());
        }

        [Fact]
        public void If_NotLoggedIn_Person_Sends_Exit_Msg_Room_IsEmptied()
        {
            MockSocketProxy serverMockSocket = new MockSocketProxy();
            Server testServer = new Server(serverMockSocket);
            Room testRoom = testServer.GetRoom();
            MockSocketProxy mockSocket = new MockSocketProxy();
            NormalPerson testPerson = new NormalPerson(new Person(mockSocket));

            mockSocket.internalReceiveBuffer.Add(new Message("testUserName").WrapData());
            mockSocket.internalReceiveBuffer.Add(new Message("/exit").WrapData());
            testRoom.Login(testPerson);
            testServer.HandleCommunication(testPerson, () => testPerson.ReceiveMessage());

            Assert.True(testRoom.IsRoomEmpty());
        }
    }
}