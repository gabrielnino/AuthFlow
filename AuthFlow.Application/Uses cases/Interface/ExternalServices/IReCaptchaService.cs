// The namespace AuthFlow.Application.Interfaces contains all the interface definitions for the application layer.
// This is part of the application layer in the Clean Architecture approach and is used to define contracts or services needed by the application.
using AuthFlow.Application.DTOs;
using AuthFlow.Domain.DTO;

namespace AuthFlow.Application.Interfaces
{
    // The IReCaptchaService interface defines the contract for a reCAPTCHA validation service.
    // reCAPTCHA is a Google service that helps protect websites from spam and abuse. This service 
    // is responsible for validating the reCAPTCHA token generated on the client side.
    public interface IReCaptchaService
    {
        // The Validate method takes a reCAPTCHA token as input and returns an OperationResult which encapsulates 
        // the result of the validation operation. The result is a boolean indicating whether the token validation was 
        // successful or not. The implementation of this method should handle the actual interaction with the reCAPTCHA service.
        Task<OperationResult<bool>> Validate(ReCaptcha token);
    }
}
