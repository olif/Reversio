using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reversio.Server
{
    public delegate void ConnectionOpenDelegate();

    public delegate void MessageRecievedDelegate();

    public class WebSocketConnection : IDisposable
    {
        private readonly WebSocket _socket;
        private WebSocketState _currentState;
        private Guid _id;

        public WebSocketConnection(WebSocket socket, Guid id)
        {
            _socket = socket;
            _id = id;
            _currentState = WebSocketState.None;
        }

        public event ConnectionOpenDelegate ConnectionOpen;
        public event MessageRecievedDelegate MessageReceived;

        protected virtual void OnConnectionOpen()
        {
            ConnectionOpen?.Invoke();
        }

        protected virtual void OnMessageReceived()
        {
            MessageReceived?.Invoke();
        }

        public async Task Send(string message)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await _socket.SendAsync(buffer, WebSocketMessageType.Text, true, new CancellationToken());
        }

        public async Task StartMonitoring()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (_currentState != _socket.State)
                    {
                        _currentState = _socket.State;
                        switch (_socket.State)
                        {
                            case WebSocketState.Open:
                                OnConnectionOpen();
                                break;
                            case WebSocketState.CloseReceived:
                                OnConnectionClose();
                                break;
                        }
                    }
                    var token = CancellationToken.None;
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    var msg = await _socket.ReceiveAsync(buffer, token);
                    switch (msg.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            var request = Encoding.UTF8.GetString(
                                buffer.Array,
                                buffer.Offset,
                                buffer.Count);

                            var type = WebSocketMessageType.Text;
                            var data = Encoding.UTF8.GetBytes("Echo echo ");
                            buffer = new ArraySegment<byte>(data);
                            OnMessageReceived();
                            //await _socket.SendAsync(buffer, type, true, token);
                            break;
                    }
                }
            });
        }

        private void OnConnectionClose()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            ;
        }
    }
}
