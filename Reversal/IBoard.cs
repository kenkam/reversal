using System.Collections.Generic;

namespace Reversal
{
    public interface IBoard
    {
        Position MaximumPosition { get; }
        IEnumerable<IPiece> GetPieces();
        IPiece GetPiece(Position position);
        void Play(IPiece piece);
        bool CanPlay(IPiece piece);
        int Score(Side side);
    }
}