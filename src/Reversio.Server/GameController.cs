using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reversio.Domain;

namespace Reversio.Server
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly GameServer _gameServer;

        public GameController(GameServer server)
        {
            _gameServer = server;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Ping");
        }

        public IActionResult CreateNewGame(Bystander bystander)
        {
            var game = _gameServer.CreateNewGame(bystander);
            return Ok(game.CurrentState);
        }

        [HttpPost("signin")]
        public IActionResult SignInBystander(Person person)
        {
            var bystander = new Bystander(person.Name);
            _gameServer.RegisterBystander(bystander);
            return Ok(bystander);
        }

        [HttpGet("games/active")]
        public IActionResult GetActiveGames()
        {
            var games = _gameServer.ActiveGames;
            return Ok(games);
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }
}
