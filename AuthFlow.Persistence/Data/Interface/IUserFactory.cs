namespace AuthFlow.Persistence.Data.Interface
{
    using User = Domain.Entities.User;
    public interface IUserFactory
    {
        User CreateUser(string username, string passwordHash, string email, DateTime creationDateTime);
    }
}
