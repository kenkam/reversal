using System.Collections.Generic;
using System.Linq;

namespace Reversal
{
    internal sealed class ContiguousOpponentPieces : IContiguousOpponentPieces
    {
        public bool HasCapturablePieces(IPieceBag pieceBag, IPiece startingPiece, Direction direction)
        {
            return GetCapturablePieces(pieceBag, startingPiece, direction).Any();
        }

        public void Capture(IPieceBag pieceBag, IPiece startingPiece, Direction direction)
        {
            foreach (var piece in GetCapturablePieces(pieceBag, startingPiece, direction))
            {
                piece.Flip();
            }
        }

        private IEnumerable<IPiece> GetCapturablePieces(IPieceBag pieceBag, IPiece startingPiece, Direction direction)
        {
            var opponents = GetContiguousOpponentPieces(pieceBag, startingPiece, direction)
                .ToArray();
            if (!opponents.Any())
            {
                return Enumerable.Empty<IPiece>();
            }

            var nextPiece = GetNextPieceFrom(pieceBag, opponents.Last(), direction);
            if (nextPiece == null || nextPiece.Side != startingPiece.Side)
            {
                return Enumerable.Empty<IPiece>();
            }

            return opponents;
        } 

        private IEnumerable<IPiece> GetContiguousOpponentPieces(IPieceBag pieceBag, IPiece startingPiece, Direction direction)
        {
            var piece = GetNextPieceFrom(pieceBag, startingPiece, direction);
            while (piece != null)
            {
                if (piece.Side == startingPiece.Side)
                {
                    yield break;
                }

                yield return piece;
                piece = GetNextPieceFrom(pieceBag, piece, direction);
            }
        }

        private IPiece GetNextPieceFrom(IPieceBag pieceBag, IPiece piece, Direction direction)
        {
            return pieceBag.GetPiece(direction.AwayFrom(piece.Position));
        }
    }
}