using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Reversal.Tests
{
    [TestFixture]
    public class GameTestFixture
    {
        [Test]
        public void Game_WhenConstructing_ShouldInitializeTurn()
        {
            // Arrange
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            var expectedSide = Game.startingSide;

            // Act
            var subject = fixture.Create<Game>();

            // Assert
            Assert.That(subject.Turn, Is.EqualTo(expectedSide));
        }

        [TestFixture]
        public class PlayPieceAtTestFixture
        {
            private IFixture fixture;
            private Mock<IBoard> boardMock;
            private Position position;
            private bool canPlay;

            [SetUp]
            public void Setup()
            {
                fixture = new Fixture()
                    .Customize(new AutoMoqCustomization());

                position = fixture.Create<Position>();

                canPlay = true;
                boardMock = fixture.Freeze<Mock<IBoard>>();
                boardMock.Setup(x => x.CanPlay(
                    It.Is<Piece>(
                        r => r.Position.Equals(position))))
                    .Returns(() => canPlay);
            }

            [Test]
            public void PlayPieceAt_WhenIsValidMove_ShouldPlayPiece()
            {
                // Arrange
                canPlay = true;
                var subject = fixture.Create<Game>();
                var expectedTurn = subject.Turn;

                // Act
                subject.PlayPieceAt(position);

                // Assert
                boardMock.Verify(x => x.Play(
                    It.Is<Piece>(
                        r => r.Side == expectedTurn &&
                             r.Position.Equals(position))), 
                    Times.Once);
            }

            [Test]
            public void PlayPieceAt_WhenIsValidMove_ShouldChangeTurn()
            {
                // Arrange
                canPlay = true;
                var subject = fixture.Create<Game>();
                var currentTurn = subject.Turn;

                // Act
                subject.PlayPieceAt(position);

                // Assert
                Assert.That(subject.Turn, Is.Not.EqualTo(currentTurn));
            }

            [Test]
            public void PlayPieceAt_WhenInvalidMove_ShouldNotPlayPiece()
            {
                // Arrange
                canPlay = false;
                var subject = fixture.Create<Game>();

                // Act
                subject.PlayPieceAt(position);

                // Assert
                boardMock.Verify(x => x.Play(It.IsAny<Piece>()), Times.Never);
            }

            [Test]
            public void PlayPieceAt_WhenInvalidMove_ShouldNotChangeTurn()
            {
                // Arrange
                canPlay = false;
                var subject = fixture.Create<Game>();
                var currentTurn = subject.Turn;

                // Act
                subject.PlayPieceAt(position);

                // Assert
                Assert.That(subject.Turn, Is.EqualTo(currentTurn));
            }
        }
    }
}