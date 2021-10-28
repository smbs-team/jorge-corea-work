namespace PTASMapTileFunctionsUnitTest.FunctionTests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
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
    /// Tests the CreateAPIData function
    /// </summary>
    [TestClass]
    public class CreateAPIDataTests
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
                CreateAPIData apiData = new CreateAPIData(null);
            }
            catch (System.ArgumentNullException ex)
            {
                argumentException = ex.ParamName == "dbContextFactory";
            }

            //Assert
            Assert.IsTrue(argumentException);
        }

        #region CreateAPIData Tests

        /// <summary>
        /// Tests the CreateAPIData method with the data updates disabled.
        /// </summary>
        [TestMethod]
        public void Test_CreateAPIData_DataUpdatesDisabled()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = false;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            CreateAPIData apiData = new CreateAPIData(dbContextFactory);

            //Act
            StatusCodeResult result = apiData.Run(request, string.Empty, log).Result as StatusCodeResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotImplemented, result.StatusCode);
        }

        /// <summary>
        /// Tests the CreateAPIData method with an empty resource.
        /// </summary>
        [TestMethod]
        public void Test_CreateAPIData_EmptyResource()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            CreateAPIData apiData = new CreateAPIData(dbContextFactory);

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
        /// Tests the CreateAPIData method when the resource does not exist.
        /// </summary>
        [TestMethod]
        public void Test_CreateAPIData_ResourceNotExist()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty, new PtasParceldetail());
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            CreateAPIData apiData = new CreateAPIData(dbContextFactory);

            //Act
            NotFoundObjectResult result = apiData.Run(request, CreateAPIDataTests.ParcelResource + "_NotExist", log).Result as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the CreateAPIData method when the entity already exists.
        /// </summary>
        [TestMethod]
        public void Test_CreateAPIData_EntityAlreadyExists()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            const int testEntityCount = 25;
            PTASCAMADataDbContextMockFactory.AddMockParcelDetails(dbContext, testEntityCount);
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);

            var firstEntity = dbContext.PtasParceldetail.FirstOrDefault();
            var newEntity = new PtasParceldetail { PtasParceldetailid = firstEntity.PtasParceldetailid };
            HttpRequest request = RequestFactory.CreateFakeODataRequest(CreateAPIDataTests.ParcelResource, "", newEntity);

            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            CreateAPIData apiData = new CreateAPIData(dbContextFactory);

            //Act
            ConflictObjectResult result = apiData.Run(request, CreateAPIDataTests.ParcelResource, log).Result as ConflictObjectResult;

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the CreateAPIData method with an entity without primary key.
        /// </summary>
        [TestMethod]
        public void Test_CreateAPIData_EntityWithoutPrimaryKey()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();

            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);

            dynamic newEntity = new ExpandoObject();
            HttpRequest request = RequestFactory.CreateFakeODataRequest(CreateAPIDataTests.ParcelResource, "", newEntity);

            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            CreateAPIData apiData = new CreateAPIData(dbContextFactory);

            //Act
            BadRequestObjectResult result = apiData.Run(request, CreateAPIDataTests.ParcelResource, log).Result as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the CreateAPIData method with an entity containing
        /// a property that isn't a member on the object.
        /// </summary>
        [TestMethod]
        public void Test_CreateAPIData_EntityMissingMember()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();

            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);

            dynamic newEntity = new ExpandoObject();
            newEntity.PtasParceldetailid = Guid.NewGuid();
            newEntity.MissingMember = true;
            HttpRequest request = RequestFactory.CreateFakeODataRequest(CreateAPIDataTests.ParcelResource, "", newEntity);

            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            CreateAPIData apiData = new CreateAPIData(dbContextFactory);

            //Act
            ObjectResult result = apiData.Run(request, CreateAPIDataTests.ParcelResource, log).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);

            ErrorResultModel resultValue = result.Value as ErrorResultModel;
            Assert.IsNotNull(resultValue);
            Assert.IsTrue(resultValue.Message.Contains(nameof(newEntity.MissingMember)));
        }

        /// <summary>
        /// Tests the CreateAPIData method with a request for resource.
        /// </summary>
        [TestMethod]
        public void Test_CreateAPIData_Resource()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();

            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);

            const string expectedPtasMajor = "1";
            var newEntity = new PtasParceldetail { PtasParceldetailid = Guid.NewGuid(), PtasMajor = expectedPtasMajor };
            HttpRequest request = RequestFactory.CreateFakeODataRequest(CreateAPIDataTests.ParcelResource, "", newEntity);

            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            CreateAPIData apiData = new CreateAPIData(dbContextFactory);

            //Act
            StatusCodeResult result = apiData.Run(request, CreateAPIDataTests.ParcelResource, log).Result as StatusCodeResult;

            dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            var entity = dbContext.PtasParceldetail.Where(p => p.PtasParceldetailid == newEntity.PtasParceldetailid).FirstOrDefault();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);
            Assert.IsNotNull(entity);
            Assert.AreEqual(expectedPtasMajor, entity.PtasMajor);
        }

        #endregion CreateAPIData Tests
    }
}
