using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.Entities;
using AuthFlow.Infraestructure.Repositories.Abstract;
using AuthFlow.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthFlow.Infraestructure.Repositories
{
    public class SessionRepository : EntityRepository<Session>, ISessionRepository
    {
        public SessionRepository(AuthFlowDbContext context) : base(context)
        {
        }

        public Task<OperationResult<bool>> ActivateSession(int id)
        {
            return this.Activate(id);
        }

        public Task<OperationResult<int>> CreateSession(Session entity)
        {
            return this.Add(entity);
        }

        public Task<OperationResult<bool>> DeleteSession(int id)
        {
            return this.Remove(id);
        }

        public Task<OperationResult<bool>> DisableSession(int id)
        {
            return this.Deactivate(id);
        }

        public Task<OperationResult<IQueryable<Session>>> GetSessionsAll()
        {
            return this.RetrieveAll();
        }

        public Task<OperationResult<IQueryable<Session>>> GetSessionsByFilter(Expression<Func<Session, bool>> predicate)
        {
            return this.RetrieveByFilter(predicate);
        }

        public Task<OperationResult<bool>> UpdateSession(Session entity)
        {
            return this.Modify(entity);
        }

        protected override async Task<OperationResult<bool>> ValidateEntity(Session entity, int? updatingUserId = null)
        {
            if (entity is null)
            {
                return OperationResult<bool>.Failure(Resource.NecesaryData);
            }

            var userByEmail = await base.GetEntitiesByFilter(p => p.UserId == entity.UserId && p.Token == entity.Token  && p.Id != updatingUserId);
            var userExistByEmail = userByEmail.FirstOrDefault();
            if (userExistByEmail is not null)
            {
                return OperationResult<bool>.Failure(Resource.FailedAlreadyRegisteredEmail);
            }

            return OperationResult<bool>.Success(true);
        }
    }
}
