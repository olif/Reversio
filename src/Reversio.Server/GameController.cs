using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reversio.Domain;
using Reversio.Server.Models;

namespace Reversio.Server
{
    [Route("api")]
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

        [HttpGet("players")]
        public IActionResult GetRegisteredPlayers()
        {
            var players = _gameEngine.RegisteredPlayers;
            return Ok(players);
        }

        [HttpPost]
        public IActionResult CreateNewGame(Player player)
        {
            var game = _gameEngine.CreateNewGame(player);
            return Ok(game);
        }

        [HttpPost("invite")]
        public IActionResult InviteOpponent(GameInvitationModel invitation)
        {
            var playerName = User.Identity.Name;
            var player = new Player(playerName);
            var isChallangeSent = _gameEngine.TryInvitePlayerToGame(player, invitation.Opponent);
            if (isChallangeSent)
            {
                return Ok();
            }

            return BadRequest("Opponent not available");
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
        }
    }
}

