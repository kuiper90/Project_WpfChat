using WpfApp_Client;
using Xunit;
using static WpfApp_Client.ChatUserType;

namespace UnitTest_WpfChat
{
    public class UnitTest_ChatUserType
    {
        [Fact]
        public void SetUserOnTypeShouldAddMessageToChatHistory()
        {
            ChatUserOnType chatUserOn = new ChatUserOnType();
            string testMessage = "chatUserOnType";
            ChatProperty testChatProperty = new ChatProperty();
            Assert.True(testChatProperty.Count == 0);
            ChatProperty testChatusersList = new ChatProperty();
            chatUserOn.TypeAct(testMessage, testChatProperty, testChatusersList);
            Assert.True(testChatProperty.Count == 1);
            Assert.Equal("chatUserOnType", testChatProperty[0]);
        }

        [Fact]
        public void SetUserOffTypeShouldAddMessageToChatHistory()
        {
            ChatUserOnType chatUserOn = new ChatUserOnType();
            string testMessage = "chatUserOffType";
            ChatProperty testChatHistory = new ChatProperty();
            ChatProperty testChatusersList = new ChatProperty();
            Assert.True(testChatHistory.Count == 0);
            chatUserOn.TypeAct(testMessage, testChatHistory, testChatusersList);
            Assert.True(testChatusersList.Count == 1);
            Assert.Equal("chatUserOffType", testChatHistory[0]);
        }

        [Fact]
        public void SetUserExitTypeShouldAddMessageToChatHistory()
        {
            ChatUserOnType chatUserOn = new ChatUserOnType();
            string testMessage = "chatUserExitType";
            ChatProperty testChatHistory = new ChatProperty();
            ChatProperty testChatusersList = new ChatProperty();
            Assert.True(testChatHistory.Count == 0);
            chatUserOn.TypeAct(testMessage, testChatHistory, testChatusersList);
            Assert.True(testChatusersList.Count == 1);
            Assert.Equal("chatUserExitType", testChatHistory[0]);
        }
    }
}
