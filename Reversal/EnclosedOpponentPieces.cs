using System.Collections.Generic;
using System.Linq;

namespace Reversal
{
    internal sealed class EnclosedOpponentPieces : IEnclosedOpponentPieces
    {
        private readonly IPieceBag pieceBag;

        public EnclosedOpponentPieces(IPieceBag pieceBag)
        {
            this.pieceBag = pieceBag;
        }

        public bool HasEnclosedPieces(IPiece startingPiece, Direction direction)
        {
            return GetEnclosedPieces(startingPiece, direction).Any();
        }

        public void FlipEnclosedPieces(IPiece startingPiece, Direction direction)
        {
            foreach (var piece in GetEnclosedPieces(startingPiece, direction))
            {
                piece.Flip();
            }
        }

        private IEnumerable<IPiece> GetEnclosedPieces(IPiece startingPiece, Direction direction)
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