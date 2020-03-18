using Chat;
using Chat.Common;
using Chat.Common.Test;
using WpfApp_Client;
using Xunit;

namespace UnitTest_WpfChat
{
    public class UnitTest_Client
    {
        [Fact]
        public void ServerGreetingShouldReceiveStartMsgFromServer()
        {
            MockSocketProxy mockSocket = new MockSocketProxy();
            Client testClient = new Client(mockSocket);
            testClient.CurrentIndividual = new NormalPerson(new Person(mockSocket));
            mockSocket.internalReceiveBuffer.Add(new Message("S00").WrapData());
            Assert.True(testClient.CheckServersGreeting());
        }

        [Fact]
        public void ValidPersonShouldSetValidUsernameOnConnectedPerson()
        {
            MockSocketProxy mockSocket = new MockSocketProxy();
            Client testClient = new Client(mockSocket);
            testClient.CurrentIndividual = new NormalPerson(new Person(mockSocket));
            mockSocket.internalReceiveBuffer.Add(new Message("S01").WrapData());
            testClient.LoginValidation("testUserName");
            Assert.Equal("testUserName", testClient.CurrentIndividual.Name);
        }

        [Fact]
        public void DuplicateUserNameShouldAskForAnotherUserName()
        {
            MockSocketProxy mockSocket = new MockSocketProxy();
            Client testClient = new Client(mockSocket);
            testClient.CurrentIndividual = new NormalPerson(new Person(mockSocket));
            mockSocket.internalReceiveBuffer.Add(new Message("E00").WrapData());
            mockSocket.internalReceiveBuffer.Add(new Message("S01").WrapData());
            testClient.LoginValidation("testUserNameOne");
            testClient.LoginValidation("testUserNameTwo");
            Assert.Equal("testUserNameOne", mockSocket.sentMessage[0]);
            Assert.Equal("testUserNameTwo", mockSocket.sentMessage[1]);
        }

        [Fact]
        public void If_Duplicate_UserName_NewPersonName_Is_Set_To_New_UserName()
        {
            MockSocketProxy mockSocket = new MockSocketProxy();
            Client testClient = new Client(mockSocket);

            testClient.CurrentIndividual = new NormalPerson(new Person(mockSocket));
            mockSocket.internalReceiveBuffer.Add(new Message("E00").WrapData());
            mockSocket.internalReceiveBuffer.Add(new Message("S01").WrapData());
            testClient.LoginValidation("testUserNameOne");
            testClient.LoginValidation("testUserNameTwo");
            Assert.Equal("testUserNameTwo", testClient.CurrentIndividual.Name);
        }

        //[Fact]
        //public void ClientSendsMessageWhenInputAvailable()
        //{
        //    MockSocketProxy mockSocket = new MockSocketProxy();
        //    Client testClient = new Client(mockSocket);

        //    testClient.NormalPerson = new NormalPerson(new Person(mockSocket));
        //    mockSocket.internalReceiveBuffer.Add(new Message("Hello").WrapData());
        //    Stack<string> stack = new Stack<string>(new string[] { "/exit", "test" });
        //    testClient.HandleCommunication();
        //    Assert.Equal("test", mockSocket.sentMessage[0]);
        //}

        //    [Fact]
        //    public void If_User_Types_Input_Client_Sends_Message_()
        //    {
        //        MockSocketProxy mockSocket = new MockSocketProxy();
        //        Client testClient = new Client(mockSocket);
        //        testClient.NormalPerson = new NormalPerson(new Person(mockSocket));
        //        mockSocket.internalReceiveBuffer.Add(new Message(" ").WrapData());
        //        Stack<string> stack = new Stack<string>(new string[] { "/exit", "test" });
        //        testClient.HandleCommunication();
        //        Assert.Equal("test", mockSocket.sentMessage[0]);
        //    }
    }
}