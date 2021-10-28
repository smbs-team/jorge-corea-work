namespace PTASServicesCommonUnitTest.CloudStorageTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASServicesCommon.CloudStorage;

    [TestClass]
    public class CloudStorageConfigurationTests
    {
        private const string MockConnectionString = "mockConnectionString";
        private const string StorageConnectionStringParameterName = "storageConnectionString";

        /// <summary>
        /// Tests that the constructor properly stores configuration values
        /// </summary>
        [TestMethod]
        public void Test_Constructor()
        {
            CloudStorageConfigurationProvider storageConfiguration = new CloudStorageConfigurationProvider(MockConnectionString);
            Assert.AreEqual(CloudStorageConfigurationTests.MockConnectionString, storageConfiguration.StorageConnectionString);
        }

        /// <summary>
        /// Tests that the constructor throws an exception when connections string is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_EmptyConnectionString()
        {
            // Arrange
            bool exceptionThrown = false;

            // Act
            try
            {
                CloudStorageConfigurationProvider storageConfiguration = new CloudStorageConfigurationProvider(string.Empty);

            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == CloudStorageConfigurationTests.StorageConnectionStringParameterName)
                {
                    exceptionThrown = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionThrown);                       
        }
    }
}
