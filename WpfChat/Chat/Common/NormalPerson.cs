using System;
using System.Net.Sockets;

namespace Chat.Common
{
    public class NormalPerson : INormalPerson
    {
        Individual individ;

        public Action<Individual> CloseConnectionAction
        {
            get => individ.CloseConnectionAction;
            set => individ.CloseConnectionAction = value;
        }

        public NormalPerson(Individual normalPerson)
        {
            this.individ = normalPerson;
        }

        public string Name
        {
            get => individ.Name;
            set => individ.Name = value;
        }

        public bool IsConnected
        {
            get => individ.IsConnected;
            set => individ.IsConnected = value;
        }

        public void SendMessage(Message message)
        {
            try
            {
                individ.SendMessage(message);
            }
            catch (SocketException se) when (se.NativeErrorCode.Equals(10054))
            {
                CloseConnectionAction?.Invoke(individ);
                //throw;
            }
        }

        public Message ReceiveMessage()
        {
            try
            {
                return individ.ReceiveMessage();
            }
            catch (SocketException se) when (se.NativeErrorCode.Equals(10054))
            {
                CloseConnectionAction?.Invoke(individ);
                //throw;
                return null;
            }
        }

        public string ConnectionInfo()
            => individ.ConnectionInfo();

        public void CloseConnection()
        {
            individ.CloseConnection();
        }
    }
}