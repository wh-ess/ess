#region

using System;
using System.Net.Sockets;

#endregion

namespace ESS.Framework.Common.Socketing
{
    public class SocketInfo
    {
        public SocketInfo(Socket socket)
        {
            if (!socket.Connected)
            {
                throw new Exception("Invalid socket, socket is disconnected.");
            }
            InnerSocket = socket;
            SocketRemotingEndpointAddress = socket.RemoteEndPoint.ToString();
        }

        public string SocketRemotingEndpointAddress { get; private set; }
        public Socket InnerSocket { get; private set; }

        public void Close()
        {
            if (InnerSocket == null || !InnerSocket.Connected)
            {
                return;
            }

            try
            {
                InnerSocket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
            }
            try
            {
                InnerSocket.Close();
            }
            catch
            {
            }
        }
    }
}