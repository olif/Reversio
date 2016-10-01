using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversio.WebSocketServer;

namespace Reversio.Server.IntegrationTests
{
    public class WebSocketServerStub : WebSocketServer.ServerOld
    {
        public Action<WebSocketConnection> OnConnectedAction;
        public Action<WebSocketConnection, string> OnMessageReceivedAction;
        public Action<WebSocketConnection> OnCloseAction;

        public IEnumerable<WebSocketConnection> CurrentActiveConnections => base.ActiveConnections;

        public WebSocketServerStub()
        {
            OnConnectedAction = (conn) => { };
            OnMessageReceivedAction = (conn, msg) => { };
            OnCloseAction = (conn) => { };
        }

        protected override Task OnConnected(WebSocketConnection connection)
        {
            OnConnectedAction(connection);
            return Task.FromResult(0);
        }

        protected override Task OnMessageReceived(WebSocketConnection connection, string msg)
        {
            OnMessageReceivedAction(connection, msg);
            return Task.FromResult(0);
        }

        protected override Task OnClose(WebSocketConnection connection)
        {
            OnCloseAction(connection);
            return Task.FromResult(0);
        }
    }
}
