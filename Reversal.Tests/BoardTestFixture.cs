using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Moq;

namespace Reversal.Tests
{
    [TestFixture]
    public class BoardTestFixture
    {
        [Test]
        public void GetPieces_WhenCalled_ShouldReturnPieceBag()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var pieceBag = fixture.Freeze<IPieceBag>();
            var subject = fixture.Create<Board>();

            // Act
            var result = subject.GetPieces();

            // Assert
            Assert.That(result, Is.SameAs(pieceBag));
        }

        [Test]
        public void GetPiece_WhenCalled_ShouldDelegateToPieceBag()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var position = fixture.Create<Position>();

            var pieceBagMock = fixture.Freeze<Mock<IPieceBag>>();
            var piece = fixture.Create<IPiece>();
            pieceBagMock.Setup(x => x.GetPiece(position))
                .Returns(piece);
            var subject = fixture.Create<Board>();

            // Act
            var result = subject.GetPiece(position);

            // Assert
            Assert.That(result, Is.SameAs(piece));
        }

        [Test]
        public void MaximumPosition_WhenCalled_ShouldReturnMaximumPosition()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var position = fixture.Freeze<Position>();
            var subject = fixture.Create<Board>();

            // Act
            var result = subject.MaximumPosition;

            // Assert
            Assert.That(result, Is.EqualTo(position));
        }

        [TestFixture]
        public class PlayTestFixture
        {
            private IFixture fixture;
            private Position maximum;
            private Mock<IPieceBag> pieceBagMock;
            private Mock<IContiguousOpponentPieces> contiguousOpponentPiecesMock;
            private IPiece occupyingPiece;
            private FakePiece piece;

            [SetUp]
            public void SetUp()
            {
                fixture = new Fixture()
                    .Customize(new AutoMoqCustomization());
                maximum = fixture.Create<Position>();
                pieceBagMock = fixture.Freeze<Mock<IPieceBag>>();
                var contiguousOpponentPiecesFactoryMock = fixture.Freeze<Mock<IContiguousOpponentPiecesFactory>>();

                piece = fixture.Build<FakePiece>()
                    .With(x => x.Position, new Position(maximum.X - 1, maximum.Y - 1))
                    .Create();

                occupyingPiece = null;
                pieceBagMock.Setup(x => x.GetPiece(piece.Position))
                    .Returns(() => occupyingPiece);

                contiguousOpponentPiecesMock = fixture.Create<Mock<IContiguousOpponentPieces>>();
                contiguousOpponentPiecesFactoryMock.Setup(x => x.Create(pieceBagMock.Object))
                    .Returns(() => contiguousOpponentPiecesMock.Object);

                fixture.Register(() => new Board(maximum, pieceBagMock.Object, contiguousOpponentPiecesFactoryMock.Object));
            }

            [TestCaseSource(typeof(Direction), nameof(Direction.All))]
            public void Play_WhenEnclosingOpponentPieces_ShouldFlipEnclosingOpponentPieces(Direction direction)
            {
                // Arrange
                var subject = fixture.Create<Board>();
                contiguousOpponentPiecesMock.Setup(
                    x => x.HasCapturablePieces(
                        piece,
                        It.Is<Direction>(r => r.GetType() == direction.GetType())))
                    .Returns(true);

                // Act
                subject.Play(piece);

                // Assert
                contiguousOpponentPiecesMock.Verify(
                    x => x.Capture(
                        piece, 
                        It.Is<Direction>(r => r.GetType() == direction.GetType())),
                    Times.Once);
            }

            [Test]
            public void Play_WhenEnclosingOpponentPiecesInMultipleDirections_ShouldFlipAllEnclosingOpponentPieces()
            {
                // Arrange
                var subject = fixture.Create<Board>();
                contiguousOpponentPiecesMock.Setup(
                    x => x.HasCapturablePieces(
                        piece,
                        It.IsAny<Direction>()))
                    .Returns(true);

                // Act
                subject.Play(piece);

                // Assert
                contiguousOpponentPiecesMock.Verify(
                    x => x.Capture(
                        piece,
                        It.IsAny<Direction>()),
                    Times.Exactly(Direction.All().Count()));
            }

            [Test]
            public void Play_WhenNotEnclosingOpponentPieces_ShouldThrowInvalidOperationException()
            {
                // Arrange
                contiguousOpponentPiecesMock.Setup(x => x.HasCapturablePieces(piece, It.IsAny<Direction>()))
                    .Returns(false);
                var subject = fixture.Create<Board>();

                // Act
                TestDelegate action = () => subject.Play(piece);

                // Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Test]
            public void CanPlay_WhenPositionIsOccupied_ShouldThrowInvalidOperationException()
            {
                // Arrange
                occupyingPiece = fixture.Create<IPiece>();
                var subject = fixture.Create<Board>();

                // Act
                TestDelegate action = () => subject.Play(piece);

                // Assert
                Assert.Throws<InvalidOperationException>(action);
            }

            [Test]
            public void CanPlay_WhenPositionIsOutOfBounds_ShouldThrowInvalidOperationException()
            {
                // Arrange
                var subject = fixture.Create<Board>();
                var outOfBounds = new Position(subject.MaximumPosition.X + 1, subject.MaximumPosition.Y + 1);
                piece = fixture.Build<FakePiece>()
                    .With(x => x.Position, outOfBounds)
                    .Create();

                // Act
                TestDelegate action = () => subject.Play(piece);

                // Assert
                Assert.Throws<InvalidOperationException>(action);
            }
        }

        [TestFixture]
        public class CanPlayTestFixture
        {
            private IFixture fixture;
            private Position maximum;
            private Mock<IPieceBag> pieceBagMock;
            private Mock<IContiguousOpponentPieces> contiguousOpponentPiecesMock;
            private IPiece occupyingPiece;
            private FakePiece piece;

            [SetUp]
            public void SetUp()
            {
                fixture = new Fixture()
                    .Customize(new AutoMoqCustomization());
                maximum = fixture.Create<Position>();
                pieceBagMock = fixture.Freeze<Mock<IPieceBag>>();
                var contiguousOpponentPiecesFactoryMock = fixture.Freeze<Mock<IContiguousOpponentPiecesFactory>>();

                piece = fixture.Build<FakePiece>()
                    .With(x => x.Position, new Position(maximum.X - 1, maximum.Y - 1))
                    .Create();

                occupyingPiece = null;
                pieceBagMock.Setup(x => x.GetPiece(piece.Position))
                    .Returns(() => occupyingPiece);

                contiguousOpponentPiecesMock = fixture.Create<Mock<IContiguousOpponentPieces>>();
                contiguousOpponentPiecesFactoryMock.Setup(x => x.Create(pieceBagMock.Object))
                    .Returns(() => contiguousOpponentPiecesMock.Object);

                fixture.Register(() => new Board(maximum, pieceBagMock.Object, contiguousOpponentPiecesFactoryMock.Object));
            }

            [TestCaseSource(typeof(Direction), nameof(Direction.All))]
            public void CanPlay_WhenEnclosingOpponentPieces_ShouldReturnTrue(Direction direction)
            {
                // Arrange
                var subject = fixture.Create<Board>();
                contiguousOpponentPiecesMock.Setup(
                    x => x.HasCapturablePieces(
                        piece, 
                        It.Is<Direction>(r => r.GetType() == direction.GetType())))
                    .Returns(true);

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.True);
            }

            [Test]
            public void CanPlay_WhenNotEnclosingOpponentPieces_ShouldReturnFalse()
            {
                // Arrange
                contiguousOpponentPiecesMock.Setup(x => x.HasCapturablePieces(piece, It.IsAny<Direction>()))
                    .Returns(false);
                var subject = fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void CanPlay_WhenPositionIsOccupied_ShouldReturnFalse()
            {
                // Arrange
                occupyingPiece = fixture.Create<IPiece>();
                var subject = fixture.Create<Board>();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.False);
            }

            [Test]
            public void CanPlay_WhenPositionIsOutOfBounds_ShouldReturnFalse()
            {
                // Arrange
                var subject = fixture.Create<Board>();
                var outOfBounds = new Position(subject.MaximumPosition.X + 1, subject.MaximumPosition.Y + 1);
                piece = fixture.Build<FakePiece>()
                    .With(x => x.Position, outOfBounds)
                    .Create();

                // Act
                var result = subject.CanPlay(piece);

                // Assert
                Assert.That(result, Is.False);
            }
        }
        
        [TestFixture]
        public class WinningSideTestFixture
        {
            private IFixture fixture;
            private Mock<IPieceBag> pieceBagMock;

            [SetUp]
            public void SetUp()
            {
                fixture = new Fixture()
                    .Customize(new AutoMoqCustomization());
                pieceBagMock = fixture.Freeze<Mock<IPieceBag>>();
            }

            [TestCase(Side.White)]
            [TestCase(Side.Black)]
            public void WinningSide_WhenCalled_ShouldReturnSideWithMostPieces(Side side)
            {
                // Arrange
                var piece = fixture.Build<FakePiece>()
                    .With(x => x.Side, side)
                    .Create();
                pieceBagMock.Setup(x => x.GetEnumerator())
                    .Returns(() => new List<IPiece> {piece}.GetEnumerator());

                var subject = fixture.Create<Board>();

                // Act
                var result = subject.WinningSide;

                // Assert
                Assert.That(result, Is.EqualTo(side));
            }

            [Test]
            public void WinningSide_WhenBothSidesHaveSamePieces_ShouldReturnSideNone()
            {
                // Arrange
                fixture.Register(() => Enumerable.Empty<IPiece>());
                var subject = fixture.Create<Board>();

                // Act
                var result = subject.WinningSide;

                // Assert
                Assert.That(result, Is.EqualTo(Side.None));
            }
        }

        [ExcludeFromCodeCoverage]
        internal class FakePiece : IPiece
        {
            public Position Position { get; set; }
            public Side Side { get; set; }
            public void Flip()
            {
                throw new NotImplementedException();
            }
        }
    }
}
