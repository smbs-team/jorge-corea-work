namespace PTASMapTileFunctionsUnitTest.FunctionTests
{
    using Microsoft.OData.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASODataLibrary.PtasDbDataProvider.PtasCamaModel;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Integration tests for OData Api
    /// </summary>
    [TestClass]
    public class GetAPIDataTests
    {
        private const string serviceRoot = "http://localhost:7071/v1.0/API";
        private const int BatchSize = 10;


        /// <summary>
        /// Gets or sets the test context.  Assigned by the environment when tests run.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Tests a take clause in a linq query (ptas_parceldetail).
        /// </summary>
        [TestMethod]
        public void Test_QueryTop()
        {
            //Arrange
            Default.Container context = new Default.Container(new Uri(this.GetServiceRoot()));
            var parcel = context.PtasParceldetail;
            List<PtasParceldetail> parcels = null;

            //Execute
            GetAPIDataTests.Retry(() => 
            {
                var response = (context.PtasParceldetail.Take(GetAPIDataTests.BatchSize) as DataServiceQuery<PtasParceldetail>).ExecuteAsync().Result;
                parcels = response.ToList();
            });

            //Assert
            Assert.IsNotNull(parcels);
            Assert.AreEqual(GetAPIDataTests.BatchSize, parcels.Count);

            foreach (var p in parcels)
            {
                Assert.IsNotNull(p);
                Assert.IsNotNull(p.PtasParceldetailid);
                Assert.IsNotNull(p.PtasMajor);
                Assert.IsNotNull(p.PtasMinor);
            }
        }

        /// <summary>
        /// Tests an expand clause in a linq query (ptas_parceldetail).
        /// </summary>
        [TestMethod]
        public void Test_QueryExpand()
        {
            //Arrange
            Default.Container context = new Default.Container(new Uri(this.GetServiceRoot()));
            var parcel = context.PtasParceldetail;
            

            //Execute
            List<PtasParceldetail> parcels = null;
            GetAPIDataTests.Retry(() =>
            {
                var response = (context.PtasParceldetail
                    .Where(c => c.PtasDistrictidValue != null)
                    .Take(GetAPIDataTests.BatchSize) as DataServiceQuery<PtasParceldetail>)
                .Expand("PtasDistrictidValueNavigation").ExecuteAsync().Result;
                parcels = response.ToList();
            });

            //Assert
            Assert.IsNotNull(parcels);
            Assert.IsTrue(parcels.Count <= GetAPIDataTests.BatchSize);

            foreach (var p in parcels)
            {
                Assert.IsNotNull(p);
                Assert.IsNotNull(p.PtasParceldetailid);
                Assert.IsNotNull(p.PtasMajor);
                Assert.IsNotNull(p.PtasMinor);
                Assert.IsNotNull(p.PtasDistrictidValueNavigation);
                Assert.IsNotNull(p.PtasDistrictidValueNavigation.PtasDistrictid);
            }
        }

        /// <summary>
        /// Tests a select clause in a linq query (ptas_parceldetail).
        /// </summary>
        [TestMethod]
        public void Test_QuerySelect ()
        {
            //Arrange
            Default.Container context = new Default.Container(new Uri(this.GetServiceRoot()));
            var parcel = context.PtasParceldetail;

            //Execute
            List<PtasParceldetail> parcels = null;
            GetAPIDataTests.Retry(() =>
            {
                var response = (context.PtasParceldetail
                    .Take(GetAPIDataTests.BatchSize) as DataServiceQuery<PtasParceldetail>)
                .AddQueryOption("$select", "PtasParceldetailid,PtasMajor").ExecuteAsync().Result;
                parcels = response.ToList();
            });
            //Assert
            Assert.IsNotNull(parcels);
            Assert.AreEqual(GetAPIDataTests.BatchSize, parcels.Count);

            foreach (var p in parcels)
            {
                Assert.IsNotNull(p);
                Assert.IsNotNull(p.PtasParceldetailid);
                Assert.IsNotNull(p.PtasMajor);
                Assert.IsTrue(string.IsNullOrWhiteSpace(p.PtasMinor));
                Assert.IsNull(p.PtasDistrictidValueNavigation);
            }
        }

        /// <summary>
        /// Does a pass through all the entities, retrieving all linked properties.
        /// </summary>
        [TestMethod]
        public void Test_AllEntities()
        {
            foreach (var property in typeof(Default.Container).GetProperties())
            {
                Type propertyType = property.PropertyType;
                if (property.PropertyType.IsSubclassOf(typeof(DataServiceQuery)))
                {
                    Type entityType = propertyType.GenericTypeArguments[0];
                    Debug.WriteLine($"Testing entity: {entityType.ToString()}");
                    MethodInfo method = typeof(GetAPIDataTests).GetMethod(
                        nameof(GetAPIDataTests.Test_QueryTopExpand_Dynamic), 
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    MethodInfo generic = method.MakeGenericMethod(entityType);
                    generic.Invoke(this, null);
                }
            }
        }

        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <param name="toRetry">Action to retry.</param>
        private static void Retry(Action toRetry, int times = 3)
        {
            bool success = false;
            int retries = 0;
            while (!success)
            {
                try
                {
                    toRetry.Invoke();
                    success = true;
                }
                catch
                {
                    retries++;
                    if (retries >= times)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Dynamically queries for an entity with all the possible expansions, limiting the results to batch size.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        private void Test_QueryTopExpand_Dynamic<TEntity>() where TEntity : BaseEntityType
        {
            //Arrange
            Default.Container context = new Default.Container(new Uri(this.GetServiceRoot()));
            string expandString = string.Empty;
            foreach (var property in typeof(TEntity).GetProperties())
            {
                if (property.PropertyType.IsSubclassOf(typeof(BaseEntityType)))
                {
                    if (!string.IsNullOrEmpty(expandString))
                    {
                        expandString += ",";
                    }

                    expandString += property.Name;
                }
            }

            string oDataPath = typeof(TEntity).Name + "?$top=" + BatchSize.ToString();
            if (!string.IsNullOrWhiteSpace(expandString))
            {
                oDataPath += "&$expand=" + expandString;
            }

            Uri requestUri = new Uri(oDataPath, UriKind.Relative);

            //Execute
            var response = context.ExecuteAsync<TEntity>(requestUri).Result;
            IList<TEntity> entities = response.ToList();

            //Test
            Assert.IsNotNull(entities); // This is a no-crash test to check that every query on the sql server side works, even if the result set is empty.
        }

        /// <summary>
        /// Gets the service root.
        /// </summary>
        private string GetServiceRoot()
        {
            if (this.TestContext != null && this.TestContext.Properties != null)
            {
                object propertyValue = this.TestContext.Properties["ODataUrl"];
                string testContextODataUrl = propertyValue?.ToString();
                if (!string.IsNullOrWhiteSpace(testContextODataUrl))
                {
                    return testContextODataUrl;
                }
            }

            return GetAPIDataTests.serviceRoot;
        }
    }
}

