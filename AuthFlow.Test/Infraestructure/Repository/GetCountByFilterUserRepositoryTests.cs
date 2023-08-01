namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Domain.DTO;
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

    internal class GetCountByFilterUserRepositoryTests : BaseTests
    {
        private const string MessageSucess = "The search in the User entity completed successfully.";

        [Test]
        public Task When_GetCountByFilter_ValidParameters_Then_Success()
        {
            // Given
            var filter = string.Empty;

            // When
            var result =  _userRepository.GetCountByFilter(filter);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().Be(100001);
            result.Result.Message.Should().Be(MessageSucess);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_GetCountByFilter_ValidParameters02_Then_Success()
        {
            // Given
            var filter = "Michael";

            // When
            var result = _userRepository.GetCountByFilter(filter);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().Be(1000);
            result.Result.Message.Should().Be(MessageSucess);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_GetCountByFilter_ValidParameters03_Then_Success()
        {
            // Given
            var filter = "Sarah";

            // When
            var result = _userRepository.GetCountByFilter(filter);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().Be(1000);
            result.Result.Message.Should().Be(MessageSucess);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_GetCountByFilter_ValidParameters04_Then_Success()
        {
            // Given
            var filter = "Mohamed";

            // When
            var result = _userRepository.GetCountByFilter(filter);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().Be(1000);
            result.Result.Message.Should().Be(MessageSucess);
            return Task.CompletedTask;
        }

        [Test]
        public Task When_GetCountByFilter_ValidParameters05_Then_Success()
        {
            // Given
            var filter = "Mo@h##\"&&||&&a6&&//(())=00**--**+++ed";

            // When
            var result = _userRepository.GetCountByFilter(filter);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().Be(0);
            result.Result.Message.Should().Be(MessageSucess);
            return Task.CompletedTask;
        }
    }
}
