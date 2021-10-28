using PTASMapTileServicesLibrary.TileProvider;
using PTASMapTileServicesLibrary.TileProvider.Blob;
using PTASMapTileServicesLibrary.TileProvider.MapServer;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using Moq.Protected;
using PTASServicesCommon.CloudStorage;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PTASMapTileServicesLibrary.TileFeatureDataProvider;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
using Microsoft.EntityFrameworkCore;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
using PTASMapTileServicesLibrary.Geography.Data;
using PTASServicesCommon.FileSystem;
using PTASServicesCommon.TokenProvider;
using Microsoft.Extensions.Logging.Console;
using PTASTileStorageWorkerLibrary.SystemProcess;
using System.Diagnostics;
using PTASServicesCommon.DependencyInjection;
using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.ApplicationInsights;

namespace PTASWebMappingUnitTestCommon
{
    public static class WebMappingMockFactory
    {
        public const string DefaultToken = "DefaultToken";
        public static ILogger DefaultLogger = null;

        public static Mock<IServiceTokenProvider> CreateMockTokenProvider(string token = null)
        {
            token = token ?? WebMappingMockFactory.DefaultToken;

            Mock<IServiceTokenProvider> toReturn = new Mock<IServiceTokenProvider>(MockBehavior.Loose);
            toReturn.Setup(m => m.GetAccessTokenAsync(It.IsAny<string>())).ReturnsAsync(token);
            return toReturn;
        }

        public static Mock<ICloudStorageSharedSignatureProvider> CreateMockSharedSignatureProvider(string signature)
        {
            Mock<ICloudStorageSharedSignatureProvider> toReturn = new Mock<ICloudStorageSharedSignatureProvider>(MockBehavior.Loose);
            toReturn.Setup(m => m.GetSharedSignature(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(signature);
            return toReturn;
        }

        public static Mock<IProcessFactory> CreateMockProcessFactory(Mock<IProcess> createdProcess = null)
        {
            if (createdProcess == null)
            {
                createdProcess = WebMappingMockFactory.CreateMockProcess();
            }
            Mock<IProcessFactory> toReturn = new Mock<IProcessFactory>(MockBehavior.Strict);
            toReturn.Setup(m => m.CreateProcess(It.IsAny<ProcessStartInfo>())).Returns(createdProcess.Object);
            return toReturn;
        }

        public static Mock<IProcess> CreateMockProcess()
        {
            Mock<IProcess> toReturn = new Mock<IProcess>(MockBehavior.Strict);
            toReturn.Setup(m => m.Start());
            toReturn.Setup(m => m.WaitForExit());
            return toReturn;
        }

        public static Mock<IFileSystemProvider> CreateMockFileSystemProvider(string fileContent)
        {
            Mock<IFileSystemProvider> toReturn = new Mock<IFileSystemProvider>(MockBehavior.Loose);
            toReturn.Setup(m => m.ReadAllTextAsync(It.IsAny<string>())).ReturnsAsync(fileContent);
            return toReturn;
        }

        public static Mock<IBlobTileConfigurationProvider> CreateMockBlobTileConfigurationProvider(string containerName, string tilePathMask)
        {
            Mock<IBlobTileConfigurationProvider> toReturn = new Mock<IBlobTileConfigurationProvider>(MockBehavior.Strict);
            toReturn.Setup(m => m.TileContainerName).Returns(containerName);
            toReturn.Setup(m => m.TilePathMask).Returns(tilePathMask);
            return toReturn;
        }

        public static Mock<IMapServerLayerConfigurationProvider> CreateMockMapServerLayerConfigurationProvider(string mapServerUrl, KeyValuePair<string, string>[] layers, bool isRasterLayerProvider)
        {
            Mock<IMapServerLayerConfigurationProvider> toReturn = new Mock<IMapServerLayerConfigurationProvider>(MockBehavior.Strict);
            Dictionary<string, string> layerDictionary = new Dictionary<string, string>();
            foreach (var pair in layers)
            {
                layerDictionary.Add(pair.Key, pair.Value);
            }

            toReturn.Setup(m => m.IsRasterLayerProvider).Returns(isRasterLayerProvider);
            toReturn.Setup(m => m.MapServerUrl).Returns(mapServerUrl);
            toReturn.Setup(m => m.GetMapFileForLayerId(It.IsAny<string>()))
                .Returns((string layerId) => { return (null, layerDictionary[layerId]); });
            return toReturn;
        }

        public static Mock<ICloudStorageProvider> CreateMockCloudStorageProvider(CloudBlobContainer blobContainer)
        {
            Mock<ICloudStorageProvider> toReturn = new Mock<ICloudStorageProvider>(MockBehavior.Strict);
            toReturn.Setup(m => m.GetCloudBlobContainer(It.IsAny<string>(), It.IsAny<ClientCredential>())).ReturnsAsync(blobContainer);
            return toReturn;
        }

        public static Mock<ICloudStorageConfigurationProvider> CreateMockCloudStorageConfigurationProvider(string storageConnectionString)
        {
            Mock<ICloudStorageConfigurationProvider> toReturn = new Mock<ICloudStorageConfigurationProvider>(MockBehavior.Strict);
            toReturn.Setup(m => m.StorageConnectionString).Returns(storageConnectionString);
            return toReturn;
        }

        public static Mock<ILogger> CreateMockLogger()
        {
            // Logger is created without strict mock behavior because extension methods can't be verified
            Mock<ILogger> toReturn = new Mock<ILogger>();
            return toReturn;
        }
        public static ILogger CreateConsoleLogger()
        {
            if (WebMappingMockFactory.DefaultLogger == null)
            {
                Log.Logger = new LoggerConfiguration()
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .CreateLogger();

                SerilogLoggerProvider serilogLoggerProvider = new SerilogLoggerProvider(Log.Logger);
                WebMappingMockFactory.DefaultLogger = serilogLoggerProvider.CreateLogger("ConsoleLogger");
            }

            return WebMappingMockFactory.DefaultLogger;
        }
        public static Mock<ITileProvider> CreateMockTileProvider()
        {
            Mock<ITileProvider> toReturn = new Mock<ITileProvider>(MockBehavior.Strict);
            return toReturn;
        }

        public static byte[] CreateMockByteArray(int size, int seed)
        {
            byte[] toReturn = new byte[size];
            for (int i = 0; i < size; i++)
            {
                toReturn[i] = (byte) ((i + seed) % 255);
            }
            
            return toReturn;
        }

        public static KeyValuePair<string, string>[] CreateMockLayersConfiguration()
        {
            KeyValuePair<string, string>[] toReturn = new KeyValuePair<string, string>[2];
            toReturn[0] = new KeyValuePair<string, string>("contour", "ContourMapTest.map");
            toReturn[1] = new KeyValuePair<string, string>("parcel", "Parcel_GPKG.map");
            return toReturn;
        }

        public static Mock<HttpMessageHandler> CreateMockHttpMessageHandler_Exception(System.Exception exception)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ThrowsAsync(exception);

            return handlerMock;
        }

        public static Mock<HttpMessageHandler> CreateMockHttpMessageHandler_String(HttpStatusCode statusCode, string content)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(             
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = statusCode,
                   Content = new StringContent(content),
               });            

            return handlerMock;
        }

        public static Mock<HttpMessageHandler> CreateMockHttpMessageHandler_Binary(HttpStatusCode statusCode, byte[] content)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = statusCode,
                   Content = new ByteArrayContent(content),
               });

            return handlerMock;
        }

        public static Mock<ITileProviderFactory> GetMockTileProviderFactory(ITileProvider blobTileProvider, ITileProvider mapServerTileProvider)
        {
            Mock<ITileProviderFactory> toReturn = new Mock<ITileProviderFactory>(MockBehavior.Strict);

            toReturn.Setup(m => m.CreateBlobTileProvider(
                It.IsAny<ITileProvider>(),
                It.IsAny<IBlobTileConfigurationProvider>(),
                It.IsAny<TileOutputType>(),
                It.IsAny<ICloudStorageProvider>(),
                It.IsAny<ILogger>(),
                It.IsAny<TelemetryClient>())).Returns(blobTileProvider);

            toReturn.Setup(m => m.CreateMapServerTileProvider(
                It.IsAny<IMapServerLayerConfigurationProvider>(),
                It.IsAny<TileOutputType>(),
                It.IsAny<TelemetryClient>(),
                It.IsAny<HttpMessageHandler>())).Returns(mapServerTileProvider);

            return toReturn;
        }

        public static Mock<ITileFeatureDataProviderFactory> GetMockTileProviderFactory(ITileFeatureDataProvider featureDataProvider)
        {
            Mock<ITileFeatureDataProviderFactory> toReturn = new Mock<ITileFeatureDataProviderFactory>(MockBehavior.Strict);

            toReturn.Setup(m => m.CreateSqlServerTileFeatureDataProvider(
                It.IsAny<IFactory<TileFeatureDataDbContext>>(),
                It.IsAny<ILogger>(),
                It.IsAny<TelemetryClient>())).Returns(featureDataProvider);

            return toReturn;
        }

        public static Mock<ITileFeatureDataProvider> CreateMockTileFeatureDataProvider(FeatureDataResponse response)
        {
            Mock<ITileFeatureDataProvider> toReturn = new Mock<ITileFeatureDataProvider>(MockBehavior.Strict);

            toReturn.Setup(m => m.GetTileFeatureData(
                It.IsAny<Extent>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string[]>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(response);

            return toReturn;
        }

        public static FeatureDataCollection[] CreateMockFeaturesData()
        {
            return null;
        }

        public static FeatureDataResponse CreateMockFeatureDataResponse(string layerId)
        {
            return new FeatureDataResponse()
            {
                FeaturesDataCollections = WebMappingMockFactory.CreateMockFeaturesData(),
                LayerId = layerId,
            };
        }

        public static T InstantiateUnitialized<T>() where T : class
        {
            return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T)) as T;
        }
    }
}
