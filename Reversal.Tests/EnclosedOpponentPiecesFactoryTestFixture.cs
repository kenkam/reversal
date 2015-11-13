using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Reversal.Tests
{
    [TestFixture]
    public class EnclosedOpponentPiecesFactoryTestFixture
    {
        private IFixture fixture;

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        [Test]
        public void Create_WhenCalled_ShouldReturnEnclosedOpponentPieces()
        {
            // Arrange
            var pieceBag = fixture.Create<IPieceBag>();
            var subject = fixture.Create<EnclosedOpponentPiecesFactory>();

            // Act
            var result = subject.Create(pieceBag);

            // Assert
            Assert.That(result, Is.TypeOf<EnclosedOpponentPieces>());
        }
    }
}