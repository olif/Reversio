using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Reversio.Domain.UnitTest
{
    public class BoardSpecs
    {
        public BoardSpecs()
        {
      
        }

        [Fact]
        public void Test()
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

            var board = new Board(initialPositions.Translate());
            var move1 = new Move(4, 5, Disc.Dark);
            board.Place(move1);

        ;

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

            Debug.WriteLine(board);
            Assert.Equal(expectedBoard, board);
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
