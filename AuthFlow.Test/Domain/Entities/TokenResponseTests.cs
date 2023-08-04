using AuthFlow.Domain.Entities;
using FluentAssertions;

namespace AuthFlow.Test.Domain.Entities
{
    [TestFixture]
    internal class TokenResponseTests
    {
        private TokenResponse _tokenResponse;

        [SetUp]
        public void Setup()
        {
            _tokenResponse = new TokenResponse();
        }

        [Test]
        public void TestToken()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
            _tokenResponse.Token = token;
            _tokenResponse.Token.Should().Be(token);
        }
    }
}
