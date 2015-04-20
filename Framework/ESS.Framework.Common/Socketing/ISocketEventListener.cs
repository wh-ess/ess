#region

using System.Net.Sockets;

#endregion

namespace ESS.Framework.Common.Socketing
{
    public interface ISocketEventListener
    {
        void OnNewSocketAccepted(SocketInfo socketInfo);
        void OnSocketException(SocketInfo socketInfo, SocketException socketException);
    }
}