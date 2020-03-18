using Chat;
using Chat.Common;
using Chat.Common.Test;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WpfApp_Client.Utility;

namespace WpfApp_Client
{
    public delegate string Del();

    public class Client
    {
        //variabile de clasa
        private ISocketProxy clientSocket;

        private Action<string> handleMessages;

        private IPAddress currentIPAddress;

        private IPEndPoint localEndPoint = null;

        private static int clientPort = 8300;//minus; underlined - but only the name

        private Individual currentIndividual;

        private RecordFile currentFile = new RecordFile();

        private Action reconnect;

        public Client(ISocketProxy socket)
        {
            clientSocket = socket;
        }

        public Client()
        {
            //SetIPAddress();
            //clientSocket = SocketGenerator();
        }

        public void StartClient()
        {
            SetIPAddress();
            clientSocket = SocketGenerator();
            ConnectClientToEndPoint();
            CreateNormalPersonFromClient();
        }

        public Action<string> HandleMessages { get; set; }

        public Individual CurrentIndividual
        {
            get => this.currentIndividual;
            set => this.currentIndividual = value;
        }

        public void CloseSocket()
        {
            clientSocket.Close();
            clientSocket.Dispose();
        }

        public Action Reconnect
        {
            get => this.reconnect;
            set => this.reconnect = value;
        }

        public void DisconnectAction()
        {
            CurrentIndividual.IsConnected = false;
            CurrentIndividual.CloseConnection();
            Console.WriteLine("You are offline.");
        }

        private IPAddress GetIPAddress
        {
            get => currentIPAddress;
        }

        private void SetIPAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            currentIPAddress = ipHostInfo.AddressList[1];
        }

        public RecordFile GetCurrentFile
        {
            get => currentFile;
        }

        private ISocketProxy SocketGenerator()
        {
            SocketProxy socket = new SocketProxy(GetIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);
            return socket;
        }

        public void ConnectClientToEndPoint()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(GetIPAddress, clientPort);
            clientSocket.Connect(remoteEndPoint);
            Console.WriteLine("Socket connected to client {0}\n", this.clientSocket.RemoteEndPoint().ToString());
        }

        public bool HandleLogin(string clientName)
        {
            if (!LoginValidation(clientName))
                return false;
            currentIndividual.Name = clientName;
            return true;
        }

        public void HandleReconnect(string clientName)
        {
            int count = 5;

            while (!CurrentIndividual.IsConnected && count > 0)
            {
                count--;
                StartClient();
                if (CheckServersGreeting())
                {
                    CurrentIndividual.IsConnected = true;
                }
                HandleLogin(clientName);
                CreateThread();
                Thread.Sleep(1000);
            }
        }

        public void CreateNormalPersonFromClient()
        {
            try
            {
                currentIndividual = new NormalPerson(new Person(clientSocket));
                currentIndividual.IsConnected = true;
            }
            catch (Exception ex)
            {
                DisconnectAction();
            }
        }

        public bool CheckServersGreeting()
            => currentIndividual?.ReceiveMessage().CompareTo("S00") ?? false;

        public void Login(string personName)
        {
            currentIndividual.SendMessage(new Message(personName));
        }

        public bool LoginValidation(string personName)
        {
            try
            {
                Login(personName);
                Message msg = currentIndividual.ReceiveMessage();
                return VerifyData(msg);
            }
            catch (Exception ex)
            {
                Reconnect();
                return true;
            }
        }

        public void SendMessage(string msg)
        {
            currentIndividual.SendMessage(new Message(msg));
        }

        private bool IsPersonOnline(Message receivedMessage)
            => string.Compare(receivedMessage.ToString(), "E00") == 0;

        private bool IsPersonValid(Message receivedMessage)
            => string.Compare(receivedMessage.ToString(), "S01") == 0;

        private bool VerifyData(Message msg)
            => (!IsPersonOnline(msg) && IsPersonValid(msg));

        public void HandleReceive(object callback)
        {
            try
            {
                while (currentIndividual.IsConnected)
                {
                    Message serverMessage = currentIndividual.ReceiveMessage();
                    if (serverMessage == null)
                        currentIndividual.IsConnected = false;
                    LogMessage(serverMessage);
                    ((Action<string>)callback)(serverMessage.ToString());
                }
            }
            catch (SocketException ex) when (ex.NativeErrorCode == 10054)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Reconnect();
                    });
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void CreateThread()
        {
            Thread receiveThread = new Thread(HandleReceive);
            receiveThread.Start(HandleMessages);
        }

        public Tuple<MessageType, string> GetMessageType(string serverMessage)
        {
            MessageType msgType = MessageType.Unknown;
            string msg;

            if (serverMessage.StartsWith("M00"))
                ProcessUserOnlineNotification(ref serverMessage, ref msgType);
            else if (serverMessage.StartsWith("M02"))
                msgType = ChatDataType.UsersList;
            else if (serverMessage.StartsWith("M03"))
            {
                currentFile.TryWriteToFile(serverMessage.Substring(3));
                msgType = ChatDataType.MsgHistory;
            }
            else if (serverMessage.StartsWith("M01"))
                ProcessUserOfflineNotification(serverMessage, ref msgType);
            msg = serverMessage.Substring(3);
            return new Tuple<MessageType, string>(msgType, msg);
        }

        private void ProcessUserOfflineNotification(string serverMessage, ref MessageType msgType)
        {
            string userName = serverMessage.Substring(3, serverMessage.Length - 15);
            if (userName == currentIndividual.Name)
            {
                currentFile.TryWriteToFile("You are offline.\n");
                msgType = ChatUserType.Exit;
            }
            else
            {
                msgType = ChatUserType.Off;
                currentFile.TryWriteToFile(serverMessage.Substring(3));
            }
        }

        private void ProcessUserOnlineNotification(ref string serverMessage, ref MessageType msgType)
        {
            string userName = serverMessage.Substring(3, serverMessage.Length - 14);
            if (userName == currentIndividual.Name)
            {
                currentFile.TryWriteToFile("You are online.");
                serverMessage = "M00You are online.";
                msgType = ChatUserType.Enter;
            }
            else
            {
                currentFile.TryWriteToFile(serverMessage.Substring(3));
                msgType = ChatUserType.On;
            }
        }

        private void LogMessage(Message serverMessage)
        {
            Console.WriteLine("{0}\n", serverMessage);
        }
    }
}