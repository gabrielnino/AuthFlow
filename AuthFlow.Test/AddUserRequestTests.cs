using AuthFlow.Domain.DTO;

namespace AuthFlow.Domain.Tests
{
    [TestFixture]
    public class AddUserRequestTests
    {
        private AddUserRequest _addUserRequest;

        [SetUp]
        public void GivenAnAddUserRequestWithProperties()
        {
            _addUserRequest = new AddUserRequest
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
            var username = _addUserRequest.Username;

            // Then
            Assert.AreEqual("testUser", username);
        }

        [Test]
        public void WhenPasswordIsSet_ThenItReturnsCorrectValue()
        {
            // When
            var password = _addUserRequest.Password;

            // Then
            Assert.AreEqual("testPassword", password);
        }

        [Test]
        public void WhenEmailIsSet_ThenItReturnsCorrectValue()
        {
            // When
            var email = _addUserRequest.Email;

            // Then
            Assert.AreEqual("test@test.com", email);
        }
    }
}
