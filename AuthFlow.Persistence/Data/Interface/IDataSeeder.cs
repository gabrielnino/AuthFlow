namespace AuthFlow.Persistence.Data.Interface
{
    /// <summary>
    /// Defines a contract for seeding data into the application's database.
    /// </summary>
    public interface IDataSeeder
    {
        /// <summary>
        /// Seeds predefined data into the database.
        /// Implementations should ensure that relevant data entities are populated.
        /// </summary>
        void SeedData();
    }
}
