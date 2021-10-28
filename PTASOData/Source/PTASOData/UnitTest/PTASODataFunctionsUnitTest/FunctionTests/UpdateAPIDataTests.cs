namespace PTASMapTileFunctionsUnitTest.FunctionTests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Net;
    using PTASServicesCommon.DependencyInjection;
    using PTASODataFunctions.Functions;
    using PtasUnitTestCommon;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASODataUnitTestCommon.Helper;
    using PTASODataLibraryUnitTest.Helper;
    using PTASODataFunctions.Exception;
    using Microsoft.EntityFrameworkCore;
    using PTASODataLibrary.Helper;
    using PTASODataLibrary.PtasDbDataProvider.PtasCamaModel;
    using System.Dynamic;

    /// <summary>
    /// Tests the UpdateAPIData function
    /// </summary>
    [TestClass]
    public class UpdateAPIDataTests
    {
        private const string ParcelResource = "PtasParceldetail";



        /// <summary>
        /// Tests constructor when dbContextFactory is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_DBContextNull()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            bool argumentException = false;

            //Act
            try
            {
                UpdateAPIData apiData = new UpdateAPIData(null);
            }
            catch (System.ArgumentNullException ex)
            {
                argumentException = ex.ParamName == "dbContextFactory";
            }

            //Assert
            Assert.IsTrue(argumentException);
        }

        #region UpdateAPIData Tests

        /// <summary>
        /// Tests the UpdateAPIData method with the data updates disabled.
        /// </summary>
        [TestMethod]
        public void Test_UpdateAPIData_DataUpdatesDisabled()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = false;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            UpdateAPIData apiData = new UpdateAPIData(dbContextFactory);

            //Act
            StatusCodeResult result = apiData.Run(request, string.Empty, log).Result as StatusCodeResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotImplemented, result.StatusCode);
        }

        /// <summary>
        /// Tests the UpdateAPIData method with an empty resource.
        /// </summary>
        [TestMethod]
        public void Test_UpdateAPIData_EmptyResource()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            UpdateAPIData apiData = new UpdateAPIData(dbContextFactory);

            //Act
            ObjectResult result = apiData.Run(request, string.Empty, log).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);

            ErrorResultModel resultValue = result.Value as ErrorResultModel;
            Assert.IsNotNull(resultValue);
            Assert.IsTrue(resultValue.Message.Contains("resource"));
        }

        /// <summary>
        /// Tests the UpdateAPIData method when the resource does not exist.
        /// </summary>
        [TestMethod]
        public void Test_UpdateAPIData_ResourceNotExist()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty, new PtasParceldetail());
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            UpdateAPIData apiData = new UpdateAPIData(dbContextFactory);

            //Act
            NotFoundObjectResult result = apiData.Run(request, UpdateAPIDataTests.ParcelResource + "_NotExist", log).Result as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the UpdateAPIData method when the entity does not exist.
        /// </summary>
        [TestMethod]
        public void Test_UpdateAPIData_EntityNotExist()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty, new PtasParceldetail());
            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            UpdateAPIData apiData = new UpdateAPIData(dbContextFactory);

            //Act
            NotFoundObjectResult result = apiData.Run(request, UpdateAPIDataTests.ParcelResource, log).Result as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the UpdateAPIData method with an entity without primary key.
        /// </summary>
        [TestMethod]
        public void Test_UpdateAPIData_EntityWithoutPrimaryKey()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();

            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);

            dynamic newEntity = new ExpandoObject();
            HttpRequest request = RequestFactory.CreateFakeODataRequest(UpdateAPIDataTests.ParcelResource, "", newEntity);

            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            UpdateAPIData apiData = new UpdateAPIData(dbContextFactory);

            //Act
            BadRequestObjectResult result = apiData.Run(request, UpdateAPIDataTests.ParcelResource, log).Result as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the UpdateAPIData method with an entity containing
        /// a property that isn't a member on the object.
        /// </summary>
        [TestMethod]
        public void Test_UpdateAPIData_EntityMissingMember()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            const int testEntityCount = 25;
            PTASCAMADataDbContextMockFactory.AddMockParcelDetails(dbContext, testEntityCount);
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            var entityToUpdate = dbContext.PtasParceldetail.FirstOrDefault();

            dynamic newEntity = new ExpandoObject();
            newEntity.PtasParceldetailid = entityToUpdate.PtasParceldetailid;
            newEntity.MissingMember = true;
            HttpRequest request = RequestFactory.CreateFakeODataRequest(UpdateAPIDataTests.ParcelResource, "", newEntity);

            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            UpdateAPIData apiData = new UpdateAPIData(dbContextFactory);

            //Act
            ObjectResult result = apiData.Run(request, UpdateAPIDataTests.ParcelResource, log).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);

            ErrorResultModel resultValue = result.Value as ErrorResultModel;
            Assert.IsNotNull(resultValue);
            Assert.IsTrue(resultValue.Message.Contains(nameof(newEntity.MissingMember)));
        }

        /// <summary>
        /// Tests the UpdateAPIData method with a request for resource.
        /// </summary>
        [TestMethod]
        public void Test_UpdateAPIData_Resource()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            const int testEntityCount = 25;
            PTASCAMADataDbContextMockFactory.AddMockParcelDetails(dbContext, testEntityCount);
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            
            var entityToUpdate = dbContext.PtasParceldetail.FirstOrDefault();
            const string expectedPtasMajor = "PtasMajor_Update";
            entityToUpdate.PtasMajor = expectedPtasMajor;

            HttpRequest request = RequestFactory.CreateFakeODataRequest(UpdateAPIDataTests.ParcelResource, "", entityToUpdate);

            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            UpdateAPIData apiData = new UpdateAPIData(dbContextFactory);

            //Act
            NoContentResult result = apiData.Run(request, UpdateAPIDataTests.ParcelResource, log).Result as NoContentResult;

            dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            var entity = dbContext.PtasParceldetail.Where(p => p.PtasParceldetailid == entityToUpdate.PtasParceldetailid).FirstOrDefault();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(entity);
            Assert.AreEqual(expectedPtasMajor, entity.PtasMajor);
        }

        #endregion UpdateAPIData Tests
    }
}
