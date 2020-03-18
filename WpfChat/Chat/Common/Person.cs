using Chat.Common.Test;
using System;
using System.Net;
using System.Net.Sockets;

namespace Chat.Common
{
    public class Person : Individual
    {
        private string name;

        private bool isConnected = true;

        private readonly ISocketProxy socket;

        private readonly Buffer buffer;

        public Action<Individual> CloseConnectionAction
        { get; set; }

        public Person(ISocketProxy sock)
        {
            socket = sock;
            buffer = new Buffer();
        }

        public Person(string nom, ISocketProxy socket) : this(socket)
        {
            this.Name = nom;
        }

        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        public bool IsConnected
        {
            get => this.isConnected;
            set => this.isConnected = value;
        }

        public ISocketProxy GetSocket()
            => this.socket;

        public override string ToString()
            => this.Name;

        public void SendMessage(Message message)
        {
            this.socket.Send(message.WrapData());
        }

        public Message ReceiveMessage()
        {
            return new Message(buffer.Read(GetSocket()));
        }

        public void CloseConnection()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public string ConnectionInfo()
        {
            return "remote IP address: " + ((IPEndPoint)(socket.RemoteEndPoint())).Address
                    + " and port: " + ((IPEndPoint)(socket.RemoteEndPoint())).Port;
        }
    }
}