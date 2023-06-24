using AuthFlow.Domain.Entities;
using FluentValidation;

namespace AuthFlow.Application.Validators
{
    // This class is a FluentValidation Validator for Session entities
    // FluentValidation is a .NET library for building strongly-typed validation rules
    public class SessionValidator : AbstractValidator<Session>
    {
        public SessionValidator(bool isModified = false)
        {

            if (isModified)
            {
                // Rule for 'Id' property
                // It must be greater than 0, not null and not empty
                RuleFor(x => x.Id).GreaterThan(0).NotNull().NotEmpty();
            }

            

            // Rule for 'UserId' property
            // It must be greater than 0, not null and not empty
            RuleFor(x => x.UserId).GreaterThan(0).NotNull().NotEmpty();

            // Rule for 'Token' property
            // It must not be null, not empty, and its length should be between 4 and 100
            RuleFor(x => x.Token).NotNull().NotEmpty().MinimumLength(4).MaximumLength(100);

            // Rule for 'Expiration' property
            // It must be greater than 'CreatedAt', not null and not empty
            RuleFor(x => x.Expiration).GreaterThan(x => x.CreatedAt).NotNull().NotEmpty();

            // Rule for 'Active' property
            // It must not be null
            RuleFor(x => x.Active).NotNull();
        }
    }
}
