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
        public SessionRepository(AuthFlowDbContext context) : base(context)
        {
        }

        protected override async Task<OperationResult<bool>> ValidateEntity(Session entity, int? updatingUserId = null)
        {
            var validator = new SessionValidator();
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

            var userByEmail = await base.GetAllByFilter(p => p.UserId == entity.UserId && p.Token == entity.Token  && p.Id != updatingUserId);
            var userExistByEmail = userByEmail?.Data?.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredToken);
            }

            return OperationResult<bool>.Success(true);
        }
    }
}
