using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Reversio.Domain
{
    public class Board
    {
        /// <summary>
        /// The number of columns in the board
        /// </summary>
        public const int EdgeSize = 8;
        private readonly int[,] _positions;
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

        public Board() : this(new[,]
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
        })
        {
        }

        /// <summary>
        /// Use internally for setting a initial state
        /// </summary>
        /// <param name="positions">The current board state</param>
        internal Board(int[,] positions)
        {
            _positions = positions;
        }


        public int[,] CurrentState {
            get
            {
                var copy = new int[EdgeSize, EdgeSize];
                for (var i = 0; i < EdgeSize; i++)
                {
                    for (var j = 0; j < EdgeSize; j++)
                    {
                        copy[i, j] = _positions[i, j];
                    }
                }

                return copy;
            }
        }

        /// <summary>
        /// Tries to place a disc at a given position.
        /// </summary>
        /// <param name="move">The position and color of the move</param>
        /// <returns>True if the move was valid/successfull</returns>
        public IReadOnlyList<Position> TryDoMove(Move move)
        {
            var piecesToFlip = GetPiecesToFlipForMove(move);
            if (piecesToFlip.Count <= 0) return null;
            UpdateState(move, piecesToFlip);
            return piecesToFlip.ToList().AsReadOnly();
        }

        /// <summary>
        /// Checks if there is any valid moves for a given disc
        /// </summary>
        /// <param name="disc">The disc to check for</param>
        /// <returns>True if there exists any valid moves for the disc, false otherwise</returns>
        public bool HasMoves(Disc disc)
        {
            for (var i = 0; i < EdgeSize; i++)
            {
                for (var j = 0; j < EdgeSize; j++)
                {
                    if (IsMoveValid(new Move(i, j, disc)))
                        return true;
                }
            }

            return false;
        }

        private void UpdateState(Move move, IList<Position> piecesToFlip)
        {
            // Place the brick
            _positions[move.Position.Y, move.Position.X] = move.Disc.Color;
            // Switch color of all bricks that we are flipping
            foreach (var piece in piecesToFlip)
            {
                _positions[piece.Y, piece.X] *= -1;
            }
        }

        private bool IsMoveValid(Move move)
        {
            var piecesToFlip = GetPiecesToFlipForMove(move);
            return piecesToFlip.Any();
        }

        private IList<Position> GetPiecesToFlipForMove(Move move)
        {
            var positionsToFlipInMove = new List<Position>();

            if (!IsPositionEmpty(move.Position))
            {
                return positionsToFlipInMove;
            }

            for (var i = 0; i < EdgeSize; i++)
            {
                var xPos = move.Position.X;
                var yPos = move.Position.Y;
                var xStep = Directions[i, 0];
                var yStep = Directions[i, 1];
                var positionsToFlipInDirection = new Position[EdgeSize];

                for (var j = 0; j < EdgeSize; j++)
                {
                    xPos += xStep;
                    yPos -= yStep;
                    var position = new Position(xPos, yPos);

                    // Out of board
                    if (!IsPositionOnBoard(position) || IsPositionEmpty(position))
                    {
                        positionsToFlipInDirection = new Position[EdgeSize];
                        break;
                    }

                    // Our own color
                    if (_positions[yPos, xPos] == move.Disc.Color)
                    {
                        break;
                    }

                    positionsToFlipInDirection[j] = position;
                }

                for (var j = 0; j < EdgeSize; j++)
                {
                    if (positionsToFlipInDirection[j] != null)
                    {
                        positionsToFlipInMove.Add(positionsToFlipInDirection[j]);
                    }
                }
            }

            return positionsToFlipInMove;
        }

        private bool IsPositionEmpty(Position position)
        {
            return _positions[position.Y, position.X] == 0;
        }

        private static bool IsPositionOnBoard(Position position)
        {
            return position.X >= 0 && position.X < EdgeSize && position.Y >= 0 && position.Y < EdgeSize;
        }

        public override bool Equals(object obj)
        {
            var thatBoard = obj as Board;
            if (thatBoard == null) return false;
            for (var i = 0; i < EdgeSize; i++)
            {
                for (var j = 0; j < EdgeSize; j++)
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
            var sb = new StringBuilder();
            sb.AppendLine("| " + string.Join("", Enumerable.Repeat("---", EdgeSize)) + " |");
            for (var i = 0; i < charArr.GetLength(0); i++)
            {
                sb.Append("| ");
                for (var j = 0; j < charArr.GetLength(1); j++)
                {
                    sb.Append($" {charArr[i, j]} ");
                }
                sb.Append(" |");
                sb.AppendLine();
            }
            sb.AppendLine("| " + string.Join("", Enumerable.Repeat("---", EdgeSize)) + " |");
            return sb.ToString();
        }

        public static char[,] Translate(int[,] intArr)
        {
            var charArr = new char[intArr.GetLength(0), intArr.GetLength(1)];
            for (var i = 0; i < intArr.GetLength(0); i++)
            {
                for (var j = 0; j < intArr.GetLength(1); j++)
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
