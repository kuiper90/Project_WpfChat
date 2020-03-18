using Chat.Common;
using System;

namespace Chat
{
    public class Room
    {
        ThreadSafeRoom threadSafeRoomList;

        private Object thisLock = new Object();

        public Room()
        {
            this.threadSafeRoomList = new ThreadSafeRoom();
        }

        public void Join(Individual person)
        {
            this.threadSafeRoomList.Join(person);
        }

        public void Leave(string personName)
        {
            this.threadSafeRoomList.Leave(personName);
        }

        public void Broadcast(Message message, Individual pers)
        {
            foreach (Individual person in threadSafeRoomList.GetListExcept(pers))
            {
                person.SendMessage(message);
            }
        }

        public void BroadcastSenderAndMessage(Message message, Individual normalPerson)
        {
            Message senderMessage = new Message("M03" + normalPerson.Name + " : " + message.ToString());
            Broadcast(senderMessage, null);
        }

        public void Login(Individual pers)
        {
            string personName = null;
            pers.CloseConnectionAction = CloseConnectionAction;
            pers.SendMessage(new Message("S00"));
            personName = pers.ReceiveMessage().ToString();
            while (threadSafeRoomList.IsOnline(personName))
            {
                pers.SendMessage(new Message("E00"));
                personName = pers.ReceiveMessage().ToString();
            }
            pers.Name = personName;
            this.Join(pers);
            pers.SendMessage(new Message("S01"));
            SendUsersList(pers);
            this.Broadcast(new Message("M00" + pers.Name + " is online."), null);
        }

        public void SendUsersList(Individual pers)
        {
            pers.SendMessage(new Message("M02" + threadSafeRoomList.ConvertToString()));
        }

        public bool IsRoomEmpty() => threadSafeRoomList.Count() == 0;

        private void CloseConnectionAction(Individual individ)
        {
            if (individ.Name == null)
            {
                Console.WriteLine("Client with {0} disconnected without logging in.\n", individ.ConnectionInfo());
                individ.CloseConnection();
            }
            else
            {
                string connectionInfo = individ.ConnectionInfo();
                Exit(individ);
                Console.WriteLine("Person {0} with {1} disconnected.\n", individ.Name, connectionInfo);
            }
        }

        private void Exit(Individual normalPerson)
        {
            normalPerson.SendMessage(new Message("M01" + normalPerson.Name + " is offline."));
            Leave(normalPerson.Name);
            normalPerson.CloseConnection();
            Broadcast(new Message("M01" + normalPerson.Name + " is offline."), null);
        }

        public bool ProcessMessages(Individual normalPerson, Func<Message> Communicate)
        {
            Message message = Communicate();
            if (message == null || message.CheckIfExitMsg())
            {
                ProcessDisconnect(normalPerson);
                return false;
            }
            else
            {
                BroadcastSenderAndMessage(message, normalPerson);
                Console.WriteLine("Received from person {0} the following message: {1}\n", normalPerson.Name, message);
            }
            return true;
        }

        public void ProcessDisconnect(Individual normalPerson)
        {
            Console.WriteLine("Client with {0} disconnected. Person {1} is offline.\n",
                normalPerson.ConnectionInfo(), normalPerson.Name);
            Exit(normalPerson);
        }
    }
}