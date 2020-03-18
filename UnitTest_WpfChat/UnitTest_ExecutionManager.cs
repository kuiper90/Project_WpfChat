using System.Linq;
using WpfApp_Client.Utility;
using Xunit;

namespace UnitTest_WpfChat
{
    public class UnitTest_ExecutionManager
    {
        [Fact]
        public void PopulateActionShouldAddReconnectAndLoginToActionExecute()
        {
            ExecutionManager execManager = new ExecutionManager();
            execManager.PopulateActions(() => { }, () => { });
            execManager.PrepareExecution();
            Assert.True(execManager.ActionExecute.Keys.ElementAt(0) == Operation.Login);
            Assert.True(execManager.ActionExecute.Keys.ElementAt(1) == Operation.Reconnect);
        }
    }
}
