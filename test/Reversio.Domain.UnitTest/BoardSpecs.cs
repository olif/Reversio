using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace Reversio.Domain.UnitTest
{
    public class BoardSpecs
    {
        public BoardSpecs()
        {
      
        }

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
            var result = board.Place(move1);

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
            result.Should().BeTrue();
            board.Should().Be(expectedBoard);
        }

        [Fact]
        public void Placing_Disc_On_Invalid_Position_Returns_False()
        {
            var board = new Board(DefaultPositions);
            var invalidMove = new Move(4, 7, Disc.Dark);
            var moveResult =  board.Place(invalidMove);
            moveResult.Should().BeFalse();
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
