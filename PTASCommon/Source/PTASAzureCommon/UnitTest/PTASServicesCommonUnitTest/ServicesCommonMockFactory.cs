using Moq;
using PTASServicesCommon.CloudStorage;
using PTASServicesCommon.TokenProvider;
using System.Threading.Tasks;

namespace PTASWebMappingUnitTestCommon
{
    public static class ServicesCommonMockFactory
    {
        public const string DefaultToken = "DefaultToken";

        public static Mock<IServiceTokenProvider> CreateMockTokenProvider(string token = null)
        {
            token = token ?? ServicesCommonMockFactory.DefaultToken;

            Mock<IServiceTokenProvider> toReturn = new Mock<IServiceTokenProvider>(MockBehavior.Loose);
            toReturn.Setup(m => m.GetAccessTokenAsync(It.IsAny<string>())).Returns(Task.FromResult(token));
            return toReturn;
        }

        public static Mock<ICloudStorageConfigurationProvider> CreateMockCloudStorageConfigurationProvider(string storageConnectionString)
        {
            Mock<ICloudStorageConfigurationProvider> toReturn = new Mock<ICloudStorageConfigurationProvider>(MockBehavior.Strict);
            toReturn.Setup(m => m.StorageConnectionString).Returns(storageConnectionString);
            return toReturn;
        }
    }
}
