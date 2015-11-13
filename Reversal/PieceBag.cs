using System.Collections;
using System.Collections.Generic;

namespace Reversal
{
    internal sealed class PieceBag : IPieceBag
    {
        private readonly List<IPiece> pieces;

        public PieceBag(IEnumerable<IPiece> pieces)
        {
            this.pieces = new List<IPiece>(pieces);
        }

        public void Add(IPiece piece)
        {
            pieces.Add(piece);
        }

        public IEnumerator<IPiece> GetEnumerator()
        {
            return pieces.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IPiece GetPiece(Position position)
        {
            return pieces.Find(x => x.Position.Equals(position));
        }
    }
}