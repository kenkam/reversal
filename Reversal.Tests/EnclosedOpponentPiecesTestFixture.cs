using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System.Collections.Generic;
using System.Linq;

namespace Reversal.Tests
{
    [TestFixture]
    public class EnclosedOpponentPiecesTestFixture
    {
        private IFixture fixture;
        private Mock<IPieceBag> pieceBagMock;
        private Mock<Direction> directionMock;
        private FakePiece startingPiece;

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            pieceBagMock = fixture.Freeze<Mock<IPieceBag>>();
            directionMock = fixture.Freeze<Mock<Direction>>();
            startingPiece = fixture.Build<FakePiece>()
                .With(x => x.Side, Side.Black)
                .Create();
        }

        [Test]
        public void HasEnclosedPieces_WhenEnclosureExists_ShouldReturnTrue()
        {
            // Arrange
            var pieces = fixture.Build<FakePiece>()
                .With(x => x.Side, Side.White)
                .CreateMany()
                .ToArray();
            var positions = new[] {startingPiece}.Concat(pieces)
                .Select(x => x.Position);

            var piecesToReturn = new Queue<IPiece>(pieces);
            foreach (var position in positions)
            {
                SetupPiece(position, piecesToReturn.Any() ? piecesToReturn.Dequeue() : null);
            }

            var sameSidePiece = fixture.Build<FakePiece>()
                .With(x => x.Side, startingPiece.Side)
                .Create();
            SetupPiece(pieces.Last().Position, sameSidePiece);

            var subject = fixture.Create<EnclosedOpponentPieces>();

            // Act
            var result = subject.HasEnclosedPieces(startingPiece, directionMock.Object);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasEnclosedPieces_WhenThereIsNoSameSidePieceEnclosingOpponents_ShouldReturnFalse()
        {
            // Arrange
            var pieces = fixture.Build<FakePiece>()
                .With(x => x.Side, Side.White)
                .CreateMany()
                .ToArray();
            var positions = new[] { startingPiece }.Concat(pieces)
                .Select(x => x.Position);

            var piecesToReturn = new Queue<IPiece>(pieces);
            foreach (var position in positions)
            {
                SetupPiece(position, piecesToReturn.Any() ? piecesToReturn.Dequeue() : null);
            }

            SetupPiece(pieces.Last().Position, null);

            var subject = fixture.Create<EnclosedOpponentPieces>();

            // Act
            var result = subject.HasEnclosedPieces(startingPiece, directionMock.Object);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasEnclosedPieces_WhenEnclosureDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var sameSidePiece = fixture.Build<FakePiece>()
                .With(x => x.Side, Side.Black)
                .Create();

            SetupPiece(startingPiece.Position, sameSidePiece);

            var subject = fixture.Create<EnclosedOpponentPieces>();

            // Act
            var result = subject.HasEnclosedPieces(startingPiece, directionMock.Object);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void FlipEnclosedPieces_WhenEnclosureExists_ShouldFlipEnclosedPieces()
        {
            // Arrange
            var pieces = fixture.Build<FakePiece>()
                .With(x => x.Side, Side.White)
                .CreateMany()
                .ToArray();
            var positions = new[] { startingPiece }.Concat(pieces)
                .Select(x => x.Position);

            var piecesToReturn = new Queue<IPiece>(pieces);
            foreach (var position in positions)
            {
                SetupPiece(position, piecesToReturn.Any() ? piecesToReturn.Dequeue() : null);
            }

            var sameSidePiece = fixture.Build<FakePiece>()
                .With(x => x.Side, startingPiece.Side)
                .Create();
            SetupPiece(pieces.Last()
                .Position, sameSidePiece);

            var subject = fixture.Create<EnclosedOpponentPieces>();

            // Act
            subject.FlipEnclosedPieces(startingPiece, directionMock.Object);

            // Assert
            Assert.That(pieces.All(x => x.Flipped), Is.True);
        }

        private void SetupPiece(Position position, IPiece piece)
        {
            var nextPosition = fixture.Create<Position>();
            directionMock.Setup(x => x.AwayFrom(position))
                .Returns(nextPosition);

            pieceBagMock.Setup(x => x.GetPiece(nextPosition))
                .Returns(() => piece);
        }

        internal class FakePiece : IPiece
        {
            public Position Position { get; set; }
            public Side Side { get; set; }
            public bool Flipped { get; private set; }

            public void Flip()
            {
                Flipped = true;
            }
        }
    }
}