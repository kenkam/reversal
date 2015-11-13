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
    }
}
