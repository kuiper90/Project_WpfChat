using WpfApp_Client;
using Xunit;
using static WpfApp_Client.ChatDataType;

namespace UnitTest_WpfChat
{
    public class UnitTest_ChatDataType
    {
        [Fact]
        public void ChatDataMsgHistoryShouldAddMessageToChatHistory()
        {
            ChatDataMsgHistoryType chatMsgHistory = new ChatDataMsgHistoryType();
            string testMessage = "chatMsgHistory";
            ChatProperty testChatProperty = new ChatProperty();
            Assert.True(testChatProperty.Count == 0);
            chatMsgHistory.TypeAct(testMessage, testChatProperty, null);
            Assert.True(testChatProperty.Count == 1);
            Assert.Equal("chatMsgHistory", testChatProperty[0]);
        }

        [Fact]
        public void ChatDataUsersListShouldAddUsersToLoggedInUsersList()
        {
            ChatDataUsersListType chatTestUsersList = new ChatDataUsersListType();
            string testMessage = "chatUser1,chatUser2,chatUser3";
            ChatProperty testChatUsersProperty = new ChatProperty();
            Assert.True(testChatUsersProperty.Count == 0);
            chatTestUsersList.TypeAct(testMessage, null, testChatUsersProperty);
            Assert.True(testChatUsersProperty.Count == 3);
            Assert.Equal("chatUser1", testChatUsersProperty[0]);
            Assert.Equal("chatUser2", testChatUsersProperty[1]);
            Assert.Equal("chatUser3", testChatUsersProperty[2]);
        }
    }
}
