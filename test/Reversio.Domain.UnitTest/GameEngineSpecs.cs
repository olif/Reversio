using System;
using FluentAssertions;
using Reversio.Domain.Events;
using Xunit;

namespace Reversio.Domain.UnitTest
{
    public class GameEngineSpecs
    {
        private GameEngine _sut;

        public GameEngineSpecs()
        {
            _sut = GameEngine.Instance;
        }

        [Fact]
        public void Cannot_put_non_registered_player_in_waiting_queue()
        {
            var player = new BlackPlayer("not registered");
            Action joinGame = () => _sut.CreateNewGame(player);

            joinGame.ShouldThrow<PlayerNotRegisteredException>();
        }

        [Fact]
        public void Creates_new_game_when_two_participants_has_joined_the_waiting_queue()
        {
            bool partipant1JoinedGame = false;
            bool participant2JoinedGame = false;
            var participant1 = new Player("p1");
            var participant2 = new Player("p2");
            _sut.RegisterPlayer(participant1);
            _sut.RegisterPlayer(participant2);
            _sut.GameStarted += (object sender, Player participant, GameStartedEventArgs args) =>
            {
                if (participant1 == participant)
                {
                    partipant1JoinedGame = true;
                }

                if (participant2 == participant)
                {
                    participant2JoinedGame = true;
                }
            };

            _sut.PutPlayerInQueue(participant1);
            _sut.PutPlayerInQueue(participant2);

            partipant1JoinedGame.Should().BeTrue();
            participant2JoinedGame.Should().BeTrue();
        }

        [Fact]
        public void Player_can_make_a_move_if_it_is_the_players_turn()
        {
            var part1 = new BlackPlayer("p1");
            var part2 = new WhitePlayer("p2");
            _sut.RegisterPlayer(part1);
            _sut.RegisterPlayer(part2);
            var gameState = _sut.CreateNewGame(part1);
            _sut.JoinGame(gameState.GameId, part2);

            var flippedPieces = _sut.MakeMove(gameState.GameId, part1, new Position(2, 3));

            flippedPieces.Should().NotBeNull();
        }

        [Fact]
        public void Player_cannot_make_a_move_if_it_is_not_the_players_turn()
        {
            var part1 = new BlackPlayer("p1");
            var part2 = new WhitePlayer("p2");
            _sut.RegisterPlayer(part1);
            _sut.RegisterPlayer(part2);
            var gameState = _sut.CreateNewGame(part1);
            _sut.JoinGame(gameState.GameId, part2);

            var flippedPieces = _sut.MakeMove(gameState.GameId, part2, new Position(5, 3));

            flippedPieces.Should().BeNull();
        }

        [Fact]
        public void A_player_cannot_make_a_move_when_he_is_not_part_of_the_game()
        {
            var part1 = new BlackPlayer("p1");
            var part2 = new WhitePlayer("p2");
            var part3 = new BlackPlayer("p3");
            _sut.RegisterPlayer(part1);
            _sut.RegisterPlayer(part2);
            _sut.RegisterPlayer(part3);
            var gameState = _sut.CreateNewGame(part1);
            _sut.JoinGame(gameState.GameId, part2);

            Action moveOnWrongGame = () => _sut.MakeMove(gameState.GameId, part3, new Position(5, 3));

            moveOnWrongGame.ShouldThrow<ArgumentException>();
        }
    }
}
