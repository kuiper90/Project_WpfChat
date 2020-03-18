using System;
using System.Net.Sockets;
using Chat.Common.Test;

namespace Chat.Common
{
    public class Buffer
    {
        private const int MaxChunkLength = 1024;

        private byte[] data;

        public Buffer()
        {
            data = new byte[MaxChunkLength];
            ReadNoOfBytesReceived = 0;
        }

        public int ReadNoOfBytesReceived
        { get; private set; }

        public byte[] Read(ISocketProxy proxy)
        {
            int totalLength = BitConverter.ToInt32(ReadUntil(proxy, sizeof(int)), 0);
            ValidateLength(totalLength);

            return ReadUntil(proxy, totalLength);
        }

        private byte[] ReadUntil(ISocketProxy proxy, int totalLength)
        {
            while (ReadNoOfBytesReceived < totalLength)
            {
                ReadChunk(proxy);
            }
            return ExtractReceivedData(totalLength);
        }

        private void ReadChunk(ISocketProxy proxy)
        {
            AdjustCapacity();
            int noOfBytesInChunk = proxy.Receive(data, ReadNoOfBytesReceived, data.Length - ReadNoOfBytesReceived, SocketFlags.None);
            ValidateRead(noOfBytesInChunk);
            ReadNoOfBytesReceived += noOfBytesInChunk;
        }

        private void AdjustCapacity()
        {
            if (ReadNoOfBytesReceived == data.Length)
            {
                Array.Resize(ref data, ReadNoOfBytesReceived * 2);
            }
        }

        private byte[] ExtractReceivedData(int totalLength)
        {
            //ValidateSize(totalLength);
            var result = new byte[totalLength];
            Array.Copy(data, result, totalLength);
            Array.Copy(data, totalLength, data, 0, ReadNoOfBytesReceived - totalLength);
            ReadNoOfBytesReceived -= totalLength;
            return result;
        }

        private void ValidateSize(int size)
        {
            if (ReadNoOfBytesReceived < size)
                throw new Exception("Invalid extract from buffer.");
        }

        private void ValidateLength(int length)
        {
            if (length >= 2147483647)
                throw new Exception("Maximum limit of the integer interval has been exceeded.");
        }

        private void ValidateRead(int noOfBytesRead)
        {
            if (noOfBytesRead == 0)
                throw new Exception("You've received zero bytes. Please, check your connection.");
        }
    }
}