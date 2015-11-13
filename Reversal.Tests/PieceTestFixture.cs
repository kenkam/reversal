using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Reversal.Tests
{
    [TestFixture]
    public class PieceTestFixture
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        [Test]
        public void Position_WhenCalled_ShouldReturnPosition()
        {
            // Arrange
            var expectedPosition = fixture.Freeze<Position>();
            var subject = fixture.Create<Piece>();

            // Act
            var result = subject.Position;

            // Assert
            Assert.That(result, Is.EqualTo(expectedPosition));
        }

        [TestCase(Side.Black)]
        [TestCase(Side.White)]
        public void Side_WhenCalled_ShouldReturnSide(Side side)
        {
            // Arrange
            fixture.Inject(side);
            var subject = fixture.Create<Piece>();

            // Act
            var result = subject.Side;

            // Assert
            Assert.That(result, Is.EqualTo(side));
        }

        [TestCase(Side.Black, Side.White)]
        [TestCase(Side.White, Side.Black)]
        public void Flip_WhenCalled_ShouldChangeSides(Side start, Side expected)
        {
            // Arrange
            fixture.Inject(start);
            var subject = fixture.Create<Piece>();

            // Act
            subject.Flip();

            // Assert
            Assert.That(subject.Side, Is.EqualTo(expected));
        }

        [Test]
        public void Equals_WhenPositionAndSideAreEqual_ShouldReturnTrue()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var side = fixture.Create<Side>();
            var piece1 = new Piece(position, side);
            var piece2 = new Piece(position, side);

            // Act
            var result = piece1.Equals(piece2);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_WhenSideIsDifferent_ShouldReturnFalse()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var piece1 = new Piece(position, Side.Black);
            var piece2 = new Piece(position, Side.White);

            // Act
            var result = piece1.Equals(piece2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_WhenPositionIsDifferent_ShouldReturnFalse()
        {
            // Arrange
            var position1 = new Position(0, 1);
            var position2 = new Position(0, 2);
            var side = fixture.Create<Side>();
            var piece1 = new Piece(position1, side);
            var piece2 = new Piece(position2, side);

            // Act
            var result = piece1.Equals(piece2);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
