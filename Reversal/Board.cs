using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversal
{
    public sealed class Board : IBoard
    {
        private readonly List<Piece> pieces;

        public Board(Position position, IEnumerable<Piece> pieces)
        {
            MaximumPosition = position;
            this.pieces = new List<Piece>(pieces);
        }

        public Position MaximumPosition { get; }

        public IEnumerable<Piece> GetPieces() => pieces;
        
        public Piece GetPiece(Position position)
        {
            return pieces.Find(x => x.Position.Equals(position));
        }

        public void Play(Piece piece)
        {
            if (!CanPlay(piece))
            {
                throw new InvalidOperationException($"Invalid move for {piece}");
            }

            pieces.Add(piece);
            foreach (var direction in Direction.All())
            {
                var line = new PiecesInLine(this, piece, direction);
                line.FlipOpponents();
            }
        }

        public bool CanPlay(Piece piece)
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

        private bool IsOccupied(Position position) => pieces.Any(x => x.Position.Equals(position));

        private bool IsOutOfBounds(Position position)
        {
            return position.X < 0 || position.X > MaximumPosition.X ||
                   position.Y < 0 || position.Y > MaximumPosition.Y;
        }

        private bool CapturesOpponentPiecesInAnyDirection(Piece piece)
        {
            return Direction.All()
                .Any(direction => new PiecesInLine(this, piece, direction).CapturesOpponentPieces());
        }
        
        private class PiecesInLine
        {
            private readonly Board board;
            private readonly Piece piece;
            private readonly Direction direction;

            public PiecesInLine(Board board, Piece piece, Direction direction)
            {
                this.board = board;
                this.piece = piece;
                this.direction = direction;
            }

            public bool CapturesOpponentPieces() => GetEnclosedContiguousOpponentPieces().Any();

            public void FlipOpponents()
            {
                foreach (var opponent in GetEnclosedContiguousOpponentPieces())
                {
                    opponent.Flip();
                }
            }

            private IEnumerable<Piece> GetEnclosedContiguousOpponentPieces()
            {
                var opponentPieces = GetContiguousOpponentPieces()
                    .ToArray();
                if (!opponentPieces.Any())
                {
                    return opponentPieces;
                }

                var nextPiece = GetNextPiece(opponentPieces.Last());
                if (nextPiece == null || nextPiece.Side != piece.Side)
                {
                    return Enumerable.Empty<Piece>();
                }

                return opponentPieces;
            }

            private IEnumerable<Piece> GetContiguousOpponentPieces()
            {
                var nextPiece = GetNextPiece(piece);
                while (nextPiece != null)
                {
                    if (nextPiece.Side == piece.Side)
                    {
                        yield break;
                    }

                    yield return nextPiece;
                    nextPiece = GetNextPiece(nextPiece);
                }
            }

            private Piece GetNextPiece(Piece thisPiece)
            {
                return board.GetPiece(direction.AwayFrom(thisPiece.Position));
            }
        }
    }
}