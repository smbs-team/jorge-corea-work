namespace PtasUnitTestCommon
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Helper class to create mock db context objects.
    /// </summary>
    public class MockDbFactory
    {
        /// <summary>
        /// Creates a mock database context factory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <returns></returns>
        public static IFactory<T> CreateDbContextFactory<T>(T dbContext) where T : DbContext
        {
            return new Factory<T>(() =>
            {
                return dbContext;
            });
        }

        /// <summary>
        /// Creates a DBContextOptions for an in memory database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DbContextOptions<T> CreateInMemoryDbContextOptions<T>() where T : DbContext
        {

            return (DbContextOptions<T>) new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(databaseName: "InMemory")
                .Options;
        }

    }
}
