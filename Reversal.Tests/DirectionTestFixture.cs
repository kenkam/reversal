using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Reversal.Tests
{
    [TestFixture]
    public class DirectionTestFixture
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        [Test]
        public void All_WhenCalled_ShouldReturnAllDirections()
        {
            // Arrange
            // Act
            var result = Direction.All().ToArray();

            // Assert
            Assert.That(result.OfType<Direction.North>().Count(), Is.EqualTo(1));
            Assert.That(result.OfType<Direction.NorthEast>().Count(), Is.EqualTo(1));
            Assert.That(result.OfType<Direction.East>().Count(), Is.EqualTo(1));
            Assert.That(result.OfType<Direction.SouthEast>().Count(), Is.EqualTo(1));
            Assert.That(result.OfType<Direction.South>().Count(), Is.EqualTo(1));
            Assert.That(result.OfType<Direction.SouthWest>().Count(), Is.EqualTo(1));
            Assert.That(result.OfType<Direction.West>().Count(), Is.EqualTo(1));
            Assert.That(result.OfType<Direction.NorthWest>().Count(), Is.EqualTo(1));
            Assert.That(result.Length, Is.EqualTo(8));
        }

        [Test]
        public void North_WhenCallingAwayFrom_ShouldReturnNorthPosition()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var expectedPosition = new Position(position.X, position.Y + 1);
            var subject = fixture.Create<Direction.North>();

            // Act
            var result = subject.AwayFrom(position);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void NorthEast_WhenCallingAwayFrom_ShouldReturnNorthEastPosition()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var expectedPosition = new Position(position.X + 1, position.Y + 1);
            var subject = fixture.Create<Direction.NorthEast>();

            // Act
            var result = subject.AwayFrom(position);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void East_WhenCallingAwayFrom_ShouldReturnEastPosition()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var expectedPosition = new Position(position.X + 1, position.Y);
            var subject = fixture.Create<Direction.East>();

            // Act
            var result = subject.AwayFrom(position);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void SouthEast_WhenCallingAwayFrom_ShouldReturnSouthEastPosition()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var expectedPosition = new Position(position.X + 1, position.Y - 1);
            var subject = fixture.Create<Direction.SouthEast>();

            // Act
            var result = subject.AwayFrom(position);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void South_WhenCallingAwayFrom_ShouldReturnSouthPosition()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var expectedPosition = new Position(position.X, position.Y - 1);
            var subject = fixture.Create<Direction.South>();

            // Act
            var result = subject.AwayFrom(position);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void SouthWest_WhenCallingAwayFrom_ShouldReturnSouthWestPosition()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var expectedPosition = new Position(position.X - 1, position.Y - 1);
            var subject = fixture.Create<Direction.SouthWest>();

            // Act
            var result = subject.AwayFrom(position);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void West_WhenCallingAwayFrom_ShouldReturnWestPosition()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var expectedPosition = new Position(position.X - 1, position.Y);
            var subject = fixture.Create<Direction.West>();

            // Act
            var result = subject.AwayFrom(position);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void NorthWest_WhenCallingAwayFrom_ShouldReturnNorthWestPosition()
        {
            // Arrange
            var position = fixture.Create<Position>();
            var expectedPosition = new Position(position.X - 1, position.Y + 1);
            var subject = fixture.Create<Direction.NorthWest>();

            // Act
            var result = subject.AwayFrom(position);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPosition));
        }
    }
}