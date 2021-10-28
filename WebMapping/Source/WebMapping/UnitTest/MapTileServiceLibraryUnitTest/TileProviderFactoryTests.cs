namespace PTASMapTileServiceLibraryUnitTest
{
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage.Blob;
    using PTASMapTileServicesLibrary.TileProvider;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASMapTileServicesLibrary.TileProvider.MapServer;
    using PTASServicesCommon.CloudStorage;
    using PTASWebMappingUnitTestCommon;
    using System.Collections.Generic;

    [TestClass]
    public class TileProviderFactoryTests
    {
        private const string ContainerName = "MockContainerName";
        private const string TilePathMask = "MockTileMask";
        private const string UriString = "https://test.com";
        private const TileOutputType DefaultOutputType = TileOutputType.PBF;

        /// <summary>
        /// Tests that a blob tile provider can be created
        /// </summary>
        [TestMethod]
        public void Test_CreateBlobTileProvider()
        {
            //Arrange
            ITileProviderFactory tileProviderFactory = new TileProviderFactory();
            ITileProvider fallbackTileProvider = WebMappingMockFactory.CreateMockTileProvider().Object;
            IBlobTileConfigurationProvider configuratioProvider = WebMappingMockFactory.CreateMockBlobTileConfigurationProvider(TileProviderFactoryTests.ContainerName, TileProviderFactoryTests.TilePathMask).Object;
            ICloudStorageProvider cloudStorageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(new CloudBlobContainer(new System.Uri(TileProviderFactoryTests.UriString))).Object;
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;

            //Act
            ITileProvider tileProvider = tileProviderFactory.CreateBlobTileProvider(
                fallbackTileProvider, 
                configuratioProvider, 
                TileProviderFactoryTests.DefaultOutputType, 
                cloudStorageProvider, logger);

            //Assert
            Assert.IsNotNull(tileProvider);
            Assert.IsInstanceOfType(tileProvider, typeof(BlobTileProvider));
        }

        /// <summary>
        /// Tests that a map server tile provider can be created
        /// </summary>
        [TestMethod]
        public void Test_CreateMapServerTileProvider()
        {
            //Arrange
            ITileProviderFactory tileProviderFactory = new TileProviderFactory();
            KeyValuePair<string,string>[] layerKeyValues = WebMappingMockFactory.CreateMockLayersConfiguration();
            IMapServerLayerConfigurationProvider configuratioProvider = WebMappingMockFactory.CreateMockMapServerLayerConfigurationProvider(TileProviderFactoryTests.UriString, layerKeyValues, true).Object;           

            //Act
            ITileProvider tileProvider = tileProviderFactory.CreateMapServerTileProvider(configuratioProvider, TileProviderFactoryTests.DefaultOutputType);

            //Assert
            Assert.IsNotNull(tileProvider);
            Assert.IsInstanceOfType(tileProvider, typeof(MapServerTileProvider));
        }
    }
}