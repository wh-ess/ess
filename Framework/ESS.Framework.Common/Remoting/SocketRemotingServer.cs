#region

using System;
using System.Collections.Generic;
using System.Text;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Logging;
using ESS.Framework.Common.Socketing;

#endregion

namespace ESS.Framework.Common.Remoting
{
    public class SocketRemotingServer
    {
        private readonly ILogger _logger;
        private readonly Dictionary<int, IRequestHandler> _requestHandlerDict;
        private readonly ServerSocket _serverSocket;

        public SocketRemotingServer(string name, SocketSetting socketSetting, ISocketEventListener socketEventListener = null)
        {
            _serverSocket = new ServerSocket(socketEventListener);
            _requestHandlerDict = new Dictionary<int, IRequestHandler>();
            _logger = ObjectContainer.Resolve<ILoggerFactory>()
                .Create(name ?? GetType()
                    .Name);
            _serverSocket.Bind(socketSetting.Address, socketSetting.Port)
                .Listen(socketSetting.Backlog);
        }

        public void Start()
        {
            _serverSocket.Start(HandleRemotingRequest);
        }

        public void Shutdown()
        {
            _serverSocket.Shutdown();
        }

        public void RegisterRequestHandler(int requestCode, IRequestHandler requestHandler)
        {
            _requestHandlerDict[requestCode] = requestHandler;
        }

        private void HandleRemotingRequest(ReceiveContext receiveContext)
        {
            var remotingRequest = RemotingUtil.ParseRequest(receiveContext.ReceivedMessage);
            var requestHandlerContext = new SocketRequestHandlerContext(receiveContext, remotingRequest, _logger);

            IRequestHandler requestHandler;
            if (!_requestHandlerDict.TryGetValue(remotingRequest.Code, out requestHandler))
            {
                var errorMessage = string.Format("No request handler found for remoting request, request code:{0}", remotingRequest.Code);
                _logger.Error(errorMessage);
                requestHandlerContext.SendRemotingResponse(new RemotingResponse(-1, remotingRequest.Sequence, Encoding.UTF8.GetBytes(errorMessage)));
                return;
            }

            try
            {
                _logger.DebugFormat("Handling remoting request, request code:{0}, request sequence:{1}", remotingRequest.Code,
                    remotingRequest.Sequence);
                var remotingResponse = requestHandler.HandleRequest(requestHandlerContext, remotingRequest);
                if (!remotingRequest.IsOneway && remotingResponse != null)
                {
                    requestHandlerContext.SendRemotingResponse(remotingResponse);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("Exception raised when handling remoting request, request code:{0}.", remotingRequest.Code);
                _logger.Error(errorMessage, ex);
                if (!remotingRequest.IsOneway)
                {
                    requestHandlerContext.SendRemotingResponse(new RemotingResponse(-1, remotingRequest.Sequence, Encoding.UTF8.GetBytes(ex.Message)));
                }
            }
        }
    }
}