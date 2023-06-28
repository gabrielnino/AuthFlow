using NUnit.Framework;
using AuthFlow.Application.DTOs;

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
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(data, result.Data);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public void GivenFailedOperation_WhenCreatingFailureResultWithMessage_ThenOperationResultIsFailureWithMessage()
        {
            // Given
            var message = "Operation failed";

            // When
            var result = OperationResult<string>.Failure(message);

            // Then
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNull(result.Data);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public void GivenSuccessfulOperation_WhenCreatingSuccessResultWithDataOnly_ThenOperationResultIsSuccessfulWithDataAndEmptyMessage()
        {
            // Given
            var data = 123;

            // When
            var result = OperationResult<int>.Success(data);

            // Then
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(data, result.Data);
            Assert.IsEmpty(result.Message);
        }
    }
}
