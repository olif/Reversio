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
    public class Class1
    {
        private TestServer _testServer;
        private WebSocketClient _client;
        private WebSocketServerStub _stub;

        public Class1()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _client = _testServer.CreateWebSocketClient();
            _stub = TestStartup.Server;
        }

        [Fact]
        public async Task Test()
        {
            bool isOnOpenCalled = false;
            bool messageReceived = false;
            _stub.ConnectionOpened = (conn) =>
            {
                isOnOpenCalled = true;
            };

            _stub.MessageReceived = (conn, msg) =>
            {
                messageReceived = true;
            };

            var socket = await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("hello")), WebSocketMessageType.Text, true,
                CancellationToken.None);

            Thread.Sleep(100);

            isOnOpenCalled.Should().BeTrue();
            messageReceived.Should().BeTrue();
        }

        [Fact]
        public async Task CloseTest()
        {
            bool messageReceived = false;
            bool connectionClosed = false;
            _stub.MessageReceived = async (conn, msg) =>
            {
                messageReceived = true;
                await conn.Send("hej hel");
                await conn.CloseConnection();
            };

            var socket = await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);
            var s = new WebSocketConnection(socket, CancellationToken.None);
            s.OnClose = () =>
            {
                connectionClosed = true;
            };
            s.OnMessage = (msg) =>
            {
                ;
            };
            var t = new Thread(async () =>
            {
                await s.ProcessRequest(CancellationToken.None);
            });
            await s.Send("hello");
            t.Start();

            while (true) ;

            messageReceived.Should().BeTrue();
            connectionClosed.Should().BeTrue();
        }
    }
}
