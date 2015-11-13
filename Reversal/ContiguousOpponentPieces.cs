using System.Collections.Generic;
using System.Linq;

namespace Reversal
{
    internal sealed class ContiguousOpponentPieces : IContiguousOpponentPieces
    {
        private readonly IPieceBag pieceBag;

        public ContiguousOpponentPieces(IPieceBag pieceBag)
        {
            this.pieceBag = pieceBag;
        }

        public bool HasCapturablePieces(IPiece startingPiece, Direction direction)
        {
            return GetCapturablePieces(startingPiece, direction).Any();
        }

        public void Capture(IPiece startingPiece, Direction direction)
        {
            foreach (var piece in GetCapturablePieces(startingPiece, direction))
            {
                piece.Flip();
            }
        }

        private IEnumerable<IPiece> GetCapturablePieces(IPiece startingPiece, Direction direction)
        {
            var opponents = GetContiguousOpponentPieces(startingPiece, direction)
                .ToArray();
            if (!opponents.Any())
            {
                return Enumerable.Empty<IPiece>();
            }

            var nextPiece = GetNextPieceFrom(opponents.Last(), direction);
            if (nextPiece == null || nextPiece.Side != startingPiece.Side)
            {
                return Enumerable.Empty<IPiece>();
            }

            return opponents;
        } 

        private IEnumerable<IPiece> GetContiguousOpponentPieces(IPiece startingPiece, Direction direction)
        {
            var piece = GetNextPieceFrom(startingPiece, direction);
            while (piece != null)
            {
                if (piece.Side == startingPiece.Side)
                {
                    yield break;
                }

                yield return piece;
                piece = GetNextPieceFrom(piece, direction);
            }
        }

        private IPiece GetNextPieceFrom(IPiece piece, Direction direction)
        {
            return pieceBag.GetPiece(direction.AwayFrom(piece.Position));
        }
    }
}