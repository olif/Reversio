using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;
using FluentAssertions.Events;

namespace Reversio.Domain.UnitTest
{
    public class GameSpecs
    {
        private Bystander _blackPlayer;
        private Bystander _whitePlayer;

        public GameSpecs()
        {
            _blackPlayer = new Bystander();
            _whitePlayer = new Bystander();
        }

        [Fact]
        public void Cannot_Join_More_Than_One_Opponent()
        {
            var game = new Game(_blackPlayer);
            game.JoinOpponent(_whitePlayer);

            Action action = () => game.JoinOpponent(new Bystander());

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BlackPlayer_CanMakeMove_BeforeOpponentHasJoined()
        {
            var game = new Game(_blackPlayer);
            var player = game.GetActivePlayer(_blackPlayer.Id);
            var result = game.PlayerMakesMove(player, new Position(4, 5));

            result.Should().BeTrue();
        }

        [Fact]
        public void Player_Cannot_Make_Multiple_Moves_If_There_Is_A_Valid_Move_For_Opponent()
        {
            var game = new Game(_blackPlayer);
            var player = game.GetActivePlayer(_blackPlayer.Id);
            game.PlayerMakesMove(player, new Position(4, 5));

            var result = game.PlayerMakesMove(player, new Position(2, 3));

            result.Should().BeFalse();
        }

        [Fact]
        public void Player_Can_Make_Multiple_Moves_If_There_Is_No_Valid_Move_For_Opponent()
        {
            var positions = new[,]
            {

                    {' ', ' ', ' ', ' ', ' ', ' ', 'X', 'X'},

                    {' ', ' ', ' ', ' ', ' ', ' ', 'O', 'X'},

                    {' ', ' ', ' ', ' ', ' ', ' ', 'O', 'X'},

                    {'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O'},

                    {' ', ' ', ' ', ' ', ' ', ' ', 'O', ' '},

                    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}
                };

            var board = new Board(positions.Translate());
            var game = new Game(_blackPlayer, board);
            game.JoinOpponent(_whitePlayer);
            var player = game.GetActivePlayer(_blackPlayer.Id);
            game.PlayerMakesMove(player, new Position(5, 0));
            
            var result = game.PlayerMakesMove(player, new Position(5, 2));

            result.Should().BeTrue();
        }

        [Fact]
        public void GameIsFinished_If_No_Moves_Are_Left()
        {
            var gameFinished = false;
            var positions = new[,]
            {

                {' ', ' ', ' ', ' ', ' ', ' ', 'X', 'X'},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X'},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X'},

                {' ', 'O', 'O', 'O', 'O', 'O', 'O', 'X'},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}
            };

            var board = new Board(positions.Translate());
            var game = new Game(_blackPlayer, board);
            var player = game.GetActivePlayer(_blackPlayer.Id);
            game.JoinOpponent(_whitePlayer);
            game.GameFinished += (o, e) => gameFinished = true;
            game.PlayerMakesMove(player, new Position(0, 3));

            gameFinished.Should().BeTrue();
        }
    }
}
