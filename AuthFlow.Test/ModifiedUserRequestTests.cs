using AuthFlow.Domain.DTO;

namespace AuthFlow.Domain.Tests
{
    [TestFixture]
    public class ModifiedUserRequestTests
    {
        private ModifiedUserRequest _modifiedUserRequest;

        [SetUp]
        public void GivenAModifiedUserRequestWithProperties()
        {
            _modifiedUserRequest = new ModifiedUserRequest
            {
                Username = "testUser",
                Password = "testPassword",
                Email = "test@test.com"
            };
        }

        [Test]
        public void WhenUsernameIsSet_ThenItReturnsCorrectValue()
        {
            // When
            var username = _modifiedUserRequest.Username;

            // Then
            Assert.AreEqual("testUser", username);
        }

        [Test]
        public void WhenPasswordIsSet_ThenItReturnsCorrectValue()
        {
            // When
            var password = _modifiedUserRequest.Password;

            // Then
            Assert.AreEqual("testPassword", password);
        }

        [Test]
        public void WhenEmailIsSet_ThenItReturnsCorrectValue()
        {
            // When
            var email = _modifiedUserRequest.Email;

            // Then
            Assert.AreEqual("test@test.com", email);
        }
    }
}
