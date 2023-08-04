using AuthFlow.Domain.DTO;
using FluentAssertions;

namespace AuthFlow.Test.Domain.DTO
{
    [TestFixture]
    internal class ReCaptchaResponseTests
    {
        private ReCaptchaResponse _reCaptchaResponse;

        [SetUp]
        public void Setup()
        {
            _reCaptchaResponse = new ReCaptchaResponse();
        }

        [Test]
        public void TestSuccess()
        {
            _reCaptchaResponse.Success = true;
            _reCaptchaResponse.Success.Should().BeTrue();
        }

        [Test]
        public void TestErrorCodes()
        {
            var errorCodes = new List<string> { "error1", "error2" };
            _reCaptchaResponse.ErrorCodes = errorCodes;
            _reCaptchaResponse.ErrorCodes.Should().BeEquivalentTo(errorCodes);
        }
    }
}
