using System;

namespace Reversal
{
    class Program
    {
        static void Main(string[] args)
        {
            var pieces = new[]
            {
                new Piece(new Position(3, 4), Side.Black),
                new Piece(new Position(4, 3), Side.Black),
                new Piece(new Position(3, 3), Side.White),
                new Piece(new Position(4, 4), Side.White)
            };

            var board = new Board(new Position(7, 7), pieces);
            var view = new BoardView(board);
            view.Print();

            var piece = new Piece(new Position(2, 3), Side.Black);
            Console.WriteLine($"Playing {piece}");
            board.Play(piece);
            view.Print();
        }
    }
}
