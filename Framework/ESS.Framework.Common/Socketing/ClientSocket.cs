﻿#region

using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Logging;

#endregion

namespace ESS.Framework.Common.Socketing
{
    public class ClientSocket
    {
        private readonly ILogger _logger;
        private readonly SocketService _socketService;
        private Socket _socket;
        private SocketInfo _socketInfo;

        public ClientSocket(ISocketEventListener socketEventListener)
        {
            _socketService = new SocketService(socketEventListener);
            _logger = ObjectContainer.Resolve<ILoggerFactory>()
                .Create(GetType()
                    .FullName);
        }

        public bool IsConnected { get { return _socketInfo != null && _socketInfo.InnerSocket.Connected; } }
        public SocketInfo SocketInfo { get { return _socketInfo; } }

        public ClientSocket Connect(string address, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.ReceiveBufferSize = 8192;
            _socket.Connect(address, port);
            _socketInfo = new SocketInfo(_socket);
            return this;
        }

        public ClientSocket Start(Action<byte[]> replyMessageReceivedCallback)
        {
            Task.Factory.StartNew(() =>
            {
                _socketService.ReceiveMessage(new SocketInfo(_socket), replyMessage =>
                {
                    try
                    {
                        replyMessageReceivedCallback(replyMessage);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }
                });
            }, TaskCreationOptions.None)
                .ContinueWith(task => _logger.Error(task.Exception), TaskContinuationOptions.OnlyOnFaulted);

            return this;
        }

        public ClientSocket Shutdown()
        {
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
            }
            try
            {
                _socket.Close();
            }
            catch
            {
            }

            return this;
        }

        public ClientSocket SendMessage(byte[] messageContent, Action<SendResult> callback)
        {
            _socketService.SendMessage(_socketInfo, messageContent, callback);
            return this;
        }
    }
}