using AuthFlow.Domain.Entities;
using FluentAssertions;

namespace AuthFlow.Test.Domain.Entities
{
    [TestFixture]
    internal class LogTests
    {
        private Log _log;

        [SetUp]
        public void Setup()
        {
            _log = new Log();
        }

        [Test]
        public void TestMessage()
        {
            var message = "This is a test log message";
            _log.Message = message;
            _log.Message.Should().Be(message);
        }

        [Test]
        public void TestEntityName()
        {
            var entityName = "TestEntity";
            _log.EntityName = entityName;
            _log.EntityName.Should().Be(entityName);
        }

        [Test]
        public void TestEntityValue()
        {
            var entityValue = "{\"Property\": \"Value\"}";
            _log.EntityValue = entityValue;
            _log.EntityValue.Should().Be(entityValue);
        }

        [Test]
        public void TestLevel()
        {
            var level = LogLevel.Error;
            _log.Level = level;
            _log.Level.Should().Be(level);
        }

        [Test]
        public void TestOperation()
        {
            var operation = OperationExecute.GenerateOtp;
            _log.Operation = operation;
            _log.Operation.Should().Be(operation);
        }

        [Test]
        public void TestCreatedAt()
        {
            var createdAt = DateTime.Now;
            _log.CreatedAt = createdAt;
            _log.CreatedAt.Should().Be(createdAt);
        }
    }
}
