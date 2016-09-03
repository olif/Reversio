using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversio.Domain
{
    public class Board
    {
        private readonly int[,] _positions;

        private readonly int[,] _directions = new int[8, 2]
        {
            {0, -1}, // Up
            {1, -1}, // Up-right
            {1, 0}, // Right
            {1, -1}, // Down right
            {0, 1}, // Down
            {-1, 1}, // Down left
            {-1, 0}, // Left
            {-1, 1} // Up-left
        };

        public Board()
        {
            _positions = new int[8, 8];
        }

        internal Board(int[,] positions)
        {
            _positions = positions;
        }

        public void Place(Move disc)
        {
            var x = disc.Position.X;
            var y = disc.Position.Y;
            var color = disc.Color;
            var piecesToFlip = PiecesToFlip(x, y, color);
            if (piecesToFlip.Count > 0)
            {
                _positions[y, x] = (int)disc.Color;
                FlipDiscs(piecesToFlip);
            }
        }

        private void FlipDiscs(IEnumerable<Position> piecesToFlip)
        {
            foreach (var position in piecesToFlip)
            {
                _positions[position.Y, position.X] *= -1;
            }
        }

        private IList<Position> PiecesToFlip(int x, int y, Disc color)
        {
            List<Position> positionsToFlipInMove = new List<Position>();
            for (var i = 0; i < 8; i++)
            {
                int xPos = x;
                int yPos = y;
                int xStep = _directions[i, 0];
                int yStep = _directions[i, 1];
                var positionsToFlipInDirection = new Position[8];

                for (var j = 0; j < 8; j++)
                {
                    xPos += xStep;
                    yPos += yStep;

                    // Out of board
                    if (!IsOnBoard(xPos, yPos) || IsEmpty(xPos, yPos))
                    {
                        positionsToFlipInDirection = new Position[8];
                        break;
                    }

                    // Our own color
                    if (_positions[xPos, yPos] == (int) color)
                    {
                        break;
                    }

                    positionsToFlipInDirection[j] = new Position(xPos, yPos);
                }

                for (int j = 0; j < 8; j++)
                {
                    if (positionsToFlipInDirection[j] != null)
                    {
                        positionsToFlipInMove.Add(positionsToFlipInDirection[j]);
                    }
                }
            }

            return positionsToFlipInMove;
        }

        private bool IsEmpty(int xPos, int yPos)
        {
            return _positions[xPos, yPos] == 0;
        }

        private static bool IsOnBoard(int xPos, int yPos)
        {
            return xPos >= 0 && xPos < 8 && yPos >= 0 && yPos < 8;
        }

        public override bool Equals(object obj)
        {
            var thatBoard = obj as Board;
            if (thatBoard == null) return false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_positions[i, j] != thatBoard._positions[i, j]) return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            var charArr = Translate(_positions);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charArr.GetLength(0); i++)
            {
                for (int j = 0; j < charArr.GetLength(1); j++)
                {
                    sb.Append(charArr[i, j]);
                }
                sb.Append("\r\n");
            }

            return sb.ToString();
        }

        public static char[,] Translate(int[,] intArr)
        {
            char[,] charArr = new char[intArr.GetLength(0), intArr.GetLength(1)];
            for (int i = 0; i < intArr.GetLength(0); i++)
            {
                for (int j = 0; j < intArr.GetLength(1); j++)
                {
                    switch (intArr[i, j])
                    {
                        case 0:
                            charArr[i, j] = '-';
                            break;
                        case -1:
                            charArr[i, j] = 'X';
                            break;
                        case 1:
                            charArr[i, j] = 'O';
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
            }

            return charArr;
        }
    }
}
