using System;
using System.Text;

namespace Chat
{
    public class Message
    {
        private string message;

        public Message()
        { }

        public Message(string message) : this()
        {
            this.message = message;
        }

        public Message(byte[] messageBytes)
            : this(Encoding.UTF8.GetString(messageBytes))
        { }

        public Message(byte[] messageBytes, int count)
          : this(Encoding.UTF8.GetString(messageBytes, 0, count))
        { }

        public Message(byte[] messageBytes, int start, int count)
            : this(Encoding.UTF8.GetString(messageBytes, start, count))
        { }

        public byte[] ToByteArray()
            => Encoding.UTF8.GetBytes(message);

        public override string ToString()
            => this.message;

        public bool CompareTo(Message otherMessage)
            => string.Compare(this.message.ToString(), otherMessage.ToString()) == 0;

        public bool CompareTo(string otherMessage)
            => string.Compare(this.message.ToString().ToLower(), otherMessage.ToLower()) == 0;

        public bool CheckIfEmpty()
            => String.IsNullOrWhiteSpace(this.ToString());

        public bool CheckIfExitMsg()
        {
            if (this.CompareTo("/exit"))
                return true;
            return false;
        }

        public byte[] WrapData()
        {
            byte[] input = this.ToByteArray();
            byte[] prefixLength = BitConverter.GetBytes(input.Length);
            byte[] totalSizeOutput = new byte[prefixLength.Length + input.Length];
            prefixLength.CopyTo(totalSizeOutput, 0);
            input.CopyTo(totalSizeOutput, prefixLength.Length);
            return totalSizeOutput;
        }
    }
}