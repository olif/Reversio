using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reversio.WebSockets
{
    public class WebSocketConnection1 : IDisposable
    {
        private readonly WebSocket _socket;
        private WebSocketState _currentState;
        public Guid Id { get; }
        public Action<WebSocketConnection1> OnConnected;
        public Action<WebSocketConnection1, string> OnMessageReceived;
        public Action<WebSocketConnection1> OnClose;
        private CancellationTokenSource _cancellationToken;
        private Task _task;

        public WebSocketConnection1(WebSocket socket, Guid id)
        {
            _socket = socket;
            Id = id;
            _currentState = WebSocketState.None;
            OnConnected = (conn) => { };
            OnMessageReceived = (conn, msg) => { };
            OnClose = (conn) => { };
            _cancellationToken = new CancellationTokenSource();
        }

        public async Task Send(string message)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await _socket.SendAsync(buffer, WebSocketMessageType.Text, true, _cancellationToken.Token);
        }

        public async Task StartMonitoring()
        {
            while (true)
            {
                if (_cancellationToken.Token.IsCancellationRequested)
                {
                    break;
                }

                if (_currentState != _socket.State)
                {
                    _currentState = _socket.State;
                    switch (_socket.State)
                    {
                        case WebSocketState.Open:
                            OnConnected(this);
                            break;
                        case WebSocketState.Closed:
                        case WebSocketState.CloseReceived:
                            OnConnectionClose();
                            break;
                    }
                }
                if (_socket.State == WebSocketState.Open)
                {
                    var token = CancellationToken.None;
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    var msg = await _socket.ReceiveAsync(buffer, _cancellationToken.Token);
                    switch (msg.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            var str = Encoding.UTF8.GetString(buffer.Array);
                            OnMessageReceived(this, str);
                            break;
                        case WebSocketMessageType.Close:
                            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                            break;
                    }
                }
            }
        }

        public async Task Close()
        {
            _cancellationToken.Cancel();
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
        }

        private void OnConnectionClose()
        {
            OnClose(this);
        }

        public void Dispose()
        {
            ;
        }
    }
}
