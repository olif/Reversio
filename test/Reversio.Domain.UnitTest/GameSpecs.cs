using System;
using FluentAssertions;
using Xunit;

namespace Reversio.Domain.UnitTest
{
    public class GameSpecs
    {
        private readonly BlackPlayer _blackPlayer;
        private readonly WhitePlayer _whitePlayer;

        public GameSpecs()
        {
            _blackPlayer = new BlackPlayer("player1");
            _whitePlayer = new WhitePlayer("player2");
        }

        [Fact]
        public void GameState_Is_Set_To_WaitingForOpponent_When_New_Game_Is_Created()
        {
            var game = new Game(_blackPlayer);

            game.GameState.Should().Be(GameState.WaitingForOpponent);
        }

        [Fact]
        public void GameState_Is_Set_To_Playing_When_Opponent_Has_Joined()
        {
            var game = new Game(_blackPlayer);
            game.JoinOpponent(_whitePlayer);

            game.GameState.Should().Be(GameState.Playing);

        }

        [Fact]
        public void Cannot_Join_More_Than_One_Opponent()
        {
            var game = new Game(_blackPlayer);
            game.JoinOpponent(_whitePlayer);

            Action action = () => game.JoinOpponent(new WhitePlayer("player"));

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BlackPlayer_CanMakeMove_BeforeOpponentHasJoined()
        {
            var game = new Game(_blackPlayer);
            var result = game.PlayerMakesMove(_blackPlayer, new Position(4, 5));

            result.Should().NotBeNull();
        }

        [Fact]
        public void Player_Cannot_Make_Multiple_Moves_If_There_Is_A_Valid_Move_For_Opponent()
        {
            var game = new Game(_blackPlayer);
            game.PlayerMakesMove(_blackPlayer, new Position(4, 5));

            var result = game.PlayerMakesMove(_blackPlayer, new Position(2, 3));

            result.Should().BeNull();
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
            game.PlayerMakesMove(_blackPlayer, new Position(5, 0));
            
            var result = game.PlayerMakesMove(_blackPlayer, new Position(5, 2));

            result.Should().NotBeNull();
        }

        [Fact]
        public void GameIsFinished_If_No_Moves_Are_Left()
        {
            GameState gameState = null;
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
            game.JoinOpponent(_whitePlayer);
            game.GameStateChanged += (o, observerId, e) =>
            {
                gameState = e.CurrentState.GameState;
            };
            game.PlayerMakesMove(_blackPlayer, new Position(0, 3));

            gameState.Should().Be(GameState.Finished);
        }
    }
}
