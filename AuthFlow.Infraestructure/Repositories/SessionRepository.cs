using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Validators;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories.Abstract;
using AuthFlow.Persistence.Data;

namespace AuthFlow.Infraestructure.Repositories
{
    // SessionRepository is a concrete implementation of ISessionRepository
    // It provides repository operations for the Session entity using the EntityRepository base class
    public class SessionRepository : EntityRepository<Session>, ISessionRepository
    {
        public SessionRepository(AuthFlowDbContext context, IUserRepository userRepository) : base(context)
        {
            _userRepository = userRepository;
        }

        public IUserRepository _userRepository { get; set; }

        // Override the ValidateEntity method to provide custom validation logic for the Session entity
        protected override async Task<OperationResult<bool>> ValidateEntity(Session entity, int? updatingUserId = null)
        {
            // Check if the entity is being modified or added
            var isModified = updatingUserId != null;

            // Create a new instance of the SessionValidator and validate the entity
            var validator = new SessionValidator(isModified);
            var result = validator.Validate(entity);

            // If the validation fails, return a failure operation result with the validation error message
            if (!result.IsValid)
            {
                var errorMessage = string.Empty;
                foreach (FluentValidation.Results.ValidationFailure error in result.Errors)
                {
                    errorMessage += ", " + error.ErrorMessage;
                }
                return OperationResult<bool>.Failure(string.Format(Resource.FailedDataSizeCharacter, errorMessage));
            }

            // If the entity is null, return a failure operation result with a custom error message
            if (entity is null)
            {
                return OperationResult<bool>.Failure(Resource.FailedNecesaryData);
            }

            // Check if the associated User exists
            var userExistById = await _userRepository.GetAllByFilter(x => x.Id.Equals(entity.UserId));
            var user = userExistById?.Data.FirstOrDefault();
            if (user is null)
            {
                return OperationResult<bool>.Failure(Resource.FailedUserNotFound);
            }

            // Return a success operation result
            return OperationResult<bool>.Success(true);
        }
    }
}
