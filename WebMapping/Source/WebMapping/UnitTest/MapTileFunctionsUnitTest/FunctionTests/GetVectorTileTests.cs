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
using PTASServicesCommon.TokenProvider;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
using PTASServicesCommon.DependencyInjection;

namespace PTASMapTileFunctionsUnitTest.FunctionTests
{
    [TestClass]
    public class GetVectorTileTests
    {
        private const string MapServerUrl = "http://mapserver.cloudapp.azure.net/mapserver/?map={0}&mode=tile&tilemode=gmap&tile={1}+{2}+{3}&layers={4}&map.imagetype=mvt&MAXFEATURES=65000";
        private const string CloudStorageConfigurationProviderParameterName = "cloudStorageConfigurationProvider";
        private const string BlobTileConfigurationProviderParameterName = "blobTileConfigurationProvider";
        private const string TileProviderFactoryParameterName = "tileProviderFactory";
        private const string BlobTileConfigurationProvider = "blobTileConfigurationProvider";
        private const string MockConnectionString = "mockConnectionString";
        private const string MockTileContainerName = "mockContainer";
        private const string MockTilePathMask = "{0}/{1}/{2}/{3}.pbf";
        private const string MockLayerId = "mockLayer";
        private const string MockErrorMessage = "mockErrorMessage";
        private const int TileX = 1;
        private const int TileY = 2;
        private const int TileZ = 3;
        private const int MockTileSize = 2048;
        private const int TileBytesSeed = 23;


        #region Constructor Tests

        /// <summary>
        /// Tests constructor when cloudStorageConfigurationProvider is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_CloudStorageConfigurationProviderNull()
        {
            // Arrange
            Mock<ICloudStorageConfigurationProvider> storageConfigurationProvider = WebMappingMockFactory.CreateMockCloudStorageConfigurationProvider(MockConnectionString);
            Mock<ITileProvider> blobTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            Mock<ITileProvider> mapServerTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            Mock<ITileProviderFactory> tileProviderFactory = WebMappingMockFactory.GetMockTileProviderFactory(blobTileProvider.Object, mapServerTileProvider.Object);
            Mock<IServiceTokenProvider> tokenProvider = WebMappingMockFactory.CreateMockTokenProvider();

            bool exceptionHandled = false;

            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);


            // Act
            try
            {
                GetVectorTile getTileFunction = new GetVectorTile(
                    storageConfigurationProvider.Object,
                    null,
                    tileProviderFactory.Object,
                    tokenProvider.Object,
                    dbContextFactory);
            } 
            catch (System.ArgumentNullException ex)
            {
                if (ex.ParamName == GetVectorTileTests.BlobTileConfigurationProviderParameterName)
                {
                    exceptionHandled = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests constructor when blobTileConfigurationProvider is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_BlobTileConfigurationProviderNull()
        {
            // Arrange
            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider = WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(MockTileContainerName, MockTilePathMask);
            Mock<ITileProvider> blobTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            Mock<ITileProvider> mapServerTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            Mock<ITileProviderFactory> tileProviderFactory = WebMappingMockFactory.GetMockTileProviderFactory(blobTileProvider.Object, mapServerTileProvider.Object);
            Mock<IServiceTokenProvider> tokenProvider = WebMappingMockFactory.CreateMockTokenProvider();

            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            bool exceptionHandled = false;

            // Act
            try
            {
                GetVectorTile getTileFunction = new GetVectorTile(
                    null,
                    blobTileConfigurationProvider.Object,
                    tileProviderFactory.Object,
                    tokenProvider.Object,
                    dbContextFactory);
            }
            catch (System.ArgumentNullException ex)
            {
                if (ex.ParamName == GetVectorTileTests.CloudStorageConfigurationProviderParameterName)
                {
                    exceptionHandled = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests constructor when tileProviderFactory is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_TileProviderFactoryNull()
        {
            // Arrange
            Mock<ICloudStorageConfigurationProvider> storageConfigurationProvider = WebMappingMockFactory.CreateMockCloudStorageConfigurationProvider(MockConnectionString);
            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider = WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(MockTileContainerName, MockTilePathMask);
            Mock<IServiceTokenProvider> tokenProvider = WebMappingMockFactory.CreateMockTokenProvider();

            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            bool exceptionHandled = false;

            // Act
            try
            {
                GetVectorTile getTileFunction = new GetVectorTile(
                    storageConfigurationProvider.Object,
                    blobTileConfigurationProvider.Object,
                    null,
                    tokenProvider.Object,
                    dbContextFactory);
            }
            catch (System.ArgumentNullException ex)
            {
                if (ex.ParamName == GetVectorTileTests.TileProviderFactoryParameterName)
                {
                    exceptionHandled = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionHandled);
        }

        #endregion

        #region GetTile Tests

        /// <summary>
        /// Tests the run method happy path
        /// </summary>
        [TestMethod]
        public async Task Test_Run()
        {
            // Arrange
            byte[] tileBytes = WebMappingMockFactory.CreateMockByteArray(GetVectorTileTests.MockTileSize, GetVectorTileTests.TileBytesSeed);
            Mock<ICloudStorageConfigurationProvider> storageConfigurationProvider = WebMappingMockFactory.CreateMockCloudStorageConfigurationProvider(MockConnectionString);
            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider = WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(MockTileContainerName, MockTilePathMask);

            Mock<ITileProvider> blobTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            blobTileProvider.Setup(m => m.GetTile(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(tileBytes);

            Mock<ITileProvider> mapServerTileProvider = WebMappingMockFactory.CreateMockTileProvider();            

            Mock<ITileProviderFactory> tileProviderFactory = WebMappingMockFactory.GetMockTileProviderFactory(blobTileProvider.Object, mapServerTileProvider.Object);
            Mock<IServiceTokenProvider> tokenProvider = WebMappingMockFactory.CreateMockTokenProvider();

            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            GetVectorTile getTileFunction = new GetVectorTile(
                storageConfigurationProvider.Object,
                blobTileConfigurationProvider.Object,
                tileProviderFactory.Object,
                tokenProvider.Object,
                dbContextFactory);

            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            ExecutionContext executionContext = new ExecutionContext();
            executionContext.FunctionAppDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Environment.SetEnvironmentVariable("MapServerUrl", GetVectorTileTests.MockTilePathMask);
            Environment.SetEnvironmentVariable("MapServerIp", "10.10.10.10");

            // Act
            FileContentResult result = (FileContentResult) await getTileFunction.Run(
                httpRequest.Object,
                GetVectorTileTests.MockLayerId,
                GetVectorTileTests.TileZ,
                GetVectorTileTests.TileY,
                GetVectorTileTests.TileX,
                logger.Object,
                executionContext);

            // Assert
            Assert.IsTrue(tileBytes.SequenceEqual(result.FileContents));
        }

        /// <summary>
        /// Tests the run method in the case a tile provider exception is thrown
        /// </summary>
        [TestMethod]
        public async Task Test_Run_TileProviderException()
        {
            // Arrange            
            Mock<ICloudStorageConfigurationProvider> storageConfigurationProvider = WebMappingMockFactory.CreateMockCloudStorageConfigurationProvider(MockConnectionString);
            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider = WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(MockTileContainerName, MockTilePathMask);

            Mock<ITileProvider> blobTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            blobTileProvider.Setup(m => m.GetTile(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Throws(
                new TileProviderException(TileProviderExceptionCategory.CloudStorageError, 
                    typeof(BlobTileProvider), GetVectorTileTests.MockErrorMessage,
                    new System.Exception()));

            Mock<ITileProvider> mapServerTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            Mock<ITileProviderFactory> tileProviderFactory = WebMappingMockFactory.GetMockTileProviderFactory(blobTileProvider.Object, mapServerTileProvider.Object);
            Mock<IServiceTokenProvider> tokenProvider = WebMappingMockFactory.CreateMockTokenProvider();

            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            GetVectorTile getTileFunction = new GetVectorTile(
                storageConfigurationProvider.Object,
                blobTileConfigurationProvider.Object,
                tileProviderFactory.Object,
                tokenProvider.Object,
                dbContextFactory);

            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            ExecutionContext executionContext = new ExecutionContext();
            executionContext.FunctionAppDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Environment.SetEnvironmentVariable("MapServerUrl", "{0}/{1}/{2}/{3}.pbf");

            // Act
            ObjectResult result = (ObjectResult)await getTileFunction.Run(
                httpRequest.Object,
                GetVectorTileTests.MockLayerId,
                GetVectorTileTests.TileZ,
                GetVectorTileTests.TileY,
                GetVectorTileTests.TileX,
                logger.Object,
                executionContext);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        /// <summary>
        /// Tests the run method in the case an unexpected exception is thrown
        /// </summary>
        [TestMethod]
        public async Task Test_Run_UnexpectedException()
        {
            // Arrange            
            Mock<ICloudStorageConfigurationProvider> storageConfigurationProvider = WebMappingMockFactory.CreateMockCloudStorageConfigurationProvider(MockConnectionString);
            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider = WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(MockTileContainerName, MockTilePathMask);

            Mock<ITileProvider> blobTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            blobTileProvider.Setup(m => m.GetTile(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Throws(
                new System.Exception());

            Mock<ITileProvider> mapServerTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            Mock<ITileProviderFactory> tileProviderFactory = WebMappingMockFactory.GetMockTileProviderFactory(blobTileProvider.Object, mapServerTileProvider.Object);
            Mock<IServiceTokenProvider> tokenProvider = WebMappingMockFactory.CreateMockTokenProvider();

            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            GetVectorTile getTileFunction = new GetVectorTile(
                storageConfigurationProvider.Object,
                blobTileConfigurationProvider.Object,
                tileProviderFactory.Object,
                tokenProvider.Object,
                dbContextFactory);

            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            ExecutionContext executionContext = new ExecutionContext();
            executionContext.FunctionAppDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Environment.SetEnvironmentVariable("MapServerUrl", "{0}/{1}/{2}/{3}.pbf");

            // Act
            ObjectResult result = (ObjectResult)await getTileFunction.Run(
                httpRequest.Object,
                GetVectorTileTests.MockLayerId,
                GetVectorTileTests.TileZ,
                GetVectorTileTests.TileY,
                GetVectorTileTests.TileX,
                logger.Object,
                executionContext);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion region
    }
}
