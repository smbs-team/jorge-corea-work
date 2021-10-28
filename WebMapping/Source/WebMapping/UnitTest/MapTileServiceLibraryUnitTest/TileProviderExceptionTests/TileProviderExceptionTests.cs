using PTASMapTileServicesLibrary.TileProvider.Blob;
using PTASMapTileServicesLibrary.TileProvider.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTASMapTileServiceLibraryUnitTest.TileProviderExceptionTests
{
    [TestClass]
    public class TileProviderExceptionTests
    {
        private const string ErrorMesage = "MockErrorMessage";
        private const string TileProviderTypeParameterName = "tileProviderType";

        /// <summary>
        /// Tests that the constructor stores all values passed as parameters
        /// </summary>
        [TestMethod]
        public void Test_Constructor()
        {
            //Arrange
            System.Exception innerException = new System.Exception();

            //Act
            TileProviderException tileProviderException = new TileProviderException(
                TileProviderExceptionCategory.ServerError,
                typeof(BlobTileProvider),
                TileProviderExceptionTests.ErrorMesage,
                innerException) ;

            //Assert
            Assert.AreSame(innerException, tileProviderException.InnerException);
            Assert.AreEqual(TileProviderExceptionTests.ErrorMesage, tileProviderException.Message);
            Assert.AreEqual(TileProviderExceptionCategory.ServerError, tileProviderException.TileProviderExceptionCategory);
            Assert.AreEqual(typeof(BlobTileProvider), tileProviderException.TileProviderType);
        }

        /// <summary>
        /// Tests the that the constructor throws a null exception when tile provider type is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_TileProviderTypeNull()
        {
            //Arrange
            System.Exception innerException = new System.Exception();
            bool exceptionThrown = false;

            //Act
            try
            {
                TileProviderException tileProviderException = new TileProviderException(TileProviderExceptionCategory.ServerError, null, TileProviderExceptionTests.ErrorMesage, innerException);
            } catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == TileProviderExceptionTests.TileProviderTypeParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }
    }
}
