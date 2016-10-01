using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSockets
{
    public class WebSocketConnection : IWebSocketConnection
    {
        public Guid Id { get; }
        internal Action<string> OnMessage { get; set; }
        internal Action OnClose { get; set; }
        internal Action<Exception> Error { get; set; }

        public Task Send(string message)
        {
            throw new NotImplementedException();
        }

        public WebSocketConnection()
        {
            Id = Guid.NewGuid();
            OnOpen = () => { };
        }

        public Action OnOpen { get; set; }

        public async Task ProcessRequest(WebSocket socket, CancellationToken disconnecToken)
        {
            OnOpen();
            var receiveBuffer = new ArraySegment<byte>(new byte[4096]);

            while (socket.State == WebSocketState.Open)
            {
                var result =
                    await socket.ReceiveAsync(receiveBuffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }

                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    var readBytes = receiveBuffer.Count;
                    var str = Encoding.UTF8.GetString(receiveBuffer.Array, 0, result.Count);
                    OnMessage(str);
                }
            }
        }
    }
}
