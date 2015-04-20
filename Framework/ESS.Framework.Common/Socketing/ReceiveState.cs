#region

using System;
using System.Collections.Generic;

#endregion

namespace ESS.Framework.Common.Socketing
{
    public class ReceiveState
    {
        public const int BufferSize = 8192;
        public byte[] Buffer = new byte[BufferSize];
        public int? MessageSize;
        public List<byte> ReceivedData = new List<byte>();

        public ReceiveState(SocketInfo sourceSocket, int receiveSize, Action<byte[]> messageReceivedCallback)
        {
            SourceSocket = sourceSocket;
            ReceiveSize = receiveSize;
            MessageReceivedCallback = messageReceivedCallback;
        }

        public int ReceiveSize { get; set; }

        public SocketInfo SourceSocket { get; private set; }
        public Action<byte[]> MessageReceivedCallback { get; private set; }

        public void ClearBuffer()
        {
            for (var index = 0;index < BufferSize;index++)
                Buffer[index] = 0;
        }
    }
}