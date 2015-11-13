using System;
using System.Linq;

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
            var pieceBag = new PieceBag(pieces);

            var board = new Board(new Position(7, 7), pieceBag, new EnclosedOpponentPiecesFactory());
            var game = new Game(board);
            var view = new BoardView(board);
            view.Print();

            while (true)
            {
                Console.WriteLine($"{game.Turn} turn: enter x,y");
                var input = Console.ReadLine();
                if (input == null)
                {
                    continue;
                }

                var tokens = input.Trim()
                    .Split(',')
                    .Select(x => x.Trim())
                    .Select(int.Parse)
                    .ToArray();
                var position = new Position(tokens.First(), tokens.Last());
                game.PlayPieceAt(position);

                view.Print();
            }
        }
    }
}
