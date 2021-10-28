namespace PTASServicesCommonUnitTest.CloudStorageTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASServicesCommon.CloudStorage;
    using PTASWebMappingUnitTestCommon;

    [TestClass]
    public class BlobSharedSignatureProviderTests
    {
        private const string StorageProviderParameterName = "storageProvider";
                
        /// <summary>
        /// Tests the constructor when azure storage provider is sent with null value
        /// </summary>
        [TestMethod]
        public void Test_Constructor_NullStorageProvider()
        {
            // Arrange
            bool exceptionThrown = false;

            // Act
            try
            {
                BlobSharedSignatureProvider signatureProvider = new BlobSharedSignatureProvider(null);

            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == BlobSharedSignatureProviderTests.StorageProviderParameterName)
                {
                    exceptionThrown = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }        
    }
}
