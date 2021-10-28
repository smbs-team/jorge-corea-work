using PTASMapTileServicesLibrary.TileProvider;
using PTASMapTileServicesLibrary.TileProvider.Blob;
using PTASMapTileServicesLibrary.TileProvider.Exception;
using PTASMapTileServicesLibrary.TileProvider.MapServer;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using PTASServicesCommon.CloudStorage;
using System.IO;
using System.Linq;
using Moq.Protected;
using System.Threading.Tasks;
using PTASWebMappingUnitTestCommon;
using System.Net.Http;
using System.Threading;
using System.Net;

namespace PTASMapTileServiceLibraryUnitTest.MapserverTileProviderTests
{
    [TestClass]
    public class MapServerTileProviderTests
    {
        private const string MapServerUrl = "http://mapserver.cloudapp.azure.net/mapserver/?map={0}&mode=tile&tilemode=gmap&tile={1}+{2}+{3}&layers={4}&map.imagetype=mvt&MAXFEATURES=65000";
        private const string MapServerLayerConfigurationProviderParameterName = "mapServerLayerConfigurationProvider";
        private const string MockLayerId = "parcel";
        private const string BadRequestMessage = "Bad request mock message";
        private const int TileX = 1;
        private const int TileY = 2;
        private const int TileZ = 3;
        private const int MockTileSize = 2048;
        private const int TileBytesSeed = 23;
        private const TileOutputType DefaultOutputType = TileOutputType.PBF;

        #region "Constructor Tests"

        /// <summary>
        /// Tests the that the constructor throws a null exception when the fall-back tile provider is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_ConfigurationProviderNull()
        {
            //Arrange
            bool exceptionThrown = false;            

            //Act
            try
            {
                MapServerTileProvider tileProvider = new MapServerTileProvider(null, MapServerTileProviderTests.DefaultOutputType);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == MapServerTileProviderTests.MapServerLayerConfigurationProviderParameterName)
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
        [TestMethod]
        public async Task Test_GetTile()
        {
            //Arrange
            Mock<IMapServerLayerConfigurationProvider> layersConfigurationProviderMock =
                WebMappingMockFactory.CreateMockMapServerLayerConfigurationProvider(
                    MapServerTileProviderTests.MapServerUrl,
                    WebMappingMockFactory.CreateMockLayersConfiguration(),
                    true);

            byte[] tileBytes = WebMappingMockFactory.CreateMockByteArray(MapServerTileProviderTests.MockTileSize, MapServerTileProviderTests.TileBytesSeed);

            var httpMessageHandlerMock = WebMappingMockFactory.CreateMockHttpMessageHandler_Binary(HttpStatusCode.OK, tileBytes);            


            MapServerTileProvider tileProvider = new MapServerTileProvider(
                layersConfigurationProviderMock.Object,
                MapServerTileProviderTests.DefaultOutputType,
                null,
                httpMessageHandlerMock.Object);            

            //Act
            byte[] tileContents = await tileProvider.GetTile(
                MapServerTileProviderTests.TileX,
                MapServerTileProviderTests.TileY,
                MapServerTileProviderTests.TileZ,
                MapServerTileProviderTests.MockLayerId);

            //Assert
            layersConfigurationProviderMock.Verify(m => m.MapServerUrl, Times.Once);
            Assert.IsTrue(tileBytes.SequenceEqual(tileContents));
        }

        /// <summary>
        /// Tests the get tile method, when there is an http expcetion.
        /// </summary>
        [TestMethod]
        public async Task TestGetTile_HttpException()
        {
            //Arrange
            Mock<IMapServerLayerConfigurationProvider> layersConfigurationProviderMock =
                WebMappingMockFactory.CreateMockMapServerLayerConfigurationProvider(
                    MapServerTileProviderTests.MapServerUrl,
                    WebMappingMockFactory.CreateMockLayersConfiguration(),
                    true);

            byte[] tileBytes = WebMappingMockFactory.CreateMockByteArray(MapServerTileProviderTests.MockTileSize, MapServerTileProviderTests.TileBytesSeed);

            var httpMessageHandlerMock = WebMappingMockFactory.CreateMockHttpMessageHandler_Exception(new HttpRequestException());

            MapServerTileProvider tileProvider = new MapServerTileProvider(
                layersConfigurationProviderMock.Object,
                MapServerTileProviderTests.DefaultOutputType,
                null,
                httpMessageHandlerMock.Object);

            bool exceptionHandled = false;

            //Act
            try
            {
                byte[] tileContents = await tileProvider.GetTile(
                    MapServerTileProviderTests.TileX,
                    MapServerTileProviderTests.TileY,
                    MapServerTileProviderTests.TileZ,
                    MapServerTileProviderTests.MockLayerId);
            }
            catch (TileProviderException tileProviderException)
            {
                exceptionHandled =
                    tileProviderException.TileProviderType == typeof(MapServerTileProvider) &&
                    tileProviderException.TileProviderExceptionCategory == TileProviderExceptionCategory.HttpRequestSendError;
            }

            //Assert            
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests the get tile method, happy path
        /// </summary>
        [TestMethod]
        public async Task Test_GetTile_BadRequestStatusCode()
        {
            //Arrange
            Mock<IMapServerLayerConfigurationProvider> layersConfigurationProviderMock =
                WebMappingMockFactory.CreateMockMapServerLayerConfigurationProvider(
                    MapServerTileProviderTests.MapServerUrl,
                    WebMappingMockFactory.CreateMockLayersConfiguration(),
                    true);

            byte[] tileBytes = WebMappingMockFactory.CreateMockByteArray(MapServerTileProviderTests.MockTileSize, MapServerTileProviderTests.TileBytesSeed);

            var httpMessageHandlerMock = WebMappingMockFactory.CreateMockHttpMessageHandler_String(HttpStatusCode.BadRequest, MapServerTileProviderTests.BadRequestMessage);

            MapServerTileProvider tileProvider = new MapServerTileProvider(
                layersConfigurationProviderMock.Object,
                MapServerTileProviderTests.DefaultOutputType,
                null,
                httpMessageHandlerMock.Object);

            bool exceptionHandled = false;

            //Act
            try
            {
                byte[] tileContents = await tileProvider.GetTile(
                    MapServerTileProviderTests.TileX,
                    MapServerTileProviderTests.TileY,
                    MapServerTileProviderTests.TileZ,
                    MapServerTileProviderTests.MockLayerId);
            }
            catch (TileProviderException tileProviderException)
            {
                exceptionHandled =
                    tileProviderException.TileProviderType == typeof(MapServerTileProvider) &&
                    tileProviderException.TileProviderExceptionCategory == TileProviderExceptionCategory.ServerError &&
                    tileProviderException.Message.Contains(MapServerTileProviderTests.BadRequestMessage);
            }

            //Assert            
            Assert.IsTrue(exceptionHandled);
        }

        #endregion
    }
}