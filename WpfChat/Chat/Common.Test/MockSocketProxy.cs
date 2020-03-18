using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat.Common.Test
{
    public class MockSocketProxy : ISocketProxy
    {
        private readonly ISocketProxy mockSocket;
        public List<string> sentMessage;
        public string receivedMessage;
        public List<byte[]> internalReceiveBuffer;
        private readonly IPAddress mockIPAddress = IPAddress.Loopback;
        private int cursor = 0;
        private int index = 0;

        public SocketFlags socketFlag = SocketFlags.None;

        public MockSocketProxy()
        {
            this.sentMessage = new List<string>();
            this.receivedMessage = "";
            this.internalReceiveBuffer = new List<byte[]>();
        }

        public ISocketProxy Accept() => new MockSocketProxy();

        public void Connect(IPEndPoint remoteEndPoint)
        { }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
        { }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
        { }

        public int Send(byte[] buffer)
        {
            sentMessage.Add(Encoding.UTF8.GetString(buffer, 4, buffer.Length - 4));
            return buffer.Length;
        }

        public int Send(byte[] buffer, SocketFlags socketFlags)
        {
            sentMessage.Add(Encoding.UTF8.GetString(buffer, 4, buffer.Length - 4));
            return buffer.Length;
        }

        public int Receive(byte[] buffer)
        {
            bool nodeFlag = false;

            int len = ProcessInternalReceiveBufferData(buffer, index, ref nodeFlag);
            if (nodeFlag && index < internalReceiveBuffer.Count - 1)
            {
                index++;
                cursor = 0;
            }
            return len;
        }

        private int ProcessInternalReceiveBufferData(byte[] buffer, int index, ref bool nodeFlag)
        {
            int len = Math.Min(buffer.Length, internalReceiveBuffer[index].Length - cursor);
            Array.Copy(internalReceiveBuffer[index], cursor, buffer, 0, len);
            cursor += len;
            if (cursor == internalReceiveBuffer[index].Length)
                nodeFlag = true;
            return len;
        }

        public EndPoint RemoteEndPoint() => new IPEndPoint(mockIPAddress, 0);

        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            bool nodeFlag = false;

            int len = ProcessInternalReceiveBufferData(buffer, index, ref nodeFlag);
            if (nodeFlag && index < internalReceiveBuffer.Count - 1)
            {
                index++;
                cursor = 0;
            }
            return len;
        }

        public byte[] WrapData(byte[] input, int length)
        {
            byte[] prefixLength = BitConverter.GetBytes(length);
            byte[] totalSizeOutput = new byte[prefixLength.Length + input.Length];
            prefixLength.CopyTo(totalSizeOutput, 0);
            input.CopyTo(totalSizeOutput, prefixLength.Length);
            return totalSizeOutput;
        }

        public void Close()
        { }

        public void Dispose()
        { }

        public EndPoint LocalEndPoint() => new IPEndPoint(mockIPAddress, 0);

        public void Bind(EndPoint localEp)
        { }

        public void Listen(int backlog)
        { }

        public void Shutdown(SocketShutdown how)
        { }
    }
}