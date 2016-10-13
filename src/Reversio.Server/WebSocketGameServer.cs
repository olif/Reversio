using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reversio.Domain;
using Reversio.WebSockets;

namespace Reversio.Server
{
    public class WebSocketGameServer : WebSocketServer
    {
        private GameServer _gameServer;

        public WebSocketGameServer(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public override void OnConnectionOpened(IWebSocketConnection conn)
        {
            ;
        }

        public override void OnMessageReceived(IWebSocketConnection conn, string message)
        {
            var move = JsonConvert.DeserializeObject<MoveModel>(message);
            var bystander = move.Bystander.ToBystander();
            var position = move.Position.ToPosition();
            var result = _gameServer.MakeMove(move.GameId, bystander, position);
            conn.Send(JsonConvert.SerializeObject(result));
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
