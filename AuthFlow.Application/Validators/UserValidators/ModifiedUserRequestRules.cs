using AuthFlow.Domain.Entities;
using FluentValidation;

namespace AuthFlow.Application.Validators.UserValidators
{
    public class ModifiedUserRequestRules : AbstractValidator<User>
    {
        public ModifiedUserRequestRules()
        {
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
        }
    }
}
