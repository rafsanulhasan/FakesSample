using System;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;

using AutoFixture;

using Faker;

using Microsoft.QualityTools.Testing.Fakes;

using MsFakes.Library.Fakes;

using NUnit.Framework;

namespace MsFakes.Library.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UserTests
    {
        private static readonly object[] usersContainingInvalidPrperty = new[]
        {
            new[] { new User() { Name = null, Email = Internet.Email() } },
            new[] { new User() { Name = string.Empty, Email = Internet.Email() } },
            new[] { new User() { Name = " ", Email = Internet.Email() } },
            new[] { new User() { Name = "abc", Email = null } },
            new[] { new User() { Name = "abc", Email = string.Empty } },
            new[] { new User() { Name = "abc", Email = " " } },
            new[] { new User() { Name = "abc", Email = "abc"} },
            new[] { new User() { Name = "abc", Email = "@a"} },
            new[] { new User() { Name = "abc", Email = "@a.com"} },
            new[] { new User() { Name = "abc", Email = "@.com"} },
            new[] { new User() { Name = "abc", Email = "a.com"} },
            new[] { new User() { Name = "abc", Email = "@a."} },
        };

        private static readonly object[] usersContainingValidProperties = new[]
        {
            new[] { new User() { Name = "abc", Email = Internet.Email() } },
        };

        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
        }

        [Test]
        public void CreateInstance()
        {
            // Arrange
            var newGuid = fixture.Create<Guid>();
            var newId = newGuid.ToString().Replace("-", string.Empty);
            var utcNow = DateTime.UtcNow;
            using var shimsContext = ShimsContext.Create();
            ShimIdentityGenerator.New = () => newId;
            ShimDateTime.UtcNowGet = () => utcNow;

            // Act 
            var user = new User();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.EqualTo(newId));
                Assert.That(user.CreateDate, Is.EqualTo(utcNow));
            });
        }

        [Test]
        [TestCaseSource(nameof(usersContainingInvalidPrperty))]
        public void IsValid_WhenNameOrEmailIsNull_ReturnsFalse(User user)
        {
            // Arrange
            using var shimsContext = ShimsContext.Create();
            ShimStringHelper.IsNullOrWhiteSpaceString = _ => true;
            ShimStringHelper.IsValidEmailString = _ => true;

            // Act
            var isValid = user.IsValid();

            // Assert
            Assert.That(isValid, Is.False);
        }

        [Test]
        [TestCaseSource(nameof(usersContainingInvalidPrperty))]
        public void IsValid_WhenEmailIsInvalid_ReturnsFalse(User user)
        {
            // Arrange
            using var shimsContext = ShimsContext.Create();
            ShimStringHelper.IsNullOrWhiteSpaceString = _ => false;
            ShimStringHelper.IsValidEmailString = _ => false;

            // Act
            var isValid = user.IsValid();

            // Assert
            Assert.That(isValid, Is.False);
        }

        [Test]
        [TestCaseSource(nameof(usersContainingValidProperties))]
        public void IsValid_WhenBothNameAndEmailIsValid_ReturnsTrue(User user)
        {
            // Arrange
            using var shimsContext = ShimsContext.Create();
            ShimStringHelper.IsNullOrWhiteSpaceString = _ => false;
            ShimStringHelper.IsValidEmailString = _ => true;

            // Act
            var isValid = user.IsValid();

            // Assert
            Assert.That(isValid, Is.True);
        }
    }
}
