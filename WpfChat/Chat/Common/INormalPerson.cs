namespace Chat.Common
{
    public interface INormalPerson : Individual
    {
        void SendMessage(Message message);

        Message ReceiveMessage();

        void CloseConnection();
    }
}