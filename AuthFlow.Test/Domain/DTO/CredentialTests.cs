using AuthFlow.Domain.DTO;
using FluentAssertions;

namespace AuthFlow.Test.Domain.DTO
{

    [TestFixture]
    internal class CredentialTests
    {
        private Credential _credential;

        [SetUp]
        public void Setup()
        {
            _credential = new Credential();
        }

        [Test]
        public void TestUsername()
        {
            var username = "TestUser";
            _credential.Username = username;
            _credential.Username.Should().Be(username);
        }

        [Test]
        public void TestPassword()
        {
            var password = "TestPassword";
            _credential.Password = password;
            _credential.Password.Should().Be(password);
        }
    }
}
