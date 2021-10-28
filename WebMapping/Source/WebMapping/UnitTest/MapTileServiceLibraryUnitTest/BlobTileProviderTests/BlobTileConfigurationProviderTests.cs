using PTASMapTileServicesLibrary.TileProvider.Blob;
using PTASMapTileServicesLibrary.TileProvider.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTASMapTileServiceLibraryUnitTest.BlobTileProviderTests
{
    [TestClass]
    public class BlobTileConfigurationProviderTests
    {        
        private const string TileContainerNameParameterName = "tileContainerName";
        private const string TilePathMaskParameterName = "tilePathMask";
        private const string MockTileContainerName = "mockContainer";
        private const string MockTilePathMask = "mockPath";

        /// <summary>
        /// Tests that the constructor stores all values passed as parameters
        /// </summary>
        [TestMethod]
        public void Test_Constructor()
        {
            //Act
            BlobTileConfigurationProvider tileProviderConfiguration = new BlobTileConfigurationProvider(
                BlobTileConfigurationProviderTests.MockTileContainerName, 
                BlobTileConfigurationProviderTests.MockTilePathMask);

            //Assert
            Assert.AreEqual(BlobTileConfigurationProviderTests.MockTileContainerName, tileProviderConfiguration.TileContainerName);
            Assert.AreEqual(BlobTileConfigurationProviderTests.MockTilePathMask, tileProviderConfiguration.TilePathMask);
        }

        /// <summary>
        /// Tests the that the constructor throws a null exception when tile container name is empty
        /// </summary>
        [TestMethod]
        public void Test_Constructor_TileContainerNameEmpty()
        {
            //Arrange
            bool exceptionThrown = false;

            //Act
            try
            {
                BlobTileConfigurationProvider tileProviderConfiguration = new BlobTileConfigurationProvider(
                    string.Empty, 
                    BlobTileConfigurationProviderTests.MockTilePathMask);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == BlobTileConfigurationProviderTests.TileContainerNameParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests the that the constructor throws a null exception when tile path mask is empty
        /// </summary>
        [TestMethod]
        public void Test_Constructor_TilePathMaskEmpty()
        {
            //Arrange
            bool exceptionThrown = false;

            //Act
            try
            {
                BlobTileConfigurationProvider tileProviderConfiguration = new BlobTileConfigurationProvider(
                    BlobTileConfigurationProviderTests.MockTileContainerName, 
                    string.Empty);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == BlobTileConfigurationProviderTests.TilePathMaskParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }
    }
}