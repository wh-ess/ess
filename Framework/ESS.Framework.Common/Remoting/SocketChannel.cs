#region

using ESS.Framework.Common.Socketing;

#endregion

namespace ESS.Framework.Common.Remoting
{
    public class SocketChannel : IChannel
    {
        public SocketChannel(SocketInfo socketInfo)
        {
            SocketInfo = socketInfo;
        }

        public SocketInfo SocketInfo { get; private set; }

        public string RemotingAddress { get { return SocketInfo.SocketRemotingEndpointAddress; } }

        public void Close()
        {
            SocketInfo.InnerSocket.Close();
        }

        public override string ToString()
        {
            return RemotingAddress;
        }
    }
}