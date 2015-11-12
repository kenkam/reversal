using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversal
{
    public sealed class BoardView
    {
        private readonly Board board;

        public BoardView(Board board)
        {
            this.board = board;
        }

        public void Print()
        {
            var maximum = board.MaximumPosition;
            for (var y = maximum.Y; y >= 0; --y)
            {
                foreach (var position in GetRow(y))
                {
                    var piece = board.GetPiece(position);
                    var letter = piece == null
                        ? "." 
                        : piece.Side == Side.Black
                            ? "B"
                            : "S";
                    Console.Write(letter);
                }
                Console.WriteLine();
            }
        }

        private IEnumerable<Position> GetRow(int row)
        {
            var maximum = board.MaximumPosition;
            for (var x = 0; x <= maximum.X; ++x)
            {
                yield return new Position(x, row);
            }
        }
    }
}