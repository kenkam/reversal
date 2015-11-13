using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Reversal.Tests
{
    [TestFixture]
    public class PieceBagTestFixture
    {
        private IFixture fixture;
        private IPiece[] pieces;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            pieces = fixture.Freeze<IEnumerable<IPiece>>()
                .ToArray();
        }

        [Test]
        public void GetEnumerator_WhenCalled_ShouldReturnPieces()
        {
            // Arrange
            var subject = fixture.Create<PieceBag>();

            // Act
            var result = subject;

            // Assert
            Assert.That(result, Is.EquivalentTo(pieces));
        }

        [Test]
        public void GetPiece_WhenPieceExists_ShouldReturnPiece()
        {
            // Arrange
            var subject = fixture.Create<PieceBag>();
            var expected = pieces.First();

            // Act
            var result = subject.GetPiece(expected.Position);

            // Assert
            Assert.That(result, Is.SameAs(expected));
        }

        [Test]
        public void Add_WhenCalled_ShouldAddPieceToBag()
        {
            // Arrange
            var piece = fixture.Create<IPiece>();
            var subject = fixture.Create<PieceBag>();

            // Act
            subject.Add(piece);

            // Assert
            Assert.That(subject, Has.Member(piece));
        }
    }
}