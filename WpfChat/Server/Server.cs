using Chat;
using Chat.Common;
using Chat.Common.Test;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server_Project
{
    public class Server
    {
        private ISocketProxy serverSocket;

        private Room mainRoom;

        public Server(ISocketProxy socket)
        {
            this.serverSocket = socket;
            this.mainRoom = new Room();
        }

        public Room GetRoom() => this.mainRoom;

        public void StartServer(IPAddress ipAddress)
        {
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8300);
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);
                Console.WriteLine("Waiting for clients to connect...");
                while (true)
                {
                    ISocketProxy clientSocket = serverSocket.Accept();
                    Individual normalPerson = new NormalPerson(new Person(clientSocket));
                    Thread myThread = new Thread(HandleLogin);
                    myThread.Start(normalPerson);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void HandleLogin(object normalPerson)
        {
            Individual individ = (Individual)normalPerson;
            try
            {
                mainRoom.Login(individ);
                if (individ.Name != null)
                {
                    Console.WriteLine("Connected to {0}. Person {1} is online.\n",
                        individ.ConnectionInfo(), individ.Name);
                    this.HandleCommunication(individ, () => individ.ReceiveMessage());
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public void HandleCommunication(Individual normalPerson, Func<Message> Communicate)
        {
            while (mainRoom.ProcessMessages(normalPerson, Communicate))
            { }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Server!");

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress currentIpAddress = ipHostInfo.AddressList[1];
            ISocketProxy socket = new SocketProxy(currentIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            LingerOption lingerOption = new LingerOption(true, 0);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, lingerOption);
            Server server = new Server(socket);
            server.StartServer(currentIpAddress);
            Console.Read();
        }
    }
}