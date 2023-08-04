using AuthFlow.Domain.Entities;
using AuthFlow.Test.Infraestructure.Repository.BaseTest;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFlow.Test.Domain.Entities
{
    [TestFixture]
    internal class UserTests
    {
        private User _user;

        [SetUp]
        public void Setup()
        {
            _user = new User();
        }

        [Test]
        public void TestUserId()
        {
            _user.Id = 1;
            _user.Id.Should().Be(1);
        }

        [Test]
        public void TestUsername()
        {
            var username = "test_user";
            _user.Username = username;
            _user.Username.Should().Be(username);
        }

        [Test]
        public void TestPassword()
        {
            var password = "password";
            _user.Password = password;
            _user.Password.Should().Be(password);
        }

        [Test]
        public void TestEmail()
        {
            var email = "test_user@example.com";
            _user.Email = email;
            _user.Email.Should().Be(email);
        }

        [Test]
        public void TestCreatedAt()
        {
            var createdAt = DateTime.Now;
            _user.CreatedAt = createdAt;
            _user.CreatedAt.Should().Be(createdAt);
        }

        [Test]
        public void TestUpdatedAt()
        {
            var updatedAt = DateTime.Now;
            _user.UpdatedAt = updatedAt;
            _user.UpdatedAt.Should().Be(updatedAt);
        }

        [Test]
        public void TestActive()
        {
            _user.Active = true;
            _user.Active.Should().BeTrue();
        }
    }
}
