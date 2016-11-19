using System;
using Microsoft.AspNetCore.Mvc;
using Reversio.Domain;

namespace Reversio.Server
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly GameServer _gameServer;

        public GamesController()
        {
            _gameServer = GameServer.Instance;
        }

        [HttpPost("signin")]
        public IActionResult SignInBystander(Person person)
        {
            var bystander = new Participant(person.Name);
            return Ok(bystander);
        }

        [HttpGet("active")]
        public IActionResult GetActiveGames()
        {
            var games = _gameServer.ActiveGames;
            return Ok(games);
        }

        [HttpPost]
        public IActionResult CreateNewGame(BystanderModel model)
        {
            var bystander = model.ToBlackPlayer();
            var game = _gameServer.CreateNewGame(bystander);
            return Ok(game);
        }

        [HttpPost("{gameId:guid}/join")]
        public IActionResult JoinGame(Guid gameId, BystanderModel model)
        {
            var bystander = model.ToWhitePlayer();
            var state = _gameServer.JoinGame(gameId, bystander);
            return Ok(state);
        }

        [HttpPost("{gameId:guid}/observe")]
        public IActionResult ObserveGame(Guid gameId, BystanderModel model)
        {
            var observer = model.ToObserver();
            var state = _gameServer.JoinObserver(gameId, observer);
            return Ok(state);
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }
}

