using Chat;
using Chat.Common;
using Chat.Common.Test;
using System.Text;
using Xunit;

namespace UnitTest_WpfChat
{
    public class UnitTest_Buffer
    {
        [Fact]
        public void ReadShouldUnwrappReceivedMessage()
        {
            MockSocketProxy mockSocket = new MockSocketProxy();
            NormalPerson normalPerson = new NormalPerson(new Person(mockSocket));
            Buffer buffer = new Buffer();
            mockSocket.internalReceiveBuffer.Add(new Message("test").WrapData());
            byte[] res = buffer.Read(mockSocket);
            string result = Encoding.ASCII.GetString(res);
            Assert.Equal("test", result);
        }

        [Fact]
        public void ReadShouldUnwrappReceivedEmptyMessage()
        {
            MockSocketProxy mockSocket = new MockSocketProxy();
            NormalPerson normalPerson = new NormalPerson(new Person(mockSocket));
            Buffer buffer = new Buffer();
            mockSocket.internalReceiveBuffer.Add(new Message("").WrapData());

            byte[] res = buffer.Read(mockSocket);
            string result = Encoding.ASCII.GetString(res);
            Assert.Equal("", result);
        }

        [Fact]
        public void ReadShouldThrowExceptionIfZeroBytesMessageChunk()
        {
            MockSocketProxy mockSocket = new MockSocketProxy();
            NormalPerson normalPerson = new NormalPerson(new Person(mockSocket));
            Buffer buffer = new Buffer();
            mockSocket.internalReceiveBuffer.Add(mockSocket.WrapData(new Message("test").ToByteArray(), 10));
            var exception = Record.Exception(() => buffer.Read(mockSocket));
            Assert.IsType(typeof(System.Exception), exception);
            Assert.True(exception.Message.Contains("You've received zero bytes. Please, check your connection."));
        }
    }
}