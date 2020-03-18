using Chat;
using System.Text;
using Xunit;

namespace UnitTest_WpfChat
{
    public class UnitTest_Message
    {
        [Fact]
        public void ToString_AppliedOn_String_Should_Return_String()
        {
            Message testMessage = new Message("userName");
            Assert.Equal("userName", testMessage.ToString());
        }

        [Fact]
        public void ToByteArray_AppliedOn_String_Should_Return_String()
        {
            Message testMessage = new Message("userName");
            Assert.Equal(testMessage.ToByteArray(), Encoding.UTF8.GetBytes("userName"));
        }

        [Fact]
        public void ToString_AppliedOn_MsgCtorWithTwoParams_ByteArray_ShouldReturn_String()
        {
            Message testMessage = new Message(Encoding.UTF8.GetBytes("userName"), 8);
            Assert.Equal("userName", testMessage.ToString());
        }

        [Fact]
        public void ToString_AppliedOn_MsgCtorWithTwoParams_ByteArray_ShouldReturn_String_OfGivenLength()
        {
            Message testMessage = new Message(Encoding.UTF8.GetBytes("userName/n"), 8);
            Assert.Equal("userName", testMessage.ToString());
        }

        [Fact]
        public void ToString_AppliedOn_MsgCtorWithThreeParams_ByteArray_ShouldReturn_String()
        {
            Message testMessage = new Message(Encoding.UTF8.GetBytes("userName"), 0, 8);
            Assert.Equal("userName", testMessage.ToString());
        }

        [Fact]
        public void ToString_AppliedOn_MsgCtorWithThreeParams_ByteArray_ShouldReturn_String_OfGivenLength()
        {
            Message testMessage = new Message(Encoding.UTF8.GetBytes("userName/n"), 0, 8);
            Assert.Equal("userName", testMessage.ToString());
        }

        [Fact]
        public void ExitMsgIsCreatedCorrectly()
        {
            Message testMessage = new Message(Encoding.UTF8.GetBytes("/exit/n"), 0, 5);
            Assert.True(testMessage.CompareTo("/exit"));
        }
    }
}