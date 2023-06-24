using AuthFlow.Domain.Entities;

namespace AuthFlow.Domain.Tests
{
    [TestFixture]
    public class UserTests
    {
        private User _user;

        [SetUp]
        public void GivenAUserWithProperties()
        {
            _user = new User
            {
                Id = 1,
                Username = "TestUser",
                Password = "TestPassword",
                Email = "test@user.com",
                Active = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        [Test]
        public void WhenPropertiesAreSet_ThenTheyReturnCorrectValues()
        {
            // When
            var id = _user.Id;
            var username = _user.Username;
            var password = _user.Password;
            var email = _user.Email;
            var isActive = _user.Active;
            var createdAt = _user.CreatedAt;
            var updatedAt = _user.UpdatedAt;

            // Then
            Assert.AreEqual(1, id);
            Assert.AreEqual("TestUser", username);
            Assert.AreEqual("TestPassword", password);
            Assert.AreEqual("test@user.com", email);
            Assert.IsTrue(isActive);
            Assert.NotNull(createdAt);
            Assert.NotNull(updatedAt);
        }

        [Test]
        public void WhenUsernameIsChanged_ThenUsernameReturnsNewValue()
        {
            // When
            _user.Username = "NewUser";

            // Then
            Assert.AreEqual("NewUser", _user.Username);
        }

        [Test]
        public void WhenUserIsActive_ThenActiveReturnsTrue()
        {
            // When
            var isActive = _user.Active;

            // Then
            Assert.IsTrue(isActive);
        }

        [Test]
        public void WhenUserIsDeactivated_ThenActiveReturnsFalse()
        {
            // When
            _user.Active = false;

            // Then
            Assert.IsFalse(_user.Active);
        }
    }
}
