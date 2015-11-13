using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Reversal.Tests
{
    [TestFixture]
    public class BoardTestFixture
    {
        private static readonly IFixture Fixture = new Fixture()
            .Customize(new AutoMoqCustomization());
        
        [Test]
        public void GetPieces_WhenCalled_ShouldReturnPieces()
        {
            // Arrange
            var pieces = Fixture.Freeze<IEnumerable<Piece>>();
            var subject = Fixture.Create<Board>();

            // Act
            var result = subject.GetPieces();

            // Assert
            Assert.That(result, Is.EquivalentTo(pieces));
        }

        [Test]
        public void GetPiece_WhenPieceExists_ShouldReturnPiece()
        {
            // Arrange
            var pieces = Fixture.Freeze<IEnumerable<Piece>>()
                .ToArray();
            var subject = Fixture.Create<Board>();

            // Act
            var result = subject.GetPiece(pieces.First().Position);

            // Assert
            Assert.That(result, Is.SameAs(pieces.First()));
        }

        [Test]
        public void MaximumPosition_WhenCalled_ShouldReturnMaximumPosition()
        {
            // Arrange
            var position = Fixture.Freeze<Position>();
            var subject = Fixture.Create<Board>();

            // Act
            var result = subject.MaximumPosition;

            // Assert
            Assert.That(result, Is.EqualTo(position));
        }

        [TestFixture]
        public class PlayTestFixture
        {
            [Test]
            public void Play_WhenInvalidMove_ShouldThrowInvalidOperationException()
            {
                // Arrange
                var white = new Piece(new Position(3, 5), Side.White);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white });

                var piece = new Piece(new Position(4, 5), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                TestDelegate action = () => subject.Play(piece);

                // Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Test]
            public void Play_WhenCanCaptureHorizontally_ShouldFlipAllOpponentPieces()
            {
                // Arrange
                var white = new Piece(new Position(3, 5), Side.White);
                var opponent = new Piece(new Position(4, 5), Side.Black);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white, opponent });

                var piece = new Piece(new Position(5, 5), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                subject.Play(piece);

                // Assert
                Assert.That(opponent.Side, Is.EqualTo(Side.White));
            }

            [Test]
            public void Play_WhenCanCaptureVertically_ShouldFlipAllOpponentPieces()
            {
                // Arrange
                var white = new Piece(new Position(3, 5), Side.White);
                var opponent = new Piece(new Position(3, 4), Side.Black);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white, opponent });

                var piece = new Piece(new Position(3, 3), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                subject.Play(piece);

                // Assert
                Assert.That(opponent.Side, Is.EqualTo(Side.White));
            }

            [Test]
            public void Play_WhenCanCaptureDiagonally_ShouldFlipAllOpponentPieces()
            {
                // Arrange
                var white = new Piece(new Position(3, 5), Side.White);
                var opponent = new Piece(new Position(4, 4), Side.Black);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white, opponent });

                var piece = new Piece(new Position(3, 3), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                subject.Play(piece);

                // Assert
                Assert.That(opponent.Side, Is.EqualTo(Side.White));
            }

            [Test]
            public void Play_WhenHasMultiplePiecesInALine_ShouldFlipOnlyContiguousOpponentPieces()
            {
                // Arrange
                var opponent1 = new Piece(new Position(2, 1), Side.Black);
                var opponent2 = new Piece(new Position(3, 1), Side.Black);
                var white1 = new Piece(new Position(4, 1), Side.White);
                var safeOpponent = new Piece(new Position(5, 1), Side.Black);
                var white2 = new Piece(new Position(6, 1), Side.White);
                Fixture.Register<IEnumerable<Piece>>(() => new[]
                {
                    white1,
                    opponent1,
                    opponent2,
                    safeOpponent,
                    white2
                });

                var expectedFlippedPieces = new[] {opponent1, opponent2};

                var piece = new Piece(new Position(1, 1), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                subject.Play(piece);

                // Assert
                Assert.That(expectedFlippedPieces.All(x => x.Side == Side.White), Is.True);
                Assert.That(safeOpponent.Side, Is.EqualTo(Side.Black));
            }
        }

        [TestFixture]
        public class CanPlayTestFixture
        {
            [TestCase(5, 5, Side.White)]
            [TestCase(2, 5, Side.Black)]
            public void CanPlay_WhenCanCaptureHorizontally_ShouldReturnTrue(int x, int y, Side side)
            {
                // Arrange
                var white = new Piece(new Position(3, 5), Side.White);
                var black = new Piece(new Position(4, 5), Side.Black);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white, black });

                var piece = new Piece(new Position(x, y), side);
                var subject = Fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.True);
            }

            [TestCase(5, 5, Side.White)]
            [TestCase(5, 2, Side.Black)]
            public void CanPlay_WhenCanCaptureVertically_ShouldReturnTrue(int x, int y, Side side)
            {
                // Arrange
                var white = new Piece(new Position(5, 3), Side.White);
                var black = new Piece(new Position(5, 4), Side.Black);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white, black });

                var piece = new Piece(new Position(x, y), side);
                var subject = Fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.True);
            }

            [TestCase(5, 5, Side.White)]
            [TestCase(2, 2, Side.Black)]
            public void CanPlay_WhenCanCaptureDiagonally_ShouldReturnTrue(int x, int y, Side side)
            {
                // Arrange
                var white = new Piece(new Position(3, 3), Side.White);
                var black = new Piece(new Position(4, 4), Side.Black);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white, black });

                var piece = new Piece(new Position(x, y), side);
                var subject = Fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void CanPlay_WhenCannotCaptureHorizontally_ShouldReturnFalse()
            {
                // Arrange
                var white = new Piece(new Position(3, 5), Side.White);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white });

                var piece = new Piece(new Position(4, 5), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void CanPlay_WhenCannotCaptureVertically_ShouldReturnFalse()
            {
                // Arrange
                var white = new Piece(new Position(3, 5), Side.White);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white });

                var piece = new Piece(new Position(3, 6), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void CanPlay_WhenCannotCaptureDiagonally_ShouldReturnFalse()
            {
                // Arrange
                var white = new Piece(new Position(3, 5), Side.White);
                Fixture.Register<IEnumerable<Piece>>(() => new[] { white });

                var piece = new Piece(new Position(4, 6), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.False);
            }

            [TestCase(8, 7)]
            [TestCase(7, 8)]
            [TestCase(8, 8)]
            public void CanPlay_WhenPieceIsOutsideBoundaryMaximum_ShouldReturnFalse(int x, int y)
            {
                // Arrange
                var maximum = new Position(7, 7);
                Fixture.Register(() => maximum);
                var piece = new Piece(new Position(x, y), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.False);
            }

            [TestCase(0, -1)]
            [TestCase(-1, 0)]
            [TestCase(-1, -1)]
            public void CanPlay_WhenPieceIsOutsideBoundaryMinimum_ShouldReturnFalse(int x, int y)
            {
                // Arrange
                var piece = new Piece(new Position(x, y), Side.White);
                var subject = Fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.False);
            }
        }
    }
}
