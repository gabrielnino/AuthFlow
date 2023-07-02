using AuthFlow.Application.DTOs;
using FluentAssertions;

namespace AuthFlow.Application.Tests
{
    [TestFixture]
    public class OperationResultTests
    {
        [Test] 
        public void GivenSuccessfulOperation_WhenCreatingSuccessResultWithDataAndMessage_ThenOperationResultIsSuccessfulWithDataAndMessage()
        {
            // Given
            var data = "SuccessData";
            var message = "Operation completed successfully";

            // When
            var result = OperationResult<string>.Success(data, message);

            // Then
            result.Message.Should().Be(message);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().Be(data);
        }

        [Test]
        public void GivenFailedOperation_WhenCreatingFailureResultWithMessage_ThenOperationResultIsFailureWithMessage()
        {
            // Given
            var message = "Operation failed";

            // When
            var result = OperationResult<string>.Failure(message);

            // Then
            result.Message.Should().Be(message);
            result.IsSuccessful.Should().BeFalse();
            result.Data.Should().Be(null);

        }

        [Test]
        public void GivenSuccessfulOperation_WhenCreatingSuccessResultWithDataOnly_ThenOperationResultIsSuccessfulWithDataAndEmptyMessage()
        {
            // Given
            var data = 123;

            // When
            var result = OperationResult<int>.Success(data);

            // Then
            result.Message.Should().Be(string.Empty);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().Be(data);
        }
    }
}
