using System;
using System.Windows;

namespace WpfApp_Client
{
    public class ChatUserType : MessageType
    {
        public ChatUserType() : base(0, "ChatUser")
        { }

        public static readonly MessageType On = new ChatUserOnType();
        public static readonly MessageType Enter = new ChatUserEnterType();
        public static readonly MessageType Off = new ChatUserOffType();
        public static readonly MessageType Exit = new ChatUserExitType();

        public override Action<object, object, object> TypeAct
        {
            get { return (x, y, z) => { /* nothing here*/ }; }
        }
        
        public class ChatUserOnType : MessageType
        {
            public ChatUserOnType() : base(0, "userOn")
            { }

            public override Action<object, object, object> TypeAct
            {
                get
                {
                    return (message, chatHistory, loggedInUsers) =>
                    {
                        string msg = (string)message;
                        ((ChatProperty)chatHistory)?.Add(msg);
                        ((ChatProperty)loggedInUsers)?.Add(msg.Substring(0, (msg.Length - 11)));
                    };
                }
            }
        }

        public class ChatUserEnterType : MessageType
        {
            public ChatUserEnterType() : base(0, "userEnter")
            { }

            public override Action<object, object, object> TypeAct
            {
                get
                {
                    return (message, chatHistory, loggedInUsers) =>
                    {
                        string msg = (string)message;
                        ((ChatProperty)chatHistory)?.Add(msg);
                    };
                }
            }
        }

        public class ChatUserOffType : MessageType
        {
            public ChatUserOffType() : base(1, "userOff")
            { }

            public override Action<object, object, object> TypeAct
            {
                get
                {
                    return (message, chatHistory, loggedInUsers) =>
                    {
                        string msg = (string)message;
                        ((ChatProperty)chatHistory)?.Add(msg);
                        ((ChatProperty)loggedInUsers).Remove(msg.Substring(0, (msg.Length - 12)));
                    };
                }
            }
        }

        public class ChatUserExitType : MessageType
        {
            public ChatUserExitType() : base(2, "userExit")
            { }

            public override Action<object, object, object> TypeAct
            {
                get
                {
                    return (a, b, c) =>
                    {
                        Window chatWindow = App.Current.MainWindow;
                        chatWindow?.Close();
                    };
                }
            }
        }
    }
}