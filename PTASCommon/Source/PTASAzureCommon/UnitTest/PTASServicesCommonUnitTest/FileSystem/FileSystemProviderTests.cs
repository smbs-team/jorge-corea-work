namespace PTASServicesCommonUnitTest.CloudStorageTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage;
    using Moq;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.FileSystem;
    using PTASWebMappingUnitTestCommon;
    using System.Threading.Tasks;

    [TestClass]
    public class FileSystemProviderTests
    {
        public const string FileContent = "Test File!";
        /// <summary>
        /// Tests the happy path for ReadAllTextAsync
        /// </summary>
        [TestMethod]
        public async Task Test_ReadAllTextAsync()
        {
            // Arrange
            FileSystemProvider fileSystemProvider = new FileSystemProvider();

            // Act
            string text = await fileSystemProvider.ReadAllTextAsync("testfile.txt");

            // Assert            
            Assert.AreEqual(FileContent, text);
        }      
    }
}
