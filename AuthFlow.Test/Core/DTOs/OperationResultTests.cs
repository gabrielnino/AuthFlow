namespace AuthFlow.Test.Core.DTOs
{
    using FluentAssertions;
    using global::AuthFlow.Application.DTOs;

    namespace AuthFlow.Test.Application.DTOs
    {
        internal class OperationResultTest
        {
            private const string MessageFailure = "Operation failed.";

            [SetUp]
            public void Setup()
            {
            }

            [Test]
            public void When_Operation_Success_Then_SuccessObjectCreated()
            {
                // Given
                string data = "Test data";

                // When
                var result = OperationResult_REVIEWED<string>.Success(data);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeTrue();
                result.Message.Should().BeNullOrEmpty();
                result.Data.Should().Be(data);
            }

            [Test]
            public void When_Operation_FailureBusinessValidation_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult_REVIEWED<string>.FailureBusinessValidation(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes_REVIEWED.BusinessValidationError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureDatabase_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult_REVIEWED<string>.FailureDatabase(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes_REVIEWED.DatabaseError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureExternalService_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult_REVIEWED<string>.FailureExtenalService(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes_REVIEWED.ExternalServicesError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureUnexpectedError_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult_REVIEWED<string>.FailureUnexpectedError(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes_REVIEWED.UnexpectedError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureDataSubmittedInvalid_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult_REVIEWED<string>.FailureDataSubmittedInvalid(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes_REVIEWED.DataSubmittedInvalid.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureConfigurationMissingError_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult_REVIEWED<string>.FailureConfigurationMissingError(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes_REVIEWED.ConfigurationMissingError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }
        }
    }

}
