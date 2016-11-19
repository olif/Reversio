using System;
using System.Diagnostics;
using Xunit;
using FluentAssertions;

namespace Reversio.Domain.UnitTest
{
    public class BoardSpecs
    {
        [Fact]
        public void Defaults_To_Initial_Positions()
        {
            var board = new Board();
            var expectedBoard = new Board(DefaultPositions);
            board.Should().Be(expectedBoard);
        }

        [Fact]
        public void Placing_Disc_On_Valid_Position_Updates_Board_Positions()
        {
            var board = new Board(DefaultPositions);
            var move1 = new Move(4, 5, Disc.Dark);
            var result = board.TryDoMove(move1);

            var expectedPositions = new char[8, 8]
            {
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', 'O', 'X', ' ', ' ', ' '},

                {' ', ' ', ' ', 'X', 'X', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', 'X', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            };
            var expectedBoard = new Board(expectedPositions.Translate());
            Assert.Equal(expectedBoard, board);
            result.Should().NotBeNull();
            board.Should().Be(expectedBoard);
        }

        [Fact]
        public void Placing_Disc_On_Invalid_Position_Returns_False()
        {
            var board = new Board(DefaultPositions);
            var invalidMove = new Move(4, 7, Disc.Dark);
            var moveResult =  board.TryDoMove(invalidMove);
            moveResult.Should().BeNull();
        }

        [Fact]
        public void HasMoves_Returns_True_If_The_Color_Has_Moves_To_Do()
        {
            var positions = new char[8, 8]
            {
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', 'X', ' ', 'X', ' ', ' ', ' '},

                {' ', ' ', 'X', 'O', 'X', ' ', ' ', ' '},

                {' ', ' ', 'X', 'X', 'X', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            };

            var board = new Board(positions.Translate());

            var hasMoves = board.HasMoves(Disc.Dark);
            hasMoves.Should().BeTrue();
        }

        [Fact]
        public void HasMoves_Returns_False_If_The_Color_Has_No_Moves_To_Do()
        {
            var positions = new char[8, 8]
            {
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', 'X', 'X', 'X', ' ', ' ', ' '},

                {' ', ' ', 'X', 'O', 'X', ' ', ' ', ' '},

                {' ', ' ', 'X', 'X', 'X', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            };

            var board = new Board(positions.Translate());

            var hasMoves = board.HasMoves(Disc.Dark);
            hasMoves.Should().BeFalse();
        }

        //[Fact]
        //public void Can_Make_Multiple_Moves_Of_Same_Color_If_Other_Color_Cannot_Move()
        //{
        //    var positions = new char[8, 8]
        //    {

        //        {' ', ' ', ' ', ' ', ' ', ' ', 'X', 'X'},

        //        {' ', ' ', ' ', ' ', ' ', ' ', 'O', 'X'},

        //        {' ', ' ', ' ', ' ', ' ', ' ', 'O', 'X'},

        //        {'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O'},

        //        {' ', ' ', ' ', ' ', ' ', ' ', 'O', ' '},

        //        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

        //        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

        //        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}
        //    };

        //    var board = new Board(positions.Translate());
        //    var move1 = new Move(5, 0, Disc.Dark);
        //    var move2 = new Move(5, 2, Disc.Dark);

        //    var result1 = board.TryDoMove(move1);
        //    var result2 = board.TryDoMove(move2);

        //    Debug.WriteLine(board);
        //    result1.Should().BeTrue();
        //    result2.Should().BeTrue();
        //}

        [Fact]
        public void TryDoMove_Sets_The_Board_In_Correct_State()
        {
            var board = new Board(DefaultPositions);
            var move1 = new Move(3, 2, Disc.Dark);
            var move2 = new Move(2, 4, Disc.Light);
            var move3 = new Move(3, 5, Disc.Dark);
            var move4 = new Move(4, 2, Disc.Light);
            var move5 = new Move(5, 2, Disc.Dark);
            var move6 = new Move(5, 3, Disc.Light);

            var moveResult1 = board.TryDoMove(move1);
            var moveResult2 = board.TryDoMove(move2);
            var moveResult3 = board.TryDoMove(move3);
            var moveResult4 = board.TryDoMove(move4);
            var moveResult5 = board.TryDoMove(move5);
            var moveResult6 = board.TryDoMove(move6);

            var expectedBoard = new Board(new char[8, 8]
            {
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', 'X', 'X', 'X', ' ', ' '},

                {' ', ' ', ' ', 'O', 'O', 'O', ' ', ' '},

                {' ', ' ', 'O', 'X', 'O', ' ', ' ', ' '},

                {' ', ' ', ' ', 'X', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            }.Translate());

            moveResult1.Should().NotBeNull();
            moveResult2.Should().NotBeNull();
            moveResult3.Should().NotBeNull();
            moveResult4.Should().NotBeNull();
            moveResult5.Should().NotBeNull();
            moveResult6.Should().NotBeNull();
            board.Should().Be(expectedBoard);
        }


        private static int[,] DefaultPositions
        {
            get
            {
                var initialPositions = new char[8, 8]
                {
                    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                    {' ', ' ', ' ', 'O', 'X', ' ', ' ', ' '},

                    {' ', ' ', ' ', 'X', 'O', ' ', ' ', ' '},

                    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                };
                return initialPositions.Translate();
            }
        }
    }

    public static class TestArrTranslator
    {

        public static int[,] Translate(this char[,] charArr)
        {
            int[,] arr = new int[charArr.GetLength(0), charArr.GetLength(1)];
            for (var i = 0; i < charArr.GetLength(0); i++)
            {
                for (var j = 0; j < charArr.GetLength(1); j++)
                {
                    switch (charArr[i,j])
                    {
                        case ' ':
                            arr[i, j] = 0;
                            break;
                        case 'X':
                            arr[i, j] = -1;
                            break;
                        case 'O':
                            arr[i, j] = 1;
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
            }
            return arr;
        }
    }
}
