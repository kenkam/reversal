using System.Collections.Generic;

namespace Reversal
{
    public interface IBoard
    {
        Position MaximumPosition { get; }
        IEnumerable<Piece> GetPieces();
        Piece GetPiece(Position position);
        void Play(Piece piece);
        bool CanPlay(Piece piece);
        Side WinningSide { get; }
    }
}