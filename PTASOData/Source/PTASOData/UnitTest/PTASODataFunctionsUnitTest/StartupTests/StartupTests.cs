namespace PTASMapTileFunctionsUnitTest.StartupTests
{
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PTASODataFunctions.Startup;
    using System;


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

            Environment.SetEnvironmentVariable("DbContextType", "PtasCamaDbContext");

            ConfigurationBuilder builder = new ConfigurationBuilder();

            //Act
            startup.Configure(hostBuilder.Object);
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
