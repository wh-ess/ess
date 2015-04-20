﻿#region

using System.Threading;

#endregion

namespace ESS.Framework.Common.Remoting
{
    public class RemotingRequest : RemotingMessage
    {
        private static long _sequence;

        public RemotingRequest(int code, byte[] body) : this(code, body, false)
        {
        }

        public RemotingRequest(int code, byte[] body, bool isOneway) : this(code, Interlocked.Increment(ref _sequence), body, isOneway)
        {
        }

        public RemotingRequest(int code, long sequence, byte[] body, bool isOneway) : base(code, sequence, body)
        {
            IsOneway = isOneway;
        }

        public bool IsOneway { get; set; }

        public override string ToString()
        {
            return string.Format("[Code:{0}, Sequence:{1}, IsOneway:{2}]", Code, Sequence, IsOneway);
        }
    }
}