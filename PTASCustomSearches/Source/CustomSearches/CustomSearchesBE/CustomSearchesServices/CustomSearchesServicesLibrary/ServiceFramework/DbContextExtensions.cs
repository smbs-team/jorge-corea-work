namespace CustomSearchesServicesLibrary.ServiceFramework
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using CustomSearchesServicesLibrary.Exception;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Database context extensions.
    /// </summary>
    public static class DbContextExtensions
    {
        private static List<int> transientErrorNumbers = new List<int> { 4060, 40197, 40501, 40613, 49918, 49919, 49920, 11001 };

        /// <summary>
        /// Validates the entity properties.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="context">The database context.</param>
        /// <param name="entity">The entity object.</param>
        /// <param name="logger">The log.</param>
        public static void ValidateEntityProperties<T>(this DbContext context, T entity, ILogger logger = null)
        {
            var entityTypes = context.Model.GetEntityTypes();
            var properties = entityTypes.First(e => e.Name == entity.GetType().FullName).GetProperties().ToDictionary(p => p.Name, p => p);

            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                if (properties.ContainsKey(propertyInfo.Name))
                {
                    var modelEntityType = properties[propertyInfo.Name];
                    var value = propertyInfo.GetValue(entity);

                    // If the field is required then its value can't be null.
                    if (!modelEntityType.IsColumnNullable() && (value == null))
                    {
                        throw new CustomSearchesDatabaseException($"Field is required. Entity: '{entity.GetType().Name}' Field: '{propertyInfo.Name}'", innerException: null);
                    }

                    if (value == null)
                    {
                        continue;
                    }

                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        var maxLength = modelEntityType.GetMaxLength();

                        if (maxLength.HasValue)
                        {
                            if (((string)value).Length > maxLength.Value)
                            {
                                throw new CustomSearchesDatabaseException($"Field length exceeded. Entity: '{entity.GetType().Name}' Field: '{propertyInfo.Name}'. Max Length: '{maxLength}'.", innerException: null);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validates the entity properties.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The log.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ValidateAndSaveChangesAsync(this DbContext context, ILogger logger = null)
        {
            var recordsToValidate = context.ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added) || (e.State == EntityState.Modified));

            foreach (var recordToValidate in recordsToValidate)
            {
                context.ValidateEntityProperties(recordToValidate.Entity);
            }

            await context.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// Saves changes with retries.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="retryDelays">
        /// An array containing the delays before retrying each save attempt.
        /// The length of the array represents how many retry attempts it takes before the client will throw a exception.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task SaveChangesWithRetriesAsync(this DbContext context, int[] retryDelays = null)
        {
            retryDelays = retryDelays ?? new[] { 500, 1000, 2000 };

            for (int i = 0; i <= retryDelays.Length; i++)
            {
                try
                {
                    await context.SaveChangesAsync();
                    break;
                }
                catch (DbUpdateException)
                {
                    if (i == retryDelays.Length)
                    {
                        throw;
                    }

                    await Task.Delay(retryDelays[i]);
                }
            }
        }
    }
}