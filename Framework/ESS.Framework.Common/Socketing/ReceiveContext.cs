#region

using System;

#endregion

namespace ESS.Framework.Common.Socketing
{
    public class ReceiveContext
    {
        public ReceiveContext(SocketInfo replySocketInfo, byte[] receivedMessage, Action<ReceiveContext> messageHandledCallback)
        {
            ReplySocketInfo = replySocketInfo;
            ReceivedMessage = receivedMessage;
            MessageHandledCallback = messageHandledCallback;
        }

        public SocketInfo ReplySocketInfo { get; private set; }
        public byte[] ReceivedMessage { get; private set; }
        public byte[] ReplyMessage { get; set; }
        public Action<SendResult> ReplySentCallback { get; set; }
        public Action<ReceiveContext> MessageHandledCallback { get; private set; }
    }
}