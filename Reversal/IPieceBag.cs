using System.Collections.Generic;
using System.ComponentModel;

namespace Reversal
{
    public interface IPieceBag : IEnumerable<IPiece>
    {
        void Add(IPiece piece);
        IPiece GetPiece(Position position);
    }
}