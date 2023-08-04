using AuthFlow.Domain.DTO;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFlow.Test.Domain.DTO
{
    [TestFixture]
    internal class AddUserRequestTests
    {
        private AddUserRequest _addUserRequest;

        [SetUp]
        public void Setup()
        {
            _addUserRequest = new AddUserRequest();
        }

        [Test]
        public void TestUsername()
        {
            var username = "TestUser";
            _addUserRequest.Username = username;
            _addUserRequest.Username.Should().Be(username);
        }

        [Test]
        public void TestPassword()
        {
            var password = "TestPassword";
            _addUserRequest.Password = password;
            _addUserRequest.Password.Should().Be(password);
        }

        [Test]
        public void TestEmail()
        {
            var email = "test@example.com";
            _addUserRequest.Email = email;
            _addUserRequest.Email.Should().Be(email);
        }
    }
}
