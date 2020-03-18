using System.Net;
using System.Net.Sockets;

namespace Chat.Common.Test
{
    public class SocketProxy : ISocketProxy
    {
        private readonly Socket tcpSocket;

        public SocketProxy(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            tcpSocket = new Socket(addressFamily, socketType, protocolType);
        }

        public SocketProxy(Socket tcpSocket)
        {
            this.tcpSocket = tcpSocket;
        }

        public ISocketProxy Accept()
            => new SocketProxy(this.tcpSocket.Accept());

        public void Connect(IPEndPoint remoteEndPoint)
        {
            this.tcpSocket.Connect(remoteEndPoint);
        }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
        => this.tcpSocket.SetSocketOption(optionLevel, optionName, optionValue);

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
        => this.tcpSocket.SetSocketOption(optionLevel, optionName, optionValue);

        public int Send(byte[] buffer) => this.tcpSocket.Send(buffer);

        public int Send(byte[] buffer, SocketFlags socketFlags) => this.tcpSocket.Send(buffer, socketFlags);

        public int Receive(byte[] buffer) => this.tcpSocket.Receive(buffer);

        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlag)
            => this.tcpSocket.Receive(buffer, offset, size, SocketFlags.None);

        public EndPoint RemoteEndPoint() => this.tcpSocket.RemoteEndPoint;

        public EndPoint LocalEndPoint() => this.tcpSocket.LocalEndPoint;

        public void Close()
        {
            this.tcpSocket.Close();
        }

        public void Dispose()
        {
            this.tcpSocket.Dispose();
        }

        public void Bind(EndPoint localEp)
        {
            this.tcpSocket.Bind(localEp);
        }

        public void Listen(int backlog)
        {
            this.tcpSocket.Listen(backlog);
        }

        public void Shutdown(SocketShutdown how)
        {
            this.tcpSocket.Shutdown(how);
        }
    }
}