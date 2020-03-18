using System;

namespace WpfApp_Client
{
    public abstract class MessageType
    {
        public static readonly MessageType ChatUser = new ChatUserType();

        public static readonly MessageType ChatData = new ChatDataType();

        public static readonly MessageType Unknown = new UnknownType();

        public MessageType()
        { }

        public MessageType(int value, string messageType)
        { }
        
        public abstract Action<object, object, object> TypeAct
        { get; }


        public class UnknownType : MessageType
        {
            public UnknownType() : base(2, "UnknownType")
            { }

            public override Action<object, object, object> TypeAct
            {
                get { return (a, b, c) => { }; }
            }
        }
    }
}