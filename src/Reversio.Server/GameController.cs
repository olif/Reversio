using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reversio.Domain;

namespace Reversio.Server
{
    [Route("api/[controller]")]
    [Authorize]
    public class GamesController : Controller
    {
        private readonly GameEngine _gameEngine;

        public GamesController()
        {
            _gameEngine = GameEngine.Instance;
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            return Ok(User.Identity.Name);
        }

        [HttpPost("signin")]
        public IActionResult SignInPlayer(Player player)
        {
            _gameEngine.RegisterPlayer(player);
            return Ok();
        }

        [HttpGet("active")]
        public IActionResult GetActiveGames()
        {
            var games = _gameEngine.ActiveGames;
            return Ok(games);
        }

        [HttpPost]
        public IActionResult CreateNewGame(Player player)
        {
            var game = _gameEngine.CreateNewGame(player);
            return Ok(game);
        }

        [HttpPost("{gameId:guid}/join")]
        public IActionResult JoinGame(Guid gameId, Player player)
        {
            var state = _gameEngine.JoinGame(gameId, player);
            return Ok(state);
        }

        [HttpPost("{gameId:guid}/observe")]
        public IActionResult ObserveGame(Guid gameId, Player player)
        {
            throw new NotImplementedException();
            //var observer = model.ToObserver();
            //var state = _gameServer.JoinObserver(gameId, observer);
            //return Ok(state);
        }
    }
}

