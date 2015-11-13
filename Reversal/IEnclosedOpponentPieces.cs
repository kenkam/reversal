namespace Reversal
{
    public interface IEnclosedOpponentPieces
    {
        bool HasEnclosedPieces(IPiece startingPiece, Direction direction);
        void FlipEnclosedPieces(IPiece startingPiece, Direction direction);
    }
}