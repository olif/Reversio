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
        private readonly TestServer _testServer;
        private readonly WebSocketClient _client;
        private readonly WebSocketServerStub _stub;

        public WebSocketServerTests()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _client = _testServer.CreateWebSocketClient();
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
        public async Task Test()
        {
            var connHasBeenClosed = false;
            _stub.MessageReceived = async (conn, _) =>
            {
                await Task.Delay(200);
                await conn.CloseConnection();
            };

            var socket = await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);
            var clientConn = new WebSocketConnection(socket, CancellationToken.None);
            clientConn.OnClose = () => connHasBeenClosed = true;
            var thread = new Thread(async () => await clientConn.ProcessRequest(CancellationToken.None));
            thread.Start();
            await socket.SendAsync(GetWebsocketMsg("close"), WebSocketMessageType.Text, true, CancellationToken.None);
            await Task.Delay(500);

            connHasBeenClosed.Should().BeTrue();
        }

        private ArraySegment<byte> GetWebsocketMsg(string msg) => new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));

        //[Fact]
        //public async Task Test()
        //{
        //    bool isOnOpenCalled = false;
        //    bool messageReceived = false;
        //    _stub.ConnectionOpened = (conn) =>
        //    {
        //        isOnOpenCalled = true;
        //    };

        //    _stub.MessageReceived = (conn, msg) =>
        //    {
        //        messageReceived = true;
        //    };

        //    var socket = await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);
        //    await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("hello")), WebSocketMessageType.Text, true,
        //        CancellationToken.None);

        //    Thread.Sleep(100);

        //    isOnOpenCalled.Should().BeTrue();
        //    messageReceived.Should().BeTrue();
        //}

        //[Fact]
        //public async Task CloseTest()
        //{
        //    bool messageReceived = false;
        //    bool connectionClosed = false;
        //    _stub.MessageReceived = async (conn, msg) =>
        //    {
        //        messageReceived = true;
        //        await conn.Send("hej hel");
        //        await conn.CloseConnection();
        //    };

        //    var socket = await _client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);
        //    var s = new WebSocketConnection(socket, CancellationToken.None);
        //    s.OnClose = () =>
        //    {
        //        connectionClosed = true;
        //    };
        //    s.OnMessage = (msg) =>
        //    {
        //        ;
        //    };
        //    var t = new Thread(async () =>
        //    {
        //        await s.ProcessRequest(CancellationToken.None);
        //    });
        //    await s.Send("hello");
        //    t.Start();

        //    while (true) ;

        //    messageReceived.Should().BeTrue();
        //    connectionClosed.Should().BeTrue();
        //}
    }
}