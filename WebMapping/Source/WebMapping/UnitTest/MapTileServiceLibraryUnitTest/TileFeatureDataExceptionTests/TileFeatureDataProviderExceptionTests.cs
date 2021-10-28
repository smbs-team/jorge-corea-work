namespace PTASMapTileServiceLibraryUnitTest.TileProviderExceptionTests
{
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer;

    [TestClass]
    public class TileFeatureDataProviderExceptionTests
    {
        private const string ErrorMesage = "MockErrorMessage";
        private const string TileFeatureDataProviderTypeParameterName = "tileFeatureDataProviderType";

        /// <summary>
        /// Tests that the constructor stores all values passed as parameters
        /// </summary>
        [TestMethod]
        public void Test_Constructor()
        {
            //Arrange
            System.Exception innerException = new System.Exception();

            //Act
            TileFeatureDataProviderException tileFeatureDataProviderException = new TileFeatureDataProviderException(
                TileFeatureDataProviderExceptionCategory.SqlServerError, 
                typeof(SqlServerFeatureDataProvider),
                TileFeatureDataProviderExceptionTests.ErrorMesage, 
                innerException);

            //Assert
            Assert.AreSame(innerException, tileFeatureDataProviderException.InnerException);
            Assert.AreEqual(TileFeatureDataProviderExceptionTests.ErrorMesage, tileFeatureDataProviderException.Message);
            Assert.AreEqual(TileFeatureDataProviderExceptionCategory.SqlServerError, tileFeatureDataProviderException.TileFeatureDataProviderExceptionCategory);
            Assert.AreEqual(typeof(SqlServerFeatureDataProvider), tileFeatureDataProviderException.TileFeatureDataProviderType);
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
                TileFeatureDataProviderException tileProviderException = new TileFeatureDataProviderException(
                    TileFeatureDataProviderExceptionCategory.SqlServerError,
                    null, 
                    TileFeatureDataProviderExceptionTests.ErrorMesage, 
                    innerException);
            } catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == TileFeatureDataProviderExceptionTests.TileFeatureDataProviderTypeParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }
    }
}
