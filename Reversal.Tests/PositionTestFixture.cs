using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Reversal.Tests
{
    [TestFixture]
    public class PositionTestFixture
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        [Test]
        public void X_WhenCalled_ShouldReturnX()
        {
            // Arrange
            var x = fixture.Create<int>();
            var subject = new Position(x, fixture.Create<int>());

            // Act
            var result = subject.X;

            // Assert
            Assert.That(result, Is.EqualTo(x));
        }

        [Test]
        public void Y_WhenCalled_ShouldReturnY()
        {
            // Arrange
            var y = fixture.Create<int>();
            var subject = new Position(fixture.Create<int>(), y);

            // Act
            var result = subject.Y;

            // Assert
            Assert.That(result, Is.EqualTo(y));
        }



        [Test]
        public void Equals_WhenMembersAreEqual_ShouldReturnTrue()
        {
            // Arrange
            var x = fixture.Create<int>();
            var y = fixture.Create<int>();
            var position1 = new Position(x, y);
            var position2 = new Position(x, y);

            // Act
            var result = position1.Equals(position2);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_WhenXIsDifferent_ShouldReturnFalse()
        {
            // Arrange
            var y = fixture.Create<int>();
            var position1 = new Position(0, y);
            var position2 = new Position(1, y);

            // Actd
            var result = position1.Equals(position2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_WhenYIsDifferent_ShouldReturnFalse()
        {
            // Arrange
            var x = fixture.Create<int>();
            var position1 = new Position(x, 0);
            var position2 = new Position(x, 1);

            // Act
            var result = position1.Equals(position2);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}