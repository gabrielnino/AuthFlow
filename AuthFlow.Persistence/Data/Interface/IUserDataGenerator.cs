namespace AuthFlow.Persistence.Data.Interface
{
    using System.Collections.Immutable;
    using User = Domain.Entities.User;

    public interface IUserDataGenerator
    {
        User GetUserAnonymous();
        ImmutableList<User> GetUsers();
        ImmutableList<User> GenerateMassiveUserList(
            int sizeFirstNames = 300,
            int sizeMiddleName = 10,
            int sizeLastname = 10,
            int sizeSecondlastName = 10);
    }
}
