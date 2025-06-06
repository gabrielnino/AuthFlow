﻿namespace AuthFlow.Test.Core.DTOs
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
                var result = OperationResult<string>.Success(data);

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
                var result = OperationResult<string>.FailureBusinessValidation(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes.BusinessValidationError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureDatabase_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult<string>.FailureDatabase(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes.DatabaseError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureExternalService_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult<string>.FailureExtenalService(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes.ExternalServicesError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureUnexpectedError_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult<string>.FailureUnexpectedError(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes.UnexpectedError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureDataSubmittedInvalid_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult<string>.FailureDataSubmittedInvalid(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes.DataSubmittedInvalid.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }

            [Test]
            public void When_Operation_FailureConfigurationMissingError_Then_FailureObjectCreated()
            {
                // When
                var result = OperationResult<string>.FailureConfigurationMissingError(MessageFailure);

                // Then
                result.Should().NotBeNull();
                result.IsSuccessful.Should().BeFalse();
                var expected = ErrorTypes.ConfigurationMissingError.ToErrorString();
                result.Error.Should().Be(expected);
                result.Message.Should().Be(MessageFailure);
                result.Data.Should().BeNull();
            }
        }
    }

}
