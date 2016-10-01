using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSocketServer
{
    public class SimpleWebSocketServer
    {
        private static readonly IDictionary<Guid, WebSocket> ActiveConnections = 
            new ConcurrentDictionary<Guid, WebSocket>();

        public async Task ProcessRequest(HttpContext context)
        {
            try
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync(subProtocol: null);
                var socketId = Guid.NewGuid();
                ActiveConnections.Add(socketId, socket);

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

                        ;
                    }
                }

            }
            catch 
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("", CancellationToken.None);
            }
        }
    }
}
