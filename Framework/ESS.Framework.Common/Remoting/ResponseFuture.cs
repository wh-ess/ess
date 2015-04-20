#region

using System;
using System.Threading.Tasks;

#endregion

namespace ESS.Framework.Common.Remoting
{
    public class ResponseFuture
    {
        private readonly TaskCompletionSource<RemotingResponse> _taskSource;

        public ResponseFuture(RemotingRequest request, long timeoutMillis, TaskCompletionSource<RemotingResponse> taskSource)
        {
            Request = request;
            TimeoutMillis = timeoutMillis;
            _taskSource = taskSource;
            BeginTime = DateTime.Now;
        }

        public DateTime BeginTime { get; private set; }
        public long TimeoutMillis { get; private set; }
        public RemotingRequest Request { get; private set; }

        public bool IsTimeout()
        {
            return (DateTime.Now - BeginTime).TotalMilliseconds > TimeoutMillis;
        }

        public void SetResponse(RemotingResponse response)
        {
            _taskSource.TrySetResult(response);
        }

        public void SetException(Exception exception)
        {
            _taskSource.TrySetException(exception);
        }
    }
}