using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversal
{
    public sealed class Board : IBoard
    {
        private readonly IPieceBag pieceBag;
        private readonly IContiguousOpponentPieces contiguousOpponentPieces;

        public Board(
            Position maximum, 
            IPieceBag pieceBag,
            IContiguousOpponentPieces contiguousOpponentPieces)
        {
            MaximumPosition = maximum;
            this.pieceBag = pieceBag;
            this.contiguousOpponentPieces = contiguousOpponentPieces;
        }

        public Position MaximumPosition { get; }

        public IEnumerable<IPiece> GetPieces() => pieceBag;

        public IPiece GetPiece(Position position) => pieceBag.GetPiece(position);

        public void Play(IPiece piece)
        {
            if (!CanPlay(piece))
            {
                throw new InvalidOperationException($"Invalid move for {piece}");
            }

            pieceBag.Add(piece);
            foreach (var direction in Direction.All())
            {
                contiguousOpponentPieces.Capture(pieceBag, piece, direction);
            }
        }

        public bool CanPlay(IPiece piece)
        {
            if (IsOutOfBounds(piece.Position) || IsOccupied(piece.Position))
            {
                return false;
            }
            
            return CapturesOpponentPiecesInAnyDirection(piece);
        }

        public Side WinningSide
        {
            get
            {
                var blacks = Score(Side.Black);
                var whites = Score(Side.White);

                if (blacks > whites)
                {
                    return Side.Black;
                }
                if (whites > blacks)
                {
                    return Side.White;
                }
                return Side.None;
            }
        }

        private int Score(Side side) => GetPieces().Count(x => x.Side == side);

        private bool IsOccupied(Position position) => pieceBag.GetPiece(position) != null;

        private bool IsOutOfBounds(Position position)
        {
            return position.X < 0 || position.X > MaximumPosition.X ||
                   position.Y < 0 || position.Y > MaximumPosition.Y;
        }

        private bool CapturesOpponentPiecesInAnyDirection(IPiece piece)
        {
            return Direction.All()
                .Any(direction => contiguousOpponentPieces.HasCapturablePieces(pieceBag, piece, direction));
        }
    }
}