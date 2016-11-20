using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Reversio.Domain;
using Reversio.Domain.Events;
using Reversio.WebSockets;

namespace Reversio.Server
{
    public class WebSocketGameServer : WebSocketServer
    {
        private readonly GameServer _gameServer;

        /// <summary>
        /// All sessions with corresponding connection
        /// </summary>
        private readonly IDictionary<string, IWebSocketConnection> _sessions = new ConcurrentDictionary<string, IWebSocketConnection>();

        public WebSocketGameServer(GameServer gameServer)
        {
            _gameServer = gameServer;
            _gameServer.GameStateChanged += OnGameStateChanged;
            _gameServer.GameCreated += OnGameCreated;
            _gameServer.GameStarted += OnGameStarted;
        }

        private void OnGameStarted(object sender, Participant participant, GameStartedEventArgs eventargs)
        {
            IWebSocketConnection conn;
            if (_sessions.TryGetValue(participant.Name, out conn))
            {
                var msg = Message.GameStarted(eventargs);
                conn.Send(msg.ToJson());
            }
        }

        private void OnGameCreated(object sender, GameCreatedEventArgs eventargs)
        {
            foreach (var conn in _sessions.Values)
            {
                var msg = Message.GameCreated(eventargs);
                conn.Send(msg.ToJson());
            }
        }

        public void OnGameStateChanged(object sender, Participant participant, GameStateChangedEventArgs eventArgs)
        {
            IWebSocketConnection connection;
            var message = Message.GameStateChangedMessage(eventArgs);
            if (_sessions.TryGetValue(participant.Name, out connection))
            {
                connection.Send(message.ToJson());
            }
        }

        public override void OnConnectionOpened(IWebSocketConnection conn, IQueryCollection query)
        {
            var participantName = query["name"];
            _sessions.Add(participantName, conn);
        }

        public override void OnMessageReceived(IWebSocketConnection conn, string message)
        {
            var move = JsonConvert.DeserializeObject<Message>(message);
            switch (move.MessageType)
            {
                case MessageType.Move:
                    var movePayload = move.Deserialize<MoveModel>();
                    _gameServer.MakeMove(movePayload.GameId, movePayload.Bystander.Name, movePayload.Position.ToPosition());
                    break;

                case MessageType.StartGameWithRandomPlayer:
                    var newGamePayload = move.Deserialize<RandomGameModel>();
                    var observer = newGamePayload.Bystander.ToObserver();
                    _gameServer.PutObserverInNewGameQueue(observer);
                    break;
            }
        }

        public override void OnConnectionClosed(IWebSocketConnection conn)
        {
            ;
        }
    }

    public class RandomGameModel
    {
        public BystanderModel Bystander { get; set; }
    }

    public class MoveModel
    {
        public BystanderModel Bystander { get; set; }

        public PositionModel Position { get; set; }

        public Guid GameId { get; set; }

        public class PositionModel
        {
            public int X { get; set; }

            public int Y { get; set; }

            public Position ToPosition()
            {
                return new Position(X, Y);
            }
        }
    }

    public class BystanderModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Participant ToObserver()
        {
            return new Participant(Id, Name);
        }

        public BlackPlayer ToBlackPlayer()
        {
            return new BlackPlayer(Id, Name);
        }

        public WhitePlayer ToWhitePlayer()
        {
            return new WhitePlayer(Id, Name);
        }
    }
}
