using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Validators;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories.Abstract;
using AuthFlow.Persistence.Data;

namespace AuthFlow.Infraestructure.Repositories
{
    public class SessionRepository : EntityRepository<Session>, ISessionRepository
    {
        public SessionRepository(AuthFlowDbContext context, IUserRepository userRepository) : base(context)
        {
            _userRepository = userRepository;
        }

        public IUserRepository _userRepository { get; set; }

        protected override async Task<OperationResult<bool>> ValidateEntity(Session entity, int? updatingUserId = null)
        {
            var isModified = updatingUserId != null;
            var validator = new SessionValidator(isModified);
            var result = validator.Validate(entity);
            if (!result.IsValid)
            {
                var errorMessage = string.Empty;
                foreach (FluentValidation.Results.ValidationFailure? error in result.Errors)
                {
                    errorMessage += ", " + error.ErrorMessage;
                }
                return OperationResult<bool>.Failure(string.Format(Resource.FailedDataSizeCharacter, errorMessage));
            }

            if (entity is null)
            {
                return OperationResult<bool>.Failure(Resource.NecesaryData);
            }

            var userExistById = await _userRepository.GetAllByFilter(x => x.Id.Equals(entity.UserId));
            var user = userExistById?.Data.FirstOrDefault();
            if (user is null)
            {
                return OperationResult<bool>.Failure(Resource.FailedUserNotFound);
            }

            return OperationResult<bool>.Success(true);
        }
    }
}
