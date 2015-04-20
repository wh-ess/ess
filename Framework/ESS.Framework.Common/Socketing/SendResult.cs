#region

using System;

#endregion

namespace ESS.Framework.Common.Socketing
{
    public class SendResult
    {
        public SendResult(bool success, Exception exception)
        {
            Success = success;
            Exception = exception;
        }

        public bool Success { get; private set; }
        public Exception Exception { get; private set; }
    }
}