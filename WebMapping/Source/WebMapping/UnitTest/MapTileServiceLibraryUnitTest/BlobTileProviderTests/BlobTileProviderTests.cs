using PTASMapTileServicesLibrary.TileProvider;
using PTASMapTileServicesLibrary.TileProvider.Blob;
using PTASMapTileServicesLibrary.TileProvider.Exception;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using PTASServicesCommon.CloudStorage;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PTASWebMappingUnitTestCommon;
using PTASServicesCommon.TokenProvider;

namespace PTASMapTileServiceLibraryUnitTest.BlobTileProviderTests
{
    [TestClass]
    public class BlobTileProviderTests
    {
        private const string MockConnectionString = "mockConnectionString";
        private const string FallbackTileProviderParameterName = "fallbackTileProvider";
        private const string TileBlobConfigurationProviderParameterName = "tileBlobConfigurationProvider";
        private const string CloudStorageProviderParameterName = "cloudStorageProvider";
        private const string LoggerParameterName = "logger";       
        private const string MockTileContainerName = "mockContainer";
        private const string MockTilePathMask = "{0}/{1}/{2}/{3}.{4}";        
        private const string MockLayerId = "mockLayer";
        private const int TileX = 1;
        private const int TileY = 2;
        private const int TileZ = 3;
        private const string ResolvedTilePath = MockLayerId + "/3/2/1.pbf";
        private const int MockTileSize = 2048;
        private const int TileBytesSeed = 23;
        private const string BlobUriString = "https://windows.azure.net/fakeblob";
        private const TileOutputType DefaultOutputType = TileOutputType.PBF;

        #region "Constructor Tests"       

        /// <summary>
        /// Tests the that the constructor throws a null exception when the tile blob configuration provider is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_TileBlobConfigurationProviderNull()
        {
            //Arrange
            bool exceptionThrown = false;
            ITileProvider fallbackTileProvider = WebMappingMockFactory.CreateMockTileProvider().Object;
            ICloudStorageProvider cloudStorageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(null).Object;
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;

            //Act
            try
            {
                BlobTileProvider tileProvider = new BlobTileProvider(
                    fallbackTileProvider,
                    null,
                    BlobTileProviderTests.DefaultOutputType,
                    cloudStorageProvider,
                    logger);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == BlobTileProviderTests.TileBlobConfigurationProviderParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests the that the constructor throws a null exception when the azure storage provider is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_CloudStorageProviderNull()
        {
            //Arrange
            bool exceptionThrown = false;
            ITileProvider fallbackTileProvider = WebMappingMockFactory.CreateMockTileProvider().Object;
            IBlobTileConfigurationProvider blobTileConfigurationProvider =
                WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(BlobTileProviderTests.MockTileContainerName, BlobTileProviderTests.MockTilePathMask).Object;

            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;

            //Act
            try
            {
                BlobTileProvider tileProvider = new BlobTileProvider(
                    fallbackTileProvider,
                    blobTileConfigurationProvider,
                    BlobTileProviderTests.DefaultOutputType,
                    null,
                    logger);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == BlobTileProviderTests.CloudStorageProviderParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests the that the constructor throws a null exception when the logger is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_LoggerNull()
        {
            //Arrange
            bool exceptionThrown = false;
            ITileProvider fallbackTileProvider = WebMappingMockFactory.CreateMockTileProvider().Object;
            IBlobTileConfigurationProvider blobTileConfigurationProvider =
                WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(BlobTileProviderTests.MockTileContainerName, BlobTileProviderTests.MockTilePathMask).Object;
            ICloudStorageProvider cloudStorageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(null).Object;            

            //Act
            try
            {
                BlobTileProvider tileProvider = new BlobTileProvider(
                    fallbackTileProvider,
                    blobTileConfigurationProvider,
                    BlobTileProviderTests.DefaultOutputType,
                    cloudStorageProvider,
                    null);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == BlobTileProviderTests.LoggerParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        #endregion

        #region "GetTile Tests"

        /// <summary>
        /// Tests the get tile method, happy path
        /// </summary>
        //[TestMethod]
        public async Task Test_GetTile()
        {
            //Arrange
            Mock<ITileProvider> fallbackTileProvider = WebMappingMockFactory.CreateMockTileProvider();

            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider =
                WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(BlobTileProviderTests.MockTileContainerName, BlobTileProviderTests.MockTilePathMask);            

            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
            byte[] tileBytes = WebMappingMockFactory.CreateMockByteArray(BlobTileProviderTests.MockTileSize, BlobTileProviderTests.TileBytesSeed);

            System.Uri blobUri = new System.Uri(BlobTileProviderTests.BlobUriString);

            Mock<CloudBlockBlob> blockBlob = new Mock<CloudBlockBlob>(blobUri);
            blockBlob.Setup(m => m.ExistsAsync()).ReturnsAsync(true);
            blockBlob.Setup(m => m.DownloadToStreamAsync(It.IsAny<Stream>())).Callback((Stream stream) =>
                {
                    stream.Position = 0;
                    stream.Write(tileBytes, 0, tileBytes.Length);
                });

            Mock<CloudBlobContainer> blobContainer = new Mock<CloudBlobContainer>(blobUri);            
            blobContainer.Setup(m => m.GetBlockBlobReference(It.IsAny<string>())).Returns(blockBlob.Object);
            Mock<ICloudStorageProvider> cloudStorageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(blobContainer.Object);

            BlobTileProvider tileProvider = new BlobTileProvider(
                fallbackTileProvider.Object,
                blobTileConfigurationProvider.Object,
                BlobTileProviderTests.DefaultOutputType,
                cloudStorageProvider.Object,
                logger);

            //Act
            byte[] tileContents = await tileProvider.GetTile(
                BlobTileProviderTests.TileX,
                BlobTileProviderTests.TileY,
                BlobTileProviderTests.TileZ, 
                BlobTileProviderTests.MockLayerId);

            //Assert
            blobTileConfigurationProvider.Verify(m => m.TilePathMask, Times.Once);
            blobTileConfigurationProvider.Verify(m => m.TileContainerName, Times.Once);
            fallbackTileProvider.Verify(m => m.GetTile(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            blockBlob.Verify(m => m.ExistsAsync(), Times.Once);
            blockBlob.Verify(m => m.DownloadToStreamAsync(It.IsAny<Stream>()), Times.Once);
            blobContainer.Verify(m => m.GetBlockBlobReference(It.Is<string>(arg => arg == BlobTileProviderTests.ResolvedTilePath)), Times.Once);            

            Assert.IsTrue(tileBytes.SequenceEqual(tileContents));
        }

        /// <summary>
        /// Tests the get tile method, when it needs to use the fall-back tile provider
        /// </summary>
        //[TestMethod]
        public async Task Test_GetTile_Fallback()
        {
            //Arrange
            byte[] tileBytes = WebMappingMockFactory.CreateMockByteArray(BlobTileProviderTests.MockTileSize, BlobTileProviderTests.TileBytesSeed);

            Mock<ITileProvider> fallbackTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            fallbackTileProvider.Setup(m => m.GetTile(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(tileBytes);

            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider =
                WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(BlobTileProviderTests.MockTileContainerName, BlobTileProviderTests.MockTilePathMask);
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;

            System.Uri blobUri = new System.Uri(BlobTileProviderTests.BlobUriString);

            Mock<CloudBlockBlob> blockBlob = new Mock<CloudBlockBlob>(blobUri);
            blockBlob.Setup(m => m.ExistsAsync()).ReturnsAsync(false);
            blockBlob.Setup(m => m.UploadFromByteArrayAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()));

            Mock<CloudBlobContainer> blobContainer = new Mock<CloudBlobContainer>(blobUri);
            blobContainer.Setup(m => m.GetBlockBlobReference(It.IsAny<string>())).Returns(blockBlob.Object);
            Mock<ICloudStorageProvider> cloudStorageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(blobContainer.Object);

            BlobTileProvider tileProvider = new BlobTileProvider(
                fallbackTileProvider.Object,
                blobTileConfigurationProvider.Object,
                BlobTileProviderTests.DefaultOutputType,
                cloudStorageProvider.Object,
                logger);

            //Act
            byte[] tileContents = await tileProvider.GetTile(
                BlobTileProviderTests.TileX,
                BlobTileProviderTests.TileY,
                BlobTileProviderTests.TileZ,
                BlobTileProviderTests.MockLayerId);


            //Assert
            blobTileConfigurationProvider.Verify(m => m.TilePathMask, Times.Once);
            blobTileConfigurationProvider.Verify(m => m.TileContainerName, Times.Once);
            blockBlob.Verify(m => m.ExistsAsync(), Times.Once);
            blobContainer.Verify(m => m.GetBlockBlobReference(It.Is<string>(arg => arg == BlobTileProviderTests.ResolvedTilePath)), Times.Once);

            fallbackTileProvider.Verify(m => m.GetTile(
                It.Is<int>(arg => arg == BlobTileProviderTests.TileX),
                It.Is<int>(arg => arg == BlobTileProviderTests.TileY),
                It.Is<int>(arg => arg == BlobTileProviderTests.TileZ),
                It.Is<string>(arg => arg == BlobTileProviderTests.MockLayerId)), Times.Once);

            blockBlob.Verify(m => m.UploadFromByteArrayAsync(
                It.Is<byte[]>(arg => arg.SequenceEqual(tileBytes)),
                It.Is<int>(arg => arg == 0),
                It.Is<int>(arg => arg == tileBytes.Length)), Times.Once);

            Assert.IsTrue(tileBytes.SequenceEqual(tileContents));
        }

        /// <summary>
        /// Tests the get tile method, when an invalid configuration string
        /// </summary>
        [TestMethod]        
        public async Task Test_GetTile_InvalidConnectionStringException()
        {
            //Arrange
            Mock<ITileProvider> fallbackTileProvider = WebMappingMockFactory.CreateMockTileProvider();

            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider =
                WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(BlobTileProviderTests.MockTileContainerName, BlobTileProviderTests.MockTilePathMask);

            ICloudStorageConfigurationProvider storageConfigurationProvider = new CloudStorageConfigurationProvider(BlobTileProviderTests.MockConnectionString);
            Mock<IServiceTokenProvider> tokenProvider = WebMappingMockFactory.CreateMockTokenProvider();
            ICloudStorageProvider cloudStorageProvider = new CloudStorageProvider(storageConfigurationProvider, tokenProvider.Object);
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;

            BlobTileProvider tileProvider = new BlobTileProvider(
                fallbackTileProvider.Object,
                blobTileConfigurationProvider.Object,
                BlobTileProviderTests.DefaultOutputType,
                cloudStorageProvider,
                logger);

            bool exceptionHandled = false;

            //Act
            try
            {               
                byte[] tileContents = await tileProvider.GetTile(
                    BlobTileProviderTests.TileX,
                    BlobTileProviderTests.TileY,
                    BlobTileProviderTests.TileZ,
                    BlobTileProviderTests.MockLayerId);
            }
            catch (TileProviderException tileProviderException)
            {
                exceptionHandled =
                    tileProviderException.TileProviderType == typeof(BlobTileProvider) &&
                    tileProviderException.TileProviderExceptionCategory == TileProviderExceptionCategory.InvalidConfiguration;
            }


            //Assert
            Assert.IsTrue(exceptionHandled);
            blobTileConfigurationProvider.Verify(m => m.TileContainerName, Times.Once);
        }

        /// <summary>
        /// Tests the get tile method, when it upload to blob fails
        /// </summary>
        //[TestMethod]
        public async Task Test_GetTile_UploadToBlobStorageException()
        {
            //Arrange
            byte[] tileBytes = WebMappingMockFactory.CreateMockByteArray(BlobTileProviderTests.MockTileSize, BlobTileProviderTests.TileBytesSeed);

            Mock<ITileProvider> fallbackTileProvider = WebMappingMockFactory.CreateMockTileProvider();
            fallbackTileProvider.Setup(m => m.GetTile(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(tileBytes);

            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider =
                WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(BlobTileProviderTests.MockTileContainerName, BlobTileProviderTests.MockTilePathMask);
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();

            System.Uri blobUri = new System.Uri(BlobTileProviderTests.BlobUriString);

            Mock<CloudBlockBlob> blockBlob = new Mock<CloudBlockBlob>(blobUri);
            blockBlob.Setup(m => m.ExistsAsync()).ReturnsAsync(false);
            blockBlob.Setup(m => m.UploadFromByteArrayAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new StorageException());

            Mock<CloudBlobContainer> blobContainer = new Mock<CloudBlobContainer>(blobUri);
            blobContainer.Setup(m => m.GetBlockBlobReference(It.IsAny<string>())).Returns(blockBlob.Object);
            Mock<ICloudStorageProvider> cloudStorageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(blobContainer.Object);

            BlobTileProvider tileProvider = new BlobTileProvider(
                fallbackTileProvider.Object,
                blobTileConfigurationProvider.Object,
                BlobTileProviderTests.DefaultOutputType,
                cloudStorageProvider.Object,
                logger.Object);

            //Act
            byte[] tileContents = await tileProvider.GetTile(
                BlobTileProviderTests.TileX,
                BlobTileProviderTests.TileY,
                BlobTileProviderTests.TileZ,
                BlobTileProviderTests.MockLayerId);

            //Assert
            blobTileConfigurationProvider.Verify(m => m.TilePathMask, Times.Once);
            blobTileConfigurationProvider.Verify(m => m.TileContainerName, Times.Once);
            blockBlob.Verify(m => m.ExistsAsync(), Times.Once);            
            blobContainer.Verify(m => m.GetBlockBlobReference(It.Is<string>(arg => arg == BlobTileProviderTests.ResolvedTilePath)), Times.Once);

            fallbackTileProvider.Verify(m => m.GetTile(
                It.Is<int>(arg => arg == BlobTileProviderTests.TileX),
                It.Is<int>(arg => arg == BlobTileProviderTests.TileY),
                It.Is<int>(arg => arg == BlobTileProviderTests.TileZ),
                It.Is<string>(arg => arg == BlobTileProviderTests.MockLayerId)), Times.Once);

            blockBlob.Verify(m => m.UploadFromByteArrayAsync(
                It.Is<byte[]>(arg => arg.SequenceEqual(tileBytes)),
                It.Is<int>(arg => arg == 0),
                It.Is<int>(arg => arg == tileBytes.Length)), Times.Once);

            Assert.IsTrue(tileBytes.SequenceEqual(tileContents));
        }

        /// <summary>
        /// Tests the get tile method when there's an error while downloading from the blob
        /// </summary>
        //[TestMethod]
        public async Task Test_GetTile_StorageExceptionDownload()
        {
            //Arrange
            Mock<ITileProvider> fallbackTileProvider = WebMappingMockFactory.CreateMockTileProvider();

            Mock<IBlobTileConfigurationProvider> blobTileConfigurationProvider =
                WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(BlobTileProviderTests.MockTileContainerName, BlobTileProviderTests.MockTilePathMask);

            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
            byte[] tileBytes = WebMappingMockFactory.CreateMockByteArray(BlobTileProviderTests.MockTileSize, BlobTileProviderTests.TileBytesSeed);

            System.Uri blobUri = new System.Uri(BlobTileProviderTests.BlobUriString);

            Mock<CloudBlockBlob> blockBlob = new Mock<CloudBlockBlob>(blobUri);
            blockBlob.Setup(m => m.ExistsAsync()).ReturnsAsync(true);
            blockBlob.Setup(m => m.DownloadToStreamAsync(It.IsAny<Stream>())).Throws(new StorageException());

            Mock<CloudBlobContainer> blobContainer = new Mock<CloudBlobContainer>(blobUri);
            blobContainer.Setup(m => m.GetBlockBlobReference(It.IsAny<string>())).Returns(blockBlob.Object);
            Mock<ICloudStorageProvider> cloudStorageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(blobContainer.Object);

            BlobTileProvider tileProvider = new BlobTileProvider(
                fallbackTileProvider.Object,
                blobTileConfigurationProvider.Object,
                BlobTileProviderTests.DefaultOutputType,
                cloudStorageProvider.Object,
                logger);

            bool exceptionHandled = false;

            //Act
            try
            {
                byte[] tileContents = await tileProvider.GetTile(
                BlobTileProviderTests.TileX,
                BlobTileProviderTests.TileY,
                BlobTileProviderTests.TileZ,
                BlobTileProviderTests.MockLayerId);
            }
            catch (TileProviderException tileProviderException)
            {
                exceptionHandled =
                   tileProviderException.TileProviderType == typeof(BlobTileProvider) &&
                   tileProviderException.TileProviderExceptionCategory == TileProviderExceptionCategory.CloudStorageError;
            }


            //Assert
            Assert.IsTrue(exceptionHandled);
            blobTileConfigurationProvider.Verify(m => m.TilePathMask, Times.Once);
            blobTileConfigurationProvider.Verify(m => m.TileContainerName, Times.Once);
            fallbackTileProvider.Verify(m => m.GetTile(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            blockBlob.Verify(m => m.ExistsAsync(), Times.Once);            
            blobContainer.Verify(m => m.GetBlockBlobReference(It.Is<string>(arg => arg == BlobTileProviderTests.ResolvedTilePath)), Times.Once);
        }

        #endregion
    }
}