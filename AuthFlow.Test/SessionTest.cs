using AuthFlow.Domain.Entities;
using AuthFlow.Domain.Interfaces;

namespace AuthFlow.Domain.Tests
{
    [TestFixture]
    public class SessionTests
    {
        private Session _session;

        [SetUp]
        public void GivenASessionWithProperties()
        {
            _session = new Session
            {
                Id = 1,
                UserId = 123,
                Token = "abcd1234",
                Expiration = DateTime.Now.AddHours(1),
                CreatedAt = DateTime.Now,
                Active = true
            };
        }

        [Test]
        public void GivenASession_WhenCheckingType_ThenItInheritsFromIEntity()
        {
            // Given
            // Defined in Setup

            // When
            bool isEntity = _session is IEntity;

            // Then
            Assert.IsTrue(isEntity);
        }

        [Test]
        public void WhenPropertiesAreSet_ThenTheyReturnCorrectValues()
        {
            // When
            var id = _session.Id;
            var userId = _session.UserId;
            var token = _session.Token;
            var expiration = _session.Expiration;
            var createdAt = _session.CreatedAt;
            var isActive = _session.Active;

            // Then
            Assert.AreEqual(1, id);
            Assert.AreEqual(123, userId);
            Assert.AreEqual("abcd1234", token);
            Assert.IsTrue(isActive);
            Assert.LessOrEqual(DateTime.Now, expiration);
            Assert.LessOrEqual(createdAt, DateTime.Now);
        }

        [Test]
        public void WhenTokenIsChanged_ThenTokenReturnsNewValue()
        {
            // When
            _session.Token = "newtoken1234";

            // Then
            Assert.AreEqual("newtoken1234", _session.Token);
        }

        [Test]
        public void WhenSessionIsDeactivated_ThenActiveReturnsFalse()
        {
            // When
            _session.Active = false;

            // Then
            Assert.IsFalse(_session.Active);
        }
    }
}
