namespace AuthFlow.Persistence.Data.Interface
{
    using Microsoft.EntityFrameworkCore;
    using User = Domain.Entities.User;
    public interface IAuthFlowDbContext
    {
        int SaveChanges();
        DbSet<User> Users { get; set; }
    }
}
