using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Reversio.Server
{
    public static class GameServerExtensions
    {
        public static void UseGameServer(this IApplicationBuilder builder)
        {
            var webSocketServer = builder.ApplicationServices.GetService(typeof(WebSocketServer));
            builder.UseMiddleware<GameServerMiddleware>(webSocketServer);
        }
    }

    public class GameServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketServer _webSocketServer;

        public GameServerMiddleware(RequestDelegate next, WebSocketServer webSocketServer)
        {
            _next = next;
            _webSocketServer = webSocketServer;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
               await _webSocketServer.Handle(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}