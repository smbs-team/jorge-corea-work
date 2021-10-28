namespace PTASServicesCommonUnitTest.CloudStorageTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Moq;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.TokenProvider;
    using PTASWebMappingUnitTestCommon;

    [TestClass]
    public class CloudStorageProviderTests
    {
        private const string MockConnectionString = "DefaultEndpointsProtocol=https;AccountName=teststorage;AccountKey=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx==;EndpointSuffix=core.windows.net";
        private const string BadConnectionString = "badConnectionString";
        private const string MockBlobContainerName = "mockContainerName";
        private const string ExpectedBlobContainerUri = "https://teststorage.blob.core.windows.net/mockContainerName";
        private const string ExpectedBlobUri = "https://teststorage.blob.core.windows.net/";
        private const string StorageConfigurationProviderParameterName = "storageConfigurationProvider";
        private const string TokenProviderParameterName = "tokenProvider";

        /// <summary>
        /// Tests the constructor when azure storage configuration provider is sent with null value
        /// </summary>
        [TestMethod]
        public void Test_Constructor_NullStorageConfigurationProvider()
        {
            // Arrange
            bool exceptionThrown = false;

            // Act
            try
            {
                Mock<IServiceTokenProvider> tokenProvider = ServicesCommonMockFactory.CreateMockTokenProvider();
                CloudStorageProvider storageProvider = new CloudStorageProvider(null, tokenProvider.Object);

            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == CloudStorageProviderTests.StorageConfigurationProviderParameterName)
                {
                    exceptionThrown = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests the constructor when the token provider is sent with null value
        /// </summary>
        [TestMethod]
        public void Test_Constructor_NullTokenProvider()
        {
            // Arrange
            bool exceptionThrown = false;

            // Act
            try
            {
                Mock<ICloudStorageConfigurationProvider> storageConfigurationProvider =
                    ServicesCommonMockFactory.CreateMockCloudStorageConfigurationProvider(CloudStorageProviderTests.MockConnectionString);
    
                CloudStorageProvider storageProvider = new CloudStorageProvider(storageConfigurationProvider.Object, null);

            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == CloudStorageProviderTests.TokenProviderParameterName)
                {
                    exceptionThrown = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests the happy path for GetCloudStorageAccount
        /// </summary>
        [TestMethod]
        public void Test_GetCloudStorageAccount()
        {
            // Arrange
            Mock<ICloudStorageConfigurationProvider> storageConfigurationProvider =
                ServicesCommonMockFactory.CreateMockCloudStorageConfigurationProvider(CloudStorageProviderTests.MockConnectionString);
            Mock<IServiceTokenProvider> tokenProvider = ServicesCommonMockFactory.CreateMockTokenProvider();
            CloudStorageProvider storageProvider = new CloudStorageProvider(storageConfigurationProvider.Object, tokenProvider.Object);

            // Act
            CloudStorageAccount storageAccount = storageProvider.GetCloudStorageAccount();

            // Assert
            storageConfigurationProvider.Verify(m => m.StorageConnectionString, Times.Once);
            Assert.AreEqual(CloudStorageProviderTests.ExpectedBlobUri, storageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri);
        }

        /// <summary>
        /// Tests the happy path for GetCloudBlobContainer
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.FormatException))]
        public void Test_GetCloudStorageAccount_FormatException()
        {
            // Arrange
            ICloudStorageConfigurationProvider storageConfigurationProvider = new CloudStorageConfigurationProvider(CloudStorageProviderTests.BadConnectionString);
            Mock<IServiceTokenProvider> tokenProvider = ServicesCommonMockFactory.CreateMockTokenProvider();
            CloudStorageProvider storageProvider = new CloudStorageProvider(storageConfigurationProvider, tokenProvider.Object);

            // Act
            storageProvider.GetCloudStorageAccount();
        }

    }
}
