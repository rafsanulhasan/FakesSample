using System;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;

using Microsoft.QualityTools.Testing.Fakes;

using MsFakes.Library.Fakes;

using NUnit.Framework;

namespace MsFakes.Library.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UserServiceTests
    {
        private static readonly object[] updateUserArgs = new object[]
        {
            new object[] { new User() },
        };
        private IUserService userService;

        [SetUp]
        public void Setup()
        {
            userService = new UserService();
        }

        [Test]
        [TestCase("a", "a@a.com")]
        public void CreateUser_SetsCorrectNameAndEmail_ReturnUser(string name, string email)
        {
            // Arrange
            var utcNow = DateTime.UtcNow;
            using var _ = ShimsContext.Create();
            ShimUser.Constructor = u =>
            {
                u.Name = name;
                u.Email = email;
            };

            // Act
            var user = userService.CreateUser(name, email);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(user.Name, Is.EqualTo(name));
                Assert.That(user.Email, Is.EqualTo(email));
            });
        }

        [Test]
        [TestCaseSource(nameof(updateUserArgs))]
        public void UpdateUser_SetsCorrectLastModifiedDate_ReturnUser(User user)
        {
            // Arrange
            var utcNow = DateTime.UtcNow;
            using var _ = ShimsContext.Create();
            ShimDateTime.UtcNowGet = () => utcNow;

            // Act
            var actualUser = userService.UpdateUser(user);

            // Assert
            Assert.That(user.LastModifiedDate, Is.EqualTo(utcNow));
        }
    }
}
