using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSocketServer
{
    public static class GameServerExtensions
    {
        public static void UseGameServer(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<GameServerMiddleware>();
        }

        public static void UseSimpleServer(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<SimpleServerMiddleware>();
        }
    }

    public class SimpleServerMiddleware
    {
        private readonly RequestDelegate _next;
        private SimpleWebSocketServer _server;

        public SimpleServerMiddleware(RequestDelegate next)
        {
            _next = next;
            _server = new SimpleWebSocketServer();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
               await _server.ProcessRequest(context);
            }
            else
            {
                await _next(context);
            }
        }
    }

    public class GameServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ServerOld _webSocketServer;

        public GameServerMiddleware(RequestDelegate next, ServerOld server)
        {
            _next = next;
            _webSocketServer = server;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
               await _webSocketServer.ProcessRequest(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}