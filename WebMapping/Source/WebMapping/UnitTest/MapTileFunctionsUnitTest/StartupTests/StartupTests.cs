using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASMapTileFunctions.Startup;
using PTASMapTileServicesLibrary.TileProvider.Blob;
using PTASServicesCommon.CloudStorage;
using System;

namespace PTASMapTileFunctionsUnitTest.StartupTests
{
    [TestClass]
    public class StartupTests
    {
        /// <summary>
        /// Tests the configure method
        /// </summary>
        [TestMethod]
        public void Test_Configure()
       {
            //Arrange
            Startup startup = new Startup();
            Mock<IFunctionsHostBuilder> hostBuilder = new Mock<IFunctionsHostBuilder>();
            ServiceCollection serviceCollection = new ServiceCollection();
            hostBuilder.Setup(m => m.Services).Returns(serviceCollection);

            Environment.SetEnvironmentVariable("StorageConnectionString", "DefaultEndpointsProtocol=https;AccountName=teststorage;AccountKey=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx==;EndpointSuffix=core.windows.net");
            Environment.SetEnvironmentVariable("BlotTileContainerName", "mvtcontainer");
            Environment.SetEnvironmentVariable("BlobTilePathMask", "{0}/{1}/{2}/{3}.pbf");
            Environment.SetEnvironmentVariable("MapServerIp", "10.10.10.10");

            ConfigurationBuilder builder = new ConfigurationBuilder();

            //Act
            startup.Configure(hostBuilder.Object);

            //Assert
            Assert.IsTrue(this.ContainsServiceDescriptor(serviceCollection, typeof(ICloudStorageConfigurationProvider)));
            Assert.IsTrue(this.ContainsServiceDescriptor(serviceCollection, typeof(IBlobTileConfigurationProvider)));
        }

        private bool ContainsServiceDescriptor(ServiceCollection serviceCollection, Type type)
        {
            foreach (ServiceDescriptor descriptor in serviceCollection)
            {
                if (descriptor.ServiceType == type)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
