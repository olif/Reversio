using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reversio.WebSockets
{
    public class WebSocketServerMiddleware
    {
        private readonly WebSocketServer _server;
        private readonly RequestDelegate _next;

        public WebSocketServerMiddleware(RequestDelegate next, WebSocketServer server)
        {
            _next = next;
            _server = server;
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
}
