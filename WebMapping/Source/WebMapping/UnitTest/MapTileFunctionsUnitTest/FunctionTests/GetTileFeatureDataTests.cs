using PTASMapTileFunctions.Functions;
using PTASMapTileServicesLibrary.TileProvider;
using PTASMapTileServicesLibrary.TileProvider.Blob;
using PTASMapTileServicesLibrary.TileProvider.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASServicesCommon.CloudStorage;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PTASWebMappingUnitTestCommon;
using PTASMapTileServicesLibrary.TileFeatureDataProvider;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
using PTASMapTileServicesLibrary.Geography.Data;
using PTASServicesCommon.DependencyInjection;

namespace PTASMapTileFunctionsUnitTest.FunctionTests
{
    /// <summary>
    /// Tests the GetTileFeatureData function
    /// </summary>
    [TestClass]
    public class GetTileFeatureDataTests
    {
        private const string TileFeatureDataProviderFactoryParameterName = "tileFeatureDataProviderFactory";
        private const string DbContextParameterName = "dbContext";

        private const string MockLayerId = "MockLayerId";
        private const int ZoomLevel = 14;
        private const double MinX = 0.1;
        private const double MinY = 0.2;
        private const double MaxX = 0.3;
        private const double MaxY = 0.5;
        private const string MockErrorMessage = "Mock Error Message";

        #region Constructor Tests

        /// <summary>
        /// Tests constructor when dbContext is null
        /// </summary>
        //[TestMethod]
        public void Test_Constructor_DBContextNull()
        {
            // Arrange                       
            Mock<ITileFeatureDataProvider> CreateMockTileFeatureDataProvider = WebMappingMockFactory.CreateMockTileFeatureDataProvider(null);
            Mock<ITileFeatureDataProviderFactory> tileFeatureDataProviderFactory = WebMappingMockFactory.GetMockTileProviderFactory(CreateMockTileFeatureDataProvider.Object);
                
            bool exceptionHandled = false;

            // Act
            try
            {
                GetExtentFeatureData getTileFunction = new GetExtentFeatureData(null, tileFeatureDataProviderFactory.Object);
            } 
            catch (System.ArgumentNullException ex)
            {
                if (ex.ParamName == GetTileFeatureDataTests.DbContextParameterName)
                {
                    exceptionHandled = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionHandled);
        }


        /// <summary>
        /// Tests constructor when tileFeatureDataProviderFactory is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_TileFeatureDataProviderFactoryNull()
        {
            // Arrange                       
            TileFeatureDataDbContext mockTileFeatureDataContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(
                GetTileFeatureDataTests.MinX,
                GetTileFeatureDataTests.MinY,
                GetTileFeatureDataTests.MaxX,
                GetTileFeatureDataTests.MaxY).Object;

            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(mockTileFeatureDataContext);

            bool exceptionHandled = false;

            // Act
            try
            {
                GetExtentFeatureData getTileFunction = new GetExtentFeatureData(dbContextFactory, null);
            }
            catch (System.ArgumentNullException ex)
            {
                if (ex.ParamName == GetTileFeatureDataTests.TileFeatureDataProviderFactoryParameterName)
                {
                    exceptionHandled = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionHandled);
        }

        #endregion Constructor Tests

        #region Run Tests

        /// <summary>
        /// Tests the run method happy path
        /// </summary>
        [TestMethod]
        public void Test_Run()
        {
            // Arrange           
            FeatureDataResponse response = WebMappingMockFactory.CreateMockFeatureDataResponse(GetTileFeatureDataTests.MockLayerId);
            Mock<ITileFeatureDataProvider> mockTileFeatureDataProvider = WebMappingMockFactory.CreateMockTileFeatureDataProvider(response);
            Mock<ITileFeatureDataProviderFactory> tileFeatureDataProviderFactory = WebMappingMockFactory.GetMockTileProviderFactory(mockTileFeatureDataProvider.Object);
            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(
                GetTileFeatureDataTests.MinX,
                GetTileFeatureDataTests.MinY,
                GetTileFeatureDataTests.MaxX,
                GetTileFeatureDataTests.MaxY).Object;

            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            GetExtentFeatureData getFeatureDataFunction = new GetExtentFeatureData(dbContextFactory, tileFeatureDataProviderFactory.Object);
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();

            // Act
            JsonResult result = (JsonResult)getFeatureDataFunction.Run(
                httpRequest.Object,
                GetTileFeatureDataTests.MockLayerId,
                GetTileFeatureDataTests.ZoomLevel,
                GetTileFeatureDataTests.MinX,
                GetTileFeatureDataTests.MinY,
                GetTileFeatureDataTests.MaxX,
                GetTileFeatureDataTests.MaxY,
                logger.Object).Result;

            // Assert
            Assert.ReferenceEquals(response, result.Value);
            mockTileFeatureDataProvider.Verify(m => m.GetTileFeatureData(
                It.Is<Extent>(ex => 
                    (ex.Min.Lon == GetTileFeatureDataTests.MinX) &&
                    (ex.Min.Lat == GetTileFeatureDataTests.MinY) &&
                    (ex.Max.Lon == GetTileFeatureDataTests.MaxX) &&
                    (ex.Max.Lat == GetTileFeatureDataTests.MaxY)),
                It.Is<int>(zoom => zoom == GetTileFeatureDataTests.ZoomLevel),
                It.Is<string>(layerId => layerId == GetTileFeatureDataTests.MockLayerId),
                It.Is<string[]>(columns => columns == null),
                It.Is<string>(datasetId => datasetId == null),
                It.Is<string>(filterDatasetId => filterDatasetId == null)));
        }

        ///// <summary>
        ///// Tests the run method in the case a tile feature data provider exception is thrown
        ///// </summary>
        [TestMethod]
        public void Test_Run_TileFeatureDataProviderException()
        {
            // Arrange
            FeatureDataResponse response = WebMappingMockFactory.CreateMockFeatureDataResponse(GetTileFeatureDataTests.MockLayerId);

            Mock<ITileFeatureDataProvider> mockTileFeatureDataProvider = new Mock<ITileFeatureDataProvider>(MockBehavior.Strict);

            Exception chainedException = new Exception();

            mockTileFeatureDataProvider.Setup(m => m.GetTileFeatureData(
                It.IsAny<Extent>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string[]>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Throws(new TileFeatureDataProviderException(
                    TileFeatureDataProviderExceptionCategory.SqlServerError, 
                    mockTileFeatureDataProvider.Object.GetType(),
                    GetTileFeatureDataTests.MockErrorMessage,
                    chainedException));

            Mock<ITileFeatureDataProviderFactory> tileFeatureDataProviderFactory = WebMappingMockFactory.GetMockTileProviderFactory(mockTileFeatureDataProvider.Object);
            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(
                GetTileFeatureDataTests.MinX,
                GetTileFeatureDataTests.MinY,
                GetTileFeatureDataTests.MaxX,
                GetTileFeatureDataTests.MaxY).Object;

            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            GetExtentFeatureData getFeatureDataFunction = new GetExtentFeatureData(dbContextFactory, tileFeatureDataProviderFactory.Object);
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();

            // Act
            ObjectResult result = (ObjectResult)getFeatureDataFunction.Run(
               httpRequest.Object,
               GetTileFeatureDataTests.MockLayerId,
               GetTileFeatureDataTests.ZoomLevel,
               GetTileFeatureDataTests.MinX,
               GetTileFeatureDataTests.MinY,
               GetTileFeatureDataTests.MaxX,
               GetTileFeatureDataTests.MaxY,
               logger.Object).Result;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        /// <summary>
        /// Tests the run method in the case an unexpected exception is thrown
        /// </summary>
        [TestMethod]
        public void Test_Run_UnexpectedException()
        {
            FeatureDataResponse response = WebMappingMockFactory.CreateMockFeatureDataResponse(GetTileFeatureDataTests.MockLayerId);

            Mock<ITileFeatureDataProvider> mockTileFeatureDataProvider = new Mock<ITileFeatureDataProvider>(MockBehavior.Strict);

            Exception chainedException = new Exception();

            mockTileFeatureDataProvider.Setup(m => m.GetTileFeatureData(
                It.IsAny<Extent>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string[]>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Throws(new Exception());

            Mock<ITileFeatureDataProviderFactory> tileFeatureDataProviderFactory = WebMappingMockFactory.GetMockTileProviderFactory(mockTileFeatureDataProvider.Object);
            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(
                GetTileFeatureDataTests.MinX,
                GetTileFeatureDataTests.MinY,
                GetTileFeatureDataTests.MaxX,
                GetTileFeatureDataTests.MaxY).Object;

            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            GetExtentFeatureData getFeatureDataFunction = new GetExtentFeatureData(dbContextFactory, tileFeatureDataProviderFactory.Object);
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();

            // Act
            ObjectResult result = (ObjectResult)getFeatureDataFunction.Run(
               httpRequest.Object,
               GetTileFeatureDataTests.MockLayerId,
               GetTileFeatureDataTests.ZoomLevel,
               GetTileFeatureDataTests.MinX,
               GetTileFeatureDataTests.MinY,
               GetTileFeatureDataTests.MaxX,
               GetTileFeatureDataTests.MaxY,
               logger.Object).Result;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion Run Tests
    }
}
