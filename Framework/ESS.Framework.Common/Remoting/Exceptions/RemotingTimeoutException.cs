#region

using System;

#endregion

namespace ESS.Framework.Common.Remoting.Exceptions
{
    public class RemotingTimeoutException : Exception
    {
        public RemotingTimeoutException(string address, RemotingRequest request, long timeoutMillis)
            : base(string.Format("Wait response on the channel <{0}> timeout, request:{1}, timeoutMillis:{2}ms", address, request, timeoutMillis))
        {
        }
    }
}