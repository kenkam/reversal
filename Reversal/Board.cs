﻿using System.Collections.Generic;
using System.Linq;

namespace Reversal
{
    public sealed class Board
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

        public bool CanPlay(Piece piece)
        {
            if (IsOutOfBounds(piece.Position))
            {
                return false;
            }

            return Direction.All()
                .Any(direction => new PiecesInLine(this, piece, direction).IsValid());
        }

        private bool IsOutOfBounds(Position position)
        {
            return position.X < 0 || position.X > MaximumPosition.X ||
                   position.Y < 0 || position.Y > MaximumPosition.Y;
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

            public bool IsValid() => GetContiguousOppositionPieces().Any();

            private IEnumerable<Piece> GetContiguousOppositionPieces()
            {
                var side = piece.Side;
                foreach (var next in GetContiguousPieces())
                {
                    if (next.Side != side)
                    {
                        yield return next;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }

            private IEnumerable<Piece> GetContiguousPieces()
            {
                var nextPosition = direction.AwayFrom(piece.Position);
                while (!board.IsOutOfBounds(nextPosition))
                {
                    var nextPiece = board.GetPiece(nextPosition);
                    if (nextPiece == null)
                    {
                        yield break;
                    }

                    yield return nextPiece;
                    nextPosition = direction.AwayFrom(nextPosition);
                }
            }
        }
    }
}