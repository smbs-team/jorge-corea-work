using PTASMapTileServicesLibrary.TileProvider.Blob;
using PTASMapTileServicesLibrary.TileProvider.MapServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PTASWebMappingUnitTestCommon;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
using PTASServicesCommon.DependencyInjection;

namespace PTASMapTileServiceLibraryUnitTest.MapserverTileProviderTests
{
    [TestClass]
    public class MapServerLayerConfigurationProviderTests
    {
        private const string MapServerUrl = "http://mapserver.cloudapp.azure.net/mapserver/?map={0}&mode=tile&tilemode=gmap&tile={1}+{2}+{3}&layers={4}&map.imagetype=mvt&MAXFEATURES=65000";
        private const string MapServerUrlParameterName = "mapServerUrl";
        private const string LayersParameterName = "layers";
        private const string NotFoundLayerName = "MyLayer";

        /// <summary>
        /// Tests that the constructor stores all values passed as parameters
        /// </summary>
        [TestMethod]
        public void Test_Constructor()
        {
            // Arrange
            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;            
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            //Act
            MapServerLayerConfigurationProvider mapServerProviderConfiguration = 
                new MapServerLayerConfigurationProvider(MapServerLayerConfigurationProviderTests.MapServerUrl, "10.10.10.10", dbContextFactory, true);

            //Assert
            Assert.AreEqual(MapServerLayerConfigurationProviderTests.MapServerUrl, mapServerProviderConfiguration.MapServerUrl);
        }

        /// <summary>
        /// Tests the that the constructor throws a null argument exception when map server url is empty
        /// </summary>
        [TestMethod]
        public void Test_Constructor_MapServerUrlEmpty()
        {
            //Arrange
            bool exceptionThrown = false;
            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);


            //Act
            try
            {
                MapServerLayerConfigurationProvider tileProviderConfiguration = 
                    new MapServerLayerConfigurationProvider(string.Empty, "10.10.10.10", dbContextFactory, true);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == MapServerLayerConfigurationProviderTests.MapServerUrlParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests the GetMapFileForLayerId when the layer is not found.
        /// </summary>
        [TestMethod]
        public void Test_GetMapFileForLayerId_NotFoundLayer()
        {
            // Arrange
            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            MapServerLayerConfigurationProvider mapServerProviderConfiguration =
                new MapServerLayerConfigurationProvider(MapServerLayerConfigurationProviderTests.MapServerUrl, "10.10.10.10", dbContextFactory, true);


            //Act
            string mapFileName =
                mapServerProviderConfiguration.GetMapFileForLayerId(MapServerLayerConfigurationProviderTests.NotFoundLayerName).mapFile;

            //Assert
            Assert.IsNull(mapFileName);            
        }
    }
}