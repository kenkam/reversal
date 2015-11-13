namespace Reversal
{
    internal sealed class EnclosedOpponentPiecesFactory : IEnclosedOpponentPiecesFactory
    {
        public IEnclosedOpponentPieces Create(IPieceBag pieceBag)
        {
            return new EnclosedOpponentPieces(pieceBag);
        }
    }
}