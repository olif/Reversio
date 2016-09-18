using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reversio.Domain;

namespace Reversio.Server
{
    public class GamesController : Controller
    {
        private readonly GameServer _gameServer;

        public GamesController(GameServer server)
        {
            _gameServer = server;
        }

        public IActionResult CreateNewGame(Bystander bystander)
        {
            var game = _gameServer.CreateNewGame(bystander);
            return Ok(game.CurrentState);
        }
    }
}
