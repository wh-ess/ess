#region

using System;

#endregion

namespace ESS.Framework.Common.Socketing
{
    public class SendContext
    {
        public SendContext(SocketInfo targetSocket, byte[] message, Action<SendResult> messageSendCallback)
        {
            TargetSocket = targetSocket;
            Message = message;
            MessageSendCallback = messageSendCallback;
        }

        public SocketInfo TargetSocket { get; private set; }
        public byte[] Message { get; private set; }
        public Action<SendResult> MessageSendCallback { get; private set; }
    }
}