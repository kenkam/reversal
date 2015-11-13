namespace Reversal
{
    public interface IContiguousOpponentPieces
    {
        bool HasCapturablePieces(IPiece startingPiece, Direction direction);
        void Capture(IPiece startingPiece, Direction direction);
    }
}