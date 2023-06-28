using AuthFlow.Domain.DTO;

namespace AuthFlow.Domain.Tests
{
    [TestFixture]
    public class AddSessionRequestTests
    {
        private AddSessionRequest _addSessionRequest;

        [SetUp]
        public void GivenAnAddSessionRequestWithProperties()
        {
            _addSessionRequest = new AddSessionRequest
            {
                UserId = 1,
                Token = "abcd1234"
            };
        }

        [Test]
        public void WhenUserIdIsSet_ThenItReturnsCorrectValue()
        {
            // When
            var userId = _addSessionRequest.UserId;

            // Then
            Assert.AreEqual(1, userId);
        }

        [Test]
        public void WhenTokenIsSet_ThenItReturnsCorrectValue()
        {
            // When
            var token = _addSessionRequest.Token;

            // Then
            Assert.AreEqual("abcd1234", token);
        }

        [Test]
        public void WhenTokenIsChanged_ThenTokenReturnsNewValue()
        {
            // When
            _addSessionRequest.Token = "newtoken1234";

            // Then
            Assert.AreEqual("newtoken1234", _addSessionRequest.Token);
        }
    }
}
