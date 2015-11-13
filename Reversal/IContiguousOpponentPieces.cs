namespace Reversal
{
    public interface IContiguousOpponentPieces
    {
        bool HasCapturablePieces(IPieceBag pieceBag, IPiece startingPiece, Direction direction);
        void Capture(IPieceBag pieceBag, IPiece startingPiece, Direction direction);
    }
}