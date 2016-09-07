using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;

namespace Reversio.Domain.UnitTest
{
    public class GameSpecs
    {
        private User _blackPlayer;
        private User _whitePlayer;

        public GameSpecs()
        {
            _blackPlayer = new User();
            _whitePlayer = new User();
        }

        [Fact]
        public void Cannot_Join_More_Than_One_Opponent()
        {
            var game = new Game(_blackPlayer);
            game.JoinOpponent(_whitePlayer);

            Action action = () => game.JoinOpponent(new User());

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BlackPlayer_CanMakeMove_BeforeOpponentHasJoined()
        {
            var game = new Game(_blackPlayer);

            var result = game.UserMakesMove(_blackPlayer, new Position(4, 5));

            result.Should().BeTrue();
        }

        [Fact]
        public void Player_Cannot_Make_Multiple_Moves_If_There_Is_A_Valid_Move_For_Opponent()
        {
            var game = new Game(_blackPlayer);
            game.UserMakesMove(_blackPlayer, new Position(4, 5));

            var result = game.UserMakesMove(_blackPlayer, new Position(2, 3));

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
            game.UserMakesMove(_blackPlayer, new Position(5, 0));
            
            var result = game.UserMakesMove(_blackPlayer, new Position(5, 2));

            result.Should().BeTrue();
        }

        [Fact]
        public void GameIsFinished_If_No_Moves_Are_Left()
        {
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
            game.UserMakesMove(_blackPlayer, new Position(0, 3));

            Debug.WriteLine(game.Board);
            game.GameFinished.Should().BeTrue();

        }

        //[Fact]
        //public void Can_Place_Brick()
        //{
        //    var user1 = new User();
        //    var game = new Game(user1);
        //    var user2 = new User();

        //    game.JoinOpponent(user2);

        //    var position = new Position(1, 2);
        //    game.UserMakesMove(user1, position);
        //}
    }
}
