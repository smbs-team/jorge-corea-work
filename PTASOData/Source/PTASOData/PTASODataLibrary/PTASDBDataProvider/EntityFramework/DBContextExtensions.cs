namespace PTASODataLibrary.PtasDbDataProvider.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DynamicExpresso;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using PTASODataLibrary.Helper;

    /// <summary>
    /// Extensions for the DbContext class.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Delegates that are used to query entities by name.
        /// </summary>
        private static readonly Dictionary<string, Func<DbContext, HttpRequest, HttpRequest, dynamic>> QueryDelegates = new Dictionary<string, Func<DbContext, HttpRequest, HttpRequest, dynamic>>();

        /// <summary>
        /// Resets the extensions including the underlying ODataExtensions.  Used for testing purposes.
        /// </summary>
        public static void Reset()
        {
            ODataExtensions.Reset();
            QueryDelegates.Clear();
        }

        /// <summary>
        /// Registers the database context.
        /// </summary>
        /// <param name="dbContextType">The database context type.</param>
        public static void RegisterDbContext(Type dbContextType)
        {
            var interpreter = new Interpreter().
                Reference(dbContextType).
                Reference(typeof(HttpRequest)).
                Reference(typeof(ODataExtensions));

            string dbContextTypeName = dbContextType.Name;

            if (DbContextExtensions.QueryDelegates.Count == 0)
            {
                foreach (var property in dbContextType.GetProperties())
                {
                    Type propertyType = property.PropertyType;
                    if (propertyType.IsGenericType &&
                        typeof(IQueryable).IsAssignableFrom(propertyType))
                    {
                        Type entityType = propertyType.GetGenericArguments()[0];
                        string propertyName = property.Name;
                        string entityTypeName = entityType.Name;

                        interpreter.SetVariable("entityType", entityType);
                        interpreter.Eval($"ODataExtensions.RegisterEntity(entityType)");

                        var queryDelegate = interpreter.ParseAsDelegate<Func<DbContext, HttpRequest, HttpRequest, dynamic>>(
                            $"ODataExtensions.ApplyTo(req, internalRequest, (({dbContextTypeName})dbContext).{propertyName})",
                            "dbContext",
                            "req",
                            "internalRequest");
                        DbContextExtensions.QueryDelegates.Add(entityTypeName, queryDelegate);
                    }
                }
            }
        }

        /// <summary>
        /// Gets an OData query by resource name.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="req">The HTTP request.</param>
        /// <param name="internalRequest">The internal request.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>
        /// An OData query ready to be execute.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">req or resourceName.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If resourceName is not valid.</exception>
        public static dynamic GetODataQueryByResourceName(this DbContext dbContext, HttpRequest req, HttpRequest internalRequest, string resourceName)
        {
            if (req == null)
            {
                throw new System.ArgumentNullException(nameof(req));
            }

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new System.ArgumentNullException(nameof(resourceName));
            }

            if (!QueryDelegates.ContainsKey(resourceName))
            {
                throw new ArgumentOutOfRangeException(resourceName);
            }

            Func<DbContext, HttpRequest, HttpRequest, dynamic> dbDelegate = DbContextExtensions.QueryDelegates[resourceName];
            return dbDelegate.Invoke(dbContext, req, internalRequest);
        }
    }
}
