using AuthFlow.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthFlow.Persistence.Data
{
    // Define a AuthFlowDbContext class that inherits from DbContext.
    public class AuthFlowDbContext : DbContext
    {
        // Define a DbSet for the Person model.
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }

        // Define a constructor that takes DbContextOptions<PeopleDbContext> and passes it to the base constructor.
        public AuthFlowDbContext(DbContextOptions<AuthFlowDbContext> options) : base(options)
        {
            Users = Set<User>(); // Inicializa la propiedad People con un DbSet<Person> no nulo
            Sessions = Set<Session>(); // Inicializa la propiedad People con un DbSet<Person> no nulo
            AccessTokens = Set<AccessToken>(); // Inicializa la propiedad People con un DbSet<Person> no nulo
        }
    }
}
