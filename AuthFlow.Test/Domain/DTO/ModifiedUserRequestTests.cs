using AuthFlow.Domain.DTO;
using FluentAssertions;

namespace AuthFlow.Test.Domain.DTO
{
    [TestFixture]
    internal class ModifiedUserRequestTests
    {
        private ModifiedUserRequest _modifiedUserRequest;

        [SetUp]
        public void Setup()
        {
            _modifiedUserRequest = new ModifiedUserRequest();
        }

        [Test]
        public void TestUsername()
        {
            var username = "TestUser";
            _modifiedUserRequest.Username = username;
            _modifiedUserRequest.Username.Should().Be(username);
        }

        [Test]
        public void TestPassword()
        {
            var password = "TestPassword";
            _modifiedUserRequest.Password = password;
            _modifiedUserRequest.Password.Should().Be(password);
        }

        [Test]
        public void TestEmail()
        {
            var email = "test@example.com";
            _modifiedUserRequest.Email = email;
            _modifiedUserRequest.Email.Should().Be(email);
        }
    }
}
