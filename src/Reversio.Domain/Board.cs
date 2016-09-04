using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reversio.Domain
{
    public class Board
    {
        public const int Width = 8;
        public const int Height = 8;
        private readonly int[,] _positions;
        public Disc ColorOfNextMode { get; private set; }

        private static readonly int[,] Directions = new int[8, 2]
        {
            //X  Y
            {0, -1},    // Up
            {1, -1},    // Up-right
            {1, 0},     // Right
            {1, 1},    // Down right
            {0, 1},     // Down
            {-1, 1},    // Down left
            {-1, 0},    // Left
            {-1, -1}    // Up-left
        };

        private static readonly int[,] InitialPositions = new int[Width, Height]
            {    
                //0  1  2  3  4  5  6  7
                { 0, 0, 0, 0, 0, 0, 0, 0},  //0  
                { 0, 0, 0, 0, 0, 0, 0, 0},  //1
                { 0, 0, 0, 0, 0, 0, 0, 0},  //2
                { 0, 0, 0, 1, -1, 0, 0, 0}, //3
                { 0, 0, 0, -1, 1, 0, 0, 0}, //4
                { 0, 0, 0, 0, 0, 0, 0, 0},  //5
                { 0, 0, 0, 0, 0, 0, 0, 0},  //6 
                { 0, 0, 0, 0, 0, 0, 0, 0},  //7
            };

        public Board() : this(InitialPositions)
        {
        }

        internal Board(int[,] positions)
        {
            _positions = positions;
            ColorOfNextMode = Disc.Dark;
        }

        public bool TryDoMove(Move move)
        {
            if (move.Color != ColorOfNextMode)
            {
                return false;
            }
            var piecesToFlip = GetPiecesToFlipForMove(move);
            return UpdateState(move, piecesToFlip);
        }

        private bool UpdateState(Move move, IList<Position> piecesToFlip)
        {
            var x = move.Position.X;
            var y = move.Position.Y;
            if (piecesToFlip.Any())
            {
                _positions[y, x] = (int) move.Color;
                FlipDiscs(piecesToFlip);
                if (move.Color == Disc.Dark)
                {
                    if (HasMoves(Disc.Light))
                        ToggleColor();
                }
                else if (move.Color == Disc.Light)
                {
                    if (HasMoves(Disc.Light))
                        ToggleColor();
                }
                return true;
            }

            return false;
        }

        private void ToggleColor()
        {
           if(ColorOfNextMode == Disc.Dark) ColorOfNextMode = Disc.Light;
           else if (ColorOfNextMode == Disc.Light) ColorOfNextMode = Disc.Dark;
        }

        private void FlipDiscs(IEnumerable<Position> positionsToFlipw)
        {
            foreach (var position in positionsToFlipw)
            {
                _positions[position.Y, position.X] *= -1;
            }
        }

        private bool IsMoveValid(Move move)
        {
            var piecesToFlip = GetPiecesToFlipForMove(move);
            return piecesToFlip.Any();
        }

        private IList<Position> GetPiecesToFlipForMove(Move move)
        {
            List<Position> positionsToFlipInMove = new List<Position>();

            if (!IsPositionEmpty(move.Position))
            {
                return positionsToFlipInMove;
            }

            for (var i = 0; i < Width; i++)
            {
                int xPos = move.Position.X;
                int yPos = move.Position.Y;
                int xStep = Directions[i, 0];
                int yStep = Directions[i, 1];
                var positionsToFlipInDirection = new Position[Height];

                for (var j = 0; j < Height; j++)
                {
                    xPos += xStep;
                    yPos -= yStep;
                    var position = new Position(xPos, yPos);

                    // Out of board
                    if (!IsPositionOnBoard(position) || IsPositionEmpty(position))
                    {
                        positionsToFlipInDirection = new Position[Height];
                        break;
                    }

                    // Our own color
                    if (_positions[yPos, xPos] == (int) move.Color)
                    {
                        break;
                    }

                    positionsToFlipInDirection[j] = position;
                }

                for (int j = 0; j < Height; j++)
                {
                    if (positionsToFlipInDirection[j] != null)
                    {
                        positionsToFlipInMove.Add(positionsToFlipInDirection[j]);
                    }
                }
            }

            return positionsToFlipInMove;
        }

        public bool HasMoves(Disc disc)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (IsMoveValid(new Move(i, j, disc)))
                        return true;
                }
            }

            return false;
        }

        private bool IsPositionEmpty(Position position)
        {
            return _positions[position.Y, position.X] == 0;
        }

        private static bool IsPositionOnBoard(Position position)
        {
            return position.X >= 0 && position.X < Width && position.Y >= 0 && position.Y < Height;
        }

        public override bool Equals(object obj)
        {
            var thatBoard = obj as Board;
            if (thatBoard == null) return false;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (_positions[i, j] != thatBoard._positions[i, j]) return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _positions.GetHashCode();
        }

        public override string ToString()
        {
            var charArr = Translate(_positions);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("| " + string.Join("", Enumerable.Repeat("---", Width)) + " |");
            for (int i = 0; i < charArr.GetLength(0); i++)
            {
                sb.Append("| ");
                for (int j = 0; j < charArr.GetLength(1); j++)
                {
                    sb.Append($" {charArr[i, j]} ");
                }
                sb.Append(" |");
                sb.Append("\r\n");
            }
            sb.AppendLine("| " + string.Join("", Enumerable.Repeat("---", Width)) + " |");

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
                            charArr[i, j] = ' ';
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
