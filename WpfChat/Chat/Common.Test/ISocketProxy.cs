using System.Net;
using System.Net.Sockets;

namespace Chat.Common.Test
{
    public interface ISocketProxy
    {
        ISocketProxy Accept();

        void Connect(IPEndPoint remoteEndPoint);

        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue);

        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue);

        int Send(byte[] buffer);

        int Send(byte[] buffer, SocketFlags socketFlags);

        int Receive(byte[] buffer);

        int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlag);

        EndPoint RemoteEndPoint();

        EndPoint LocalEndPoint();

        void Bind(EndPoint localEp);

        void Listen(int backlog);

        void Close();

        void Dispose();

        void Shutdown(SocketShutdown how);
    }
}