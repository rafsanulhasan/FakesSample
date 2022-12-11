using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Net.Mail.Fakes;

using AutoFixture;

using Faker;

using Microsoft.QualityTools.Testing.Fakes;

using NUnit.Framework;

namespace MsFakes.Library.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class StringHelperTests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
        }

        [Test]
        [TestCase((string)null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsNullOrWhiteSpace_WhenNullOrEmptyOrWhiteSpace_ReturnsTrue(string input)
        {
            // Act
            var isNull = input.IsNullOrWhiteSpace();

            // Assert
            Assert.That(isNull, Is.True);
        }

        [Test]
        public void IsNullOrWhiteSpace_WhenHasValidValue_ReturnsFalse()
        {
            // Arrange
            var input = fixture.Create<string>();
             
            // Act
            var isNull = input.IsNullOrWhiteSpace();

            // Assert
            Assert.That(isNull, Is.False);
        }

        [Test]
        public void IsValidEmail_WhenEmailIsInvalid_ReturnsFalse()
        {
            // Arrange
            var email = fixture.Create<string>();
            var mailAddress = fixture.Create<MailAddress>();
            using var _ = ShimsContext.Create();
            ShimMailAddress.TryCreateStringMailAddressOut = (string input, out MailAddress mail) =>
            {
                mail = mailAddress;
                return false;
            };

            // Act
            var isValid = email.IsValidEmail();

            // Assert
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void IsValidEmail_WhenEmailIsValid_ReturnsTrue()
        {
            // Arrange
            var email = Internet.Email();
            var mailAddress = fixture.Create<MailAddress>();
            using var _ = ShimsContext.Create();
            ShimMailAddress.TryCreateStringMailAddressOut = (string input, out MailAddress mail) =>
            {
                mail = mailAddress;
                return true;
            };
            
            // Act
            var isValid = email.IsValidEmail();

            // Assert
            Assert.That(isValid, Is.True);
        }
    }
}
