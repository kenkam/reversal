namespace Reversal
{
    internal sealed class ContiguousOpponentPiecesFactory : IContiguousOpponentPiecesFactory
    {
        public IContiguousOpponentPieces Create(IPieceBag pieceBag)
        {
            return new ContiguousOpponentPieces(pieceBag);
        }
    }
}