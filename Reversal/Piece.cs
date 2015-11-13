namespace Reversal
{
    public sealed class Piece
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

        private bool Equals(Piece other)
        {
            return Position.Equals(other.Position) && 
                   Side == other.Side;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Piece && Equals((Piece) obj);
        }

        public override int GetHashCode()
        {
            unchecked { return Position.GetHashCode()*397; }
        }
    }
}