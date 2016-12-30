using System;
using System.Linq;
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
            var allPlayers = _gameEngine.RegisteredPlayers;
            var otherPlayers = allPlayers.Where(x => x != new Player(User.Identity.Name));

            return Ok(otherPlayers);
        }

        [HttpPost("games/new")]
        public IActionResult CreateNewGame()
        {
            var player = GetPlayer();
            var game = _gameEngine.CreateNewGame(player);
            return Ok(game);
        }

        [HttpPost("invite")]
        public IActionResult InviteOpponent([FromBody] GameInvitationModel invitation)
        {
            Player player = GetPlayer();
            var isChallangeSent = _gameEngine.TryInvitePlayerToGame(player, invitation.Opponent);
            if (isChallangeSent)
            {
                return Ok();
            }

            return BadRequest("Opponent not available");
        }

        private Player GetPlayer()
        {
            var playerName = User.Identity.Name;
            var player = new Player(playerName);
            return player;
        }

        [HttpPost("{gameId:guid}/join")]
        public IActionResult JoinGame(Guid gameId)
        {
            var player = GetPlayer();
            var state = _gameEngine.JoinGame(gameId, player);
            return Ok(state);
        }

        [HttpPost("{gameId:guid}/observe")]
        public IActionResult ObserveGame(Guid gameId, Player player)
        {
            _gameEngine.ObserveGame(gameId, player);
            return Ok();
        }

        [HttpPost("randomgame")]
        public IActionResult StartRandomGame()
        {
            var player = GetPlayer();
            _gameEngine.PutPlayerInQueue(player);
            return Ok();
        }

        [HttpPost("{gameId:guid}/move")]
        public IActionResult MakeMove(Guid gameId, [FromBody] MoveModel.PositionModel model)
        {
            var player = GetPlayer();
            var discsFlipped = _gameEngine.MakeMove(gameId, player, model.ToPosition());
            return Ok(discsFlipped != null && discsFlipped.Count > 0);
        }
    }
}

