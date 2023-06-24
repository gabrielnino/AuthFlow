using AuthFlow.Domain.Entities;
using FluentValidation;

namespace AuthFlow.Application.Validators
{
    // This class is a FluentValidation Validator for User entities
    // FluentValidation is a .NET library for building strongly-typed validation rules
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            // Rule for 'Id' property
            // It must be greater than 0, not null and not empty
            RuleFor(x => x.Id).GreaterThan(0).NotNull().NotEmpty();

            // Rule for 'Username' property
            // It must not be null, not empty, and its length should be between 6 and 50
            RuleFor(x => x.Username).NotNull().NotEmpty().MinimumLength(6).MaximumLength(50);

            // Rule for 'Password' property
            // It must not be null, not empty, and its length should be between 6 and 100
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(6).MaximumLength(100);

            // Rule for 'Email' property
            // It must not be null, not empty, and its length should be between 10 and 100
            RuleFor(x => x.Email).NotNull().NotEmpty().MinimumLength(10).MaximumLength(100);

            // Rule for 'CreatedAt' property
            // It must not be null and should be greater than current UTC date
            ///RuleFor(x => x.CreatedAt).NotNull().GreaterThan(DateTime.UtcNow.Date);

            // Rule for 'Active' property
            // It must not be null
            RuleFor(x => x.Active).NotNull();
        }
    }
}
