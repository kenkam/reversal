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
        private static IFixture _fixture = new Fixture()
            .Customize(new AutoMoqCustomization());
        
        [Test]
        public void GetPieces_WhenCalled_ShouldReturnPieces()
        {
            // Arrange
            var pieces = _fixture.Freeze<IEnumerable<Piece>>();
            var subject = _fixture.Create<Board>();;

            // Act
            var result = subject.GetPieces();

            // Assert
            Assert.That(result, Is.EquivalentTo(pieces));
        }

        [Test]
        public void GetPiece_WhenPieceExists_ShouldReturnPiece()
        {
            // Arrange
            var pieces = _fixture.Freeze<IEnumerable<Piece>>()
                .ToArray();
            var subject = _fixture.Create<Board>();

            // Act
            var result = subject.GetPiece(pieces.First().Position);

            // Assert
            Assert.That(result, Is.SameAs(pieces.First()));
        }

        [Test]
        public void MaximumPosition_WhenCalled_ShouldReturnMaximumPosition()
        {
            // Arrange
            var position = _fixture.Freeze<Position>();
            var subject = _fixture.Create<Board>();;

            // Act
            var result = subject.MaximumPosition;

            // Assert
            Assert.That(result, Is.EqualTo(position));
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
                _fixture.Register<IEnumerable<Piece>>(() => new[] { white, black });

                var piece = new Piece(new Position(x, y), side);
                var subject = _fixture.Create<Board>();

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
                _fixture.Register<IEnumerable<Piece>>(() => new[] { white, black });

                var piece = new Piece(new Position(x, y), side);
                var subject = _fixture.Create<Board>();

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
                _fixture.Register<IEnumerable<Piece>>(() => new[] { white, black });

                var piece = new Piece(new Position(x, y), side);
                var subject = _fixture.Create<Board>();

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
                _fixture.Register<IEnumerable<Piece>>(() => new[] { white });

                var piece = new Piece(new Position(4, 5), Side.White);
                var subject = _fixture.Create<Board>();

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
                _fixture.Register<IEnumerable<Piece>>(() => new[] { white });

                var piece = new Piece(new Position(3, 6), Side.White);
                var subject = _fixture.Create<Board>();

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
                _fixture.Register<IEnumerable<Piece>>(() => new[] { white });

                var piece = new Piece(new Position(4, 6), Side.White);
                var subject = _fixture.Create<Board>();

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
                _fixture.Register(() => maximum);
                var piece = new Piece(new Position(x, y), Side.White);
                var subject = _fixture.Create<Board>();

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
                var subject = _fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.False);
            }
        }
    }
}
