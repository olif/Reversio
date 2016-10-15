using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Reversio.WebSockets;
using Xunit;

namespace Reversio.Server.IntegrationTests
{
    public class WebSocketServerTests
    {
        private readonly WebSocketClient _client;
        private readonly WebSocketBrokerStub _stub;

        public WebSocketServerTests()
        {
            var testServer = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _client = testServer.CreateWebSocketClient();
            _stub = TestStartup.Server;
        }

        [Fact]
        public async Task ConnectionOpened_IsCalled_WhenConnectionHasEstablished()
        {
            bool onOpenCalled = false;
            _stub.ConnectionOpened = (_) => onOpenCalled = true;
            await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);

            await Task.Delay(100);
            onOpenCalled.Should().BeTrue();
        }

        [Fact]
        public async Task MessageRecieved_IsCalledWithTheMessage_WhenMessageIsReceived()
        {
            var msg = "hello";
            var recievedMsg = "";
            _stub.MessageReceived = (conn, rcvdMsg) => { recievedMsg = rcvdMsg; };
            var socket = await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);
            await socket.SendAsync(GetWebsocketMsg(msg), WebSocketMessageType.Text, true, CancellationToken.None);

            await Task.Delay(100);
            recievedMsg.Should().Equals(msg);
        }

        [Fact]
        public async Task ConnectionClosed_IsCalled_WhenConnectionHasBeenClosed()
        {
            var connectionClosedCalled = false;
            _stub.ConnectionClosed = (_) => connectionClosedCalled = true;
            var socket = await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);

            await Task.Delay(100);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

            await Task.Delay(100);
            connectionClosedCalled.Should().BeTrue();
        }

        [Fact]
        public async Task CloseInitiatedByClient_InvokesOnCloseOnServer()
        {
            var connHasBeenClosed = false;
            _stub.MessageReceived = async (conn, _) =>
            {
                await Task.Delay(200);
                await conn.CloseConnection();
            };

            var socket = await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);
            var clientConn = new WebSocketConnection(socket, CancellationToken.None)
            {
                OnClose = () => connHasBeenClosed = true
            };

            var thread = new Thread(async () => await clientConn.ProcessRequest(CancellationToken.None));
            thread.Start();

            await socket.SendAsync(GetWebsocketMsg("close"), WebSocketMessageType.Text, true, CancellationToken.None);

            // This delay must be increased if running test in debug-mode
            await Task.Delay(400);

            connHasBeenClosed.Should().BeTrue();
        }

        private ArraySegment<byte> GetWebsocketMsg(string msg) => new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
    }
}