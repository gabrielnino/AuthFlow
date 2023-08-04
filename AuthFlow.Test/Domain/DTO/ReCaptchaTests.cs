using AuthFlow.Domain.DTO;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace AuthFlow.Test.Domain.DTO
{
    [TestFixture]
    internal class ReCaptchaTests
    {
        private ReCaptcha _reCaptcha;

        [SetUp]
        public void Setup()
        {
            _reCaptcha = new ReCaptcha();
        }

        [Test]
        public void TestToken()
        {
            var token = "reCaptchaTestToken";
            _reCaptcha.Token = token;
            _reCaptcha.Token.Should().Be(token);
        }
    }
}


