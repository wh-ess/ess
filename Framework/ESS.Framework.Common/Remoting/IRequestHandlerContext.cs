#region

using System;

#endregion

namespace ESS.Framework.Common.Remoting
{
    public interface IRequestHandlerContext
    {
        IChannel Channel { get; }
        Action<RemotingResponse> SendRemotingResponse { get; }
    }
}