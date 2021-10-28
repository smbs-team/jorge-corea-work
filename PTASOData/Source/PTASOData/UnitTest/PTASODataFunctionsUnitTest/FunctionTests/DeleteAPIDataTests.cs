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

    /// <summary>
    /// Tests the DeleteAPIData function
    /// </summary>
    [TestClass]
    public class DeleteAPIDataTests
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
                DeleteAPIData apiData = new DeleteAPIData(null);
            }
            catch (System.ArgumentNullException ex)
            {
                argumentException = ex.ParamName == "dbContextFactory";
            }

            //Assert
            Assert.IsTrue(argumentException);
        }

        #region DeleteAPIData Tests

        /// <summary>
        /// Tests the DeleteAPIData method with the data updates disabled.
        /// </summary>
        [TestMethod]
        public void Test_DeleteAPIData_DataUpdatesDisabled()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = false;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            DeleteAPIData apiData = new DeleteAPIData(dbContextFactory);

            //Act
            StatusCodeResult result = apiData.Run(request, string.Empty, Guid.Empty, log).Result as StatusCodeResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotImplemented, result.StatusCode);
        }

        /// <summary>
        /// Tests the DeleteAPIData method with an empty resource.
        /// </summary>
        [TestMethod]
        public void Test_DeleteAPIData_EmptyResource()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            DeleteAPIData apiData = new DeleteAPIData(dbContextFactory);

            //Act
            ObjectResult result = apiData.Run(request, string.Empty, Guid.Empty, log).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);

            ErrorResultModel resultValue = result.Value as ErrorResultModel;
            Assert.IsNotNull(resultValue);
            Assert.IsTrue(resultValue.Message.Contains("resource"));
        }

        /// <summary>
        /// Tests the DeleteAPIData method when the resource does not exist.
        /// </summary>
        [TestMethod]
        public void Test_DeleteAPIData_ResourceNotExist()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            DeleteAPIData apiData = new DeleteAPIData(dbContextFactory);

            //Act
            NotFoundObjectResult result = apiData.Run(request, DeleteAPIDataTests.ParcelResource + "_NotExist", Guid.Empty, log).Result as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the DeleteAPIData method when the entity does not exist.
        /// </summary>
        [TestMethod]
        public void Test_DeleteAPIData_EntityNotExist()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            DeleteAPIData apiData = new DeleteAPIData(dbContextFactory);

            //Act
            NotFoundObjectResult result = apiData.Run(request, DeleteAPIDataTests.ParcelResource, Guid.NewGuid(), log).Result as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the DeleteAPIData method with a request for resource.
        /// </summary>
        [TestMethod]
        public void Test_DeleteAPIData_Resource()
        {
            //Arrange
            ODataExtensions.AllowODataUpdates = true;
            PTASCAMADbContext dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            const int testEntityCount = 25;
            PTASCAMADataDbContextMockFactory.AddMockParcelDetails(dbContext, testEntityCount);
            var entityId = dbContext.PtasParceldetail.FirstOrDefault().PtasParceldetailid;

            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContext);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(DeleteAPIDataTests.ParcelResource, "");
            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            DeleteAPIData apiData = new DeleteAPIData(dbContextFactory);

            //Act
            NoContentResult result = apiData.Run(request, DeleteAPIDataTests.ParcelResource, entityId, log).Result as NoContentResult;

            dbContext = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            var entity = dbContext.PtasParceldetail.Where(p => p.PtasParceldetailid == entityId).FirstOrDefault();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNull(entity);
        }

        #endregion DeleteAPIData Tests
    }
}
