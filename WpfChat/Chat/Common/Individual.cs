using System;

namespace Chat
{
    public interface Individual
    {
        Action<Individual> CloseConnectionAction { get; set; }

        string Name { get; set; }

        bool IsConnected { get; set;}

        void SendMessage(Message message);

        Message ReceiveMessage();

        string ConnectionInfo();

        void CloseConnection();
    }
}