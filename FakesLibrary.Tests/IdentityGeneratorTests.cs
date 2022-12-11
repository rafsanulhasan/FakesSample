using System;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;

using AutoFixture;

using Microsoft.QualityTools.Testing.Fakes;

using NUnit.Framework;

namespace MsFakes.Library.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IdentityGeneratorTests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
        }

        [Test]
        public void New_GeneratedNewGuid_WithoutDashes()
        {
            // Arrange
            var newGuid = fixture.Create<Guid>();
            var expectedNewId = newGuid.ToString().Replace("-", string.Empty);
            using var _ = ShimsContext.Create();
            ShimGuid.NewGuid = () => newGuid;

            // Act
            var actualId = IdentityGenerator.New();

            // Assert
            Assert.That(actualId, Is.EqualTo(expectedNewId));
        }
    }
}
