using System.Diagnostics.CodeAnalysis;

namespace Reversal
{
    public sealed class Piece : IPiece
    {
        public Piece(Position position, Side side)
        {
            Position = position;
            Side = side;
        }

        public Position Position { get; }
        public Side Side { get; private set; }

        public void Flip() => Side = Side == Side.Black ? Side.White : Side.Black;

        public override string ToString()
        {
            return $"{Side} at {Position}";
        }
    }
}