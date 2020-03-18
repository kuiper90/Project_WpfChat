using Chat.Common.Test;
using System;
using WpfApp_Client;
using WpfApp_Client.Utility;
using Xunit;

namespace UnitTest_WpfChat
{
    public class UnitTest_OperationManager
    {
        [Fact]
        public void OperationManagerShouldPopulateActions()
        {
            User testUser = new User();
            ExecutionManager execManager = new ExecutionManager();
            OperationManager opManager = new OperationManager(testUser, execManager);
            Assert.Equal(2, execManager.ActionExecute.Count);
        }

        [Fact]
        public void ExecuteShouldRunLoginOperation()
        {
            bool invoked = false;
            User testUser = new User();
            ExecutionManager execManager = new ExecutionManager();
            OperationManager opManager = new OperationManager(testUser, execManager);

            execManager.ActionExecute[Operation.Login] = () =>
            {
                invoked = true;
            };
            opManager.Execute(Operation.Login);
            Assert.True(invoked);
        }

        [Fact]
        public void ExecuteShouldRunReconnectOperation()
        {
            bool invoked = false;
            User testUser = new User();
            ExecutionManager execManager = new ExecutionManager();
            OperationManager opManager = new OperationManager(testUser, execManager);

            execManager.ActionExecute[Operation.Reconnect] = () =>
            {
                invoked = true;
            };
            opManager.Execute(Operation.Reconnect);
            Assert.True(invoked);
        }

        [Fact]
        public void ExecuteShouldThrowExceptionForNullOperation()
        {
            User testUser = new User();
            MockSocketProxy mockSocket = new MockSocketProxy();
            testUser.CurrentClient = new Client(mockSocket);
            ExecutionManager execManager = new ExecutionManager();
            OperationManager opManager = new OperationManager(testUser, execManager);
            execManager.ActionExecute[Operation.Login] = null;
            Action act = () => opManager.Execute(Operation.Login);
            var exception = Record.Exception(act);
            Assert.IsType(typeof(System.Exception), exception);
            Assert.True(exception.Message.Contains("Cannot connect to server."));
        }
    }
}
