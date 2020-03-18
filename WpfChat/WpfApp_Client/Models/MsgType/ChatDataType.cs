using System;

namespace WpfApp_Client
{
    public class ChatDataType : MessageType
    {
        public ChatDataType() : base(0, "ChatData")
        { }

        public static readonly MessageType MsgHistory = new ChatDataMsgHistoryType();
        public static readonly MessageType UsersList = new ChatDataUsersListType();

        public override Action<object, object, object> TypeAct
        {
            get => (x, y, z) => { /* nothing here*/ };
        }

        public class ChatDataMsgHistoryType : MessageType
        {
            public ChatDataMsgHistoryType() : base(0, "msgHistory")
            { }

            public override Action<object, object, object> TypeAct
            {
                get
                {
                    return (message, chatHistory, loggedInUsers) =>
                    {
                        string msg = (string)message;
                        ((ChatProperty)chatHistory).Add(msg);
                    };
                }
            }
        }

        public class ChatDataUsersListType : MessageType
        {
            public ChatDataUsersListType() : base(0, "msgUsersList")
            { }

            public override Action<object, object, object> TypeAct
            {
                get => (message, chatHistory, loggedInUsers) =>
                    {
                        string users = (string)message;
                        foreach (string user in users.Split(','))
                        {
                            ((ChatProperty)loggedInUsers).Add(user);
                        }
                    };
            }
        }
    }
}