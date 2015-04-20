#region

using System;

#endregion

namespace ESS.Framework.Common.Remoting.Exceptions
{
    public class RemotingServerUnAvailableException : Exception
    {
        public RemotingServerUnAvailableException(string address, int port)
            : base(string.Format("Remoting server is unavailable, server address:{0}:{1}.", address, port))
        {
        }
    }
}