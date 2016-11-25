using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Reversio.Domain;
using Reversio.Domain.Events;
using Reversio.WebSockets;

namespace Reversio.Server
{
    public class WebSocketGameListener : WebSocketServer
    {
        private GameEngine _gameEngine;
        private IDictionary<ClaimsPrincipal, IWebSocketConnection> _activeSessions = new ConcurrentDictionary<ClaimsPrincipal, IWebSocketConnection>();

        public WebSocketGameListener(GameEngine gameEngine)
        {
            _gameEngine = gameEngine;
            _gameEngine.GameStarted += OnGameStarted;
            _gameEngine.GameStateChanged += OnGameStateChanged;
        }

        protected override void OnConnectionClosed(IWebSocketConnection conn)
        {
            var entry = _activeSessions.FirstOrDefault(x => x.Value.Id == conn.Id);
            _activeSessions.Remove(entry.Key);
        }

        protected override void OnConnectionOpened(IWebSocketConnection conn, HttpContext context)
        {
            var user = context.User;
            _activeSessions.Add(user, conn);
            _gameEngine.RegisterPlayer(new Player(user.Identity.Name));
        }

        protected override void OnMessageReceived(IWebSocketConnection conn, string message)
        {
            var move = JsonConvert.DeserializeObject<Message>(message);
            var principal = _activeSessions.FirstOrDefault(x => x.Value.Id == conn.Id).Key;
            var player = new Player(principal.Identity.Name);
            switch (move.MessageType)
            {
                case MessageType.Move:
                    var movePayload = move.Deserialize<MoveModel>();
                    _gameEngine.MakeMove(movePayload.GameId, player, movePayload.Position.ToPosition());
                    break;

                case MessageType.StartGameWithRandomPlayer:
                    _gameEngine.PutPlayerInQueue(player);
                    break;
            }
        }

        private void OnGameStarted(object sender, Player player, GameStartedEventArgs eventargs)
        {
            var session = _activeSessions.FirstOrDefault(x => x.Key.Identity.Name == player.Name);
            if(!session.Equals(default(KeyValuePair<ClaimsPrincipal, IWebSocketConnection>)))
            {
                var msg = Message.GameStarted(eventargs);
                session.Value.Send(msg.ToJson());
            }
        }

        private void OnGameCreated(object sender, GameCreatedEventArgs eventargs)
        {
            foreach (var conn in _activeSessions.Values)
            {
                var msg = Message.GameCreated(eventargs);
                conn.Send(msg.ToJson());
            }
        }

        public void OnGameStateChanged(object sender, Player player, GameStateChangedEventArgs eventArgs)
        {
            var session = _activeSessions.FirstOrDefault(x => x.Key.Identity.Name == player.Name);
            if (!session.Equals(default(KeyValuePair<ClaimsPrincipal, IWebSocketConnection>)))
            {
                var msg = Message.GameStateChangedMessage(eventArgs);
                session.Value.Send(msg.ToJson());
            }
        }
    }
    //public class WebSocketGameServer : WebSocketServer
    //{
    //    private readonly GameServer _gameServer;

    //    /// <summary>
    //    /// All sessions with corresponding connection
    //    /// </summary>
    //    private readonly IDictionary<string, IWebSocketConnection> _sessions = new ConcurrentDictionary<string, IWebSocketConnection>();

    //    public WebSocketGameServer(GameServer gameServer)
    //    {
    //        _gameServer = gameServer;
    //        _gameServer.GameStateChanged += OnGameStateChanged;
    //        _gameServer.GameCreated += OnGameCreated;
    //        _gameServer.GameStarted += OnGameStarted;
    //    }

    //    private void OnGameStarted(object sender, Participant participant, GameStartedEventArgs eventargs)
    //    {
    //        IWebSocketConnection conn;
    //        if (_sessions.TryGetValue(participant.Name, out conn))
    //        {
    //            var msg = Message.GameStarted(eventargs);
    //            conn.Send(msg.ToJson());
    //        }
    //    }

    //    private void OnGameCreated(object sender, GameCreatedEventArgs eventargs)
    //    {
    //        foreach (var conn in _sessions.Values)
    //        {
    //            var msg = Message.GameCreated(eventargs);
    //            conn.Send(msg.ToJson());
    //        }
    //    }

    //    public void OnGameStateChanged(object sender, Participant participant, GameStateChangedEventArgs eventArgs)
    //    {
    //        IWebSocketConnection connection;
    //        var message = Message.GameStateChangedMessage(eventArgs);
    //        if (_sessions.TryGetValue(participant.Name, out connection))
    //        {
    //            connection.Send(message.ToJson());
    //        }
    //    }

    //    public override void OnConnectionOpened(IWebSocketConnection conn, IQueryCollection query)
    //    {
    //        var participantName = query["name"];
    //        _sessions.Add(participantName, conn);
    //    }

    //    public override void OnMessageReceived(IWebSocketConnection conn, string message)
    //    {
    //        var move = JsonConvert.DeserializeObject<Message>(message);
    //        switch (move.MessageType)
    //        {
    //            case MessageType.Move:
    //                var movePayload = move.Deserialize<MoveModel>();
    //                _gameServer.MakeMove(movePayload.GameId, movePayload.Bystander.Name, movePayload.Position.ToPosition());
    //                break;

    //            case MessageType.StartGameWithRandomPlayer:
    //                var newGamePayload = move.Deserialize<RandomGameModel>();
    //                var observer = newGamePayload.Bystander.ToObserver();
    //                _gameServer.PutObserverInNewGameQueue(observer);
    //                break;
    //        }
    //    }

    //    public override void OnConnectionClosed(IWebSocketConnection conn)
    //    {
    //        ;
    //    }
    //}
}
