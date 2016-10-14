using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Reversio.Domain;
using Reversio.WebSockets;

namespace Reversio.Server
{
    public class WebSocketGameBroker : WebSocketBroker
    {
        private static GameServer _gameServer;
        private IWebSocketConnection c;
        private JsonSerializerSettings _jsonSettings;

        public WebSocketGameBroker(GameServer gameServer, HttpContext context) : base(context)
        {
            _gameServer = gameServer;
            _gameServer.GameStateChanged += OnGameStateChanged;
            _jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        private string ToJson(object o)
        {
            return JsonConvert.SerializeObject(o, _jsonSettings);
        }

        public void OnGameStateChanged(object sender, GameStateChangedEventArgs eventArgs)
        {
            var message = Message.GameStateChangedMessage(eventArgs.CurrentState);
            c.Send(message.ToJson());
        }

        public override void OnConnectionOpened(IWebSocketConnection conn)
        {
            c = conn;
        }

        public override void OnMessageReceived(IWebSocketConnection conn, string message)
        {
            var move = JsonConvert.DeserializeObject<Message>(message);
            switch (move.MessageType)
            {
                case Message.MoveType:
                    var payload = move.Deserialize<MoveModel>();
                    _gameServer.MakeMove(payload.GameId, payload.Bystander.ToBystander(), payload.Position.ToPosition());
                    break;
            }
        }

        public override void OnConnectionClosed(IWebSocketConnection conn)
        {
            ;
        }
    }

    public class MoveModel
    {
        public BystanderModel Bystander { get; set; }

        public PositionModel Position { get; set; }

        public Guid GameId { get; set; }

        public class BystanderModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public Bystander ToBystander()
            {
                return new Bystander(Id, Name);
            }
        }

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


}
