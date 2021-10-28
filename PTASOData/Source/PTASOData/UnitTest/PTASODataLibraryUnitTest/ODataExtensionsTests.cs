namespace PTASODataLibraryUnitTest.BlobTileProviderTests
{
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASODataLibrary.Helper;
    using PTASODataLibrary.PtasDbDataProvider.PtasCamaModel;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASODataLibraryUnitTest.Helper;
    using PTASODataUnitTestCommon.Helper;

    /// <summary>
    /// Tests the ODataExtensions class.
    /// </summary>
    [TestClass]
    public class ODataExtensionsTests
    {
        /// <summary>
        /// Tests the Reset method happy path.
        /// </summary>
        [TestMethod]
        public void Test_Reset()
        {
            //Arrange
            ODataExtensions.RegisterEntity<PtasParceldetail>();
            ODataExtensions.Reset();            

            //Act
            string metaData = ODataExtensions.GetMetadata().Result;

            //Assert

            //We test that no entity was defined.
            Assert.IsFalse(metaData.Contains("<EntityType Name="));
        }

        /// <summary>
        /// Tests the RegisterEntityMethod happy path.
        /// </summary>
        [TestMethod]
        public void Test_RegisterEntity()
        {
            //Arrange
            ODataExtensions.Reset();

            //Act
            ODataExtensions.RegisterEntity<PtasParceldetail>();
            ODataExtensions.RegisterEntity<PtasLand>();

            //Assert
            string metaData = ODataExtensions.GetMetadata().Result;

            //Search for directly referenced entities
            Assert.IsTrue(metaData.Contains(ODataExtensionsTests.GetExpectedEntityMetadata<PtasParceldetail>()));
            Assert.IsTrue(metaData.Contains(ODataExtensionsTests.GetExpectedEntityMetadata<PtasLand>()));

            //Search for some indirectly referenced entities
            Assert.IsTrue(metaData.Contains(ODataExtensionsTests.GetExpectedEntityMetadata<PtasZipcode>()));
            Assert.IsTrue(metaData.Contains(ODataExtensionsTests.GetExpectedEntityMetadata<PtasStateorprovince>()));
            Assert.IsTrue(metaData.Contains(ODataExtensionsTests.GetExpectedEntityMetadata<PtasYear>()));
        }

        #region GetMetadata Tests

        /// <summary>
        /// Tests the GetMetadata method with no meta-data.
        /// </summary>
        [TestMethod]
        public void Test_GetMetadata_Empty()
        {
            //Arrange
            ODataExtensions.Reset();

            //Act
            string metaData = ODataExtensions.GetMetadata().Result;

            //Assert
           
            //We test that no entity was defined.
            Assert.IsFalse(metaData.Contains("<EntityType Name="));
        }

        /// <summary>
        /// Tests the GetMetadata method happy path.
        /// </summary>
        [TestMethod]
        public void Test_GetMetadata()
        {
            //Arrange
            ODataExtensions.Reset();
            ODataExtensions.RegisterEntity<PtasZipcode>();

            //Act
            string metaData = ODataExtensions.GetMetadata().Result;

            //Assert

            //We test that the zipcode entity was defined.
            Assert.IsTrue(metaData.Contains(ODataExtensionsTests.GetExpectedEntityMetadata<PtasZipcode>()));
        }

        #endregion

        #region ApplyTo Tests

        /// <summary>
        /// Tests the ApplyTo method with the happy path.
        /// </summary>
        [TestMethod]
        public void Test_ApplyTo()
        {
            //Arrange            
            ODataExtensions.Reset();
            ODataExtensions.RegisterEntity<PtasParceldetail>();

            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            HttpRequest request = RequestFactory.CreateFakeODataRequest("PtasParceldetail", "?$top=5&$skip=5&$select=PtasMajor,PtasMinor");
            HttpRequest internalRequest = request.CreateInternalHttpContext().Request;
            var parcelQuery = from e in dbContex.PtasParceldetail select e;            

             //Act
             var oDataQuery = ODataExtensions.ApplyTo<PtasParceldetail>(request, internalRequest, parcelQuery) as IQueryable<object>;

            //Assert

            //We test that the query was generated and that it executes.
            Assert.IsNotNull(oDataQuery);
            Assert.AreEqual(0, oDataQuery.ToList().Count);
        }

        /// <summary>
        /// Tests the ApplyTo with a null request.
        /// </summary>
        [TestMethod]
        public void Test_ApplyTo_NullRequest()
        {
            //Arrange            
            ODataExtensions.Reset();
            ODataExtensions.RegisterEntity<PtasParceldetail>();

            HttpRequest request = RequestFactory.CreateFakeODataRequest("PtasParceldetail", "?$top=5&$skip=5&$select=PtasMajor,PtasMinor");
            HttpRequest internalRequest = request.CreateInternalHttpContext().Request;

            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            var parcelQuery = from e in dbContex.PtasParceldetail select e;

            bool nullArgumentException = false;

            //Act
            try
            {
                var oDataQuery = ODataExtensions.ApplyTo<PtasParceldetail>(null, internalRequest, parcelQuery) as IQueryable<object>;
            }
            catch (System.ArgumentNullException ex)
            {
                nullArgumentException = ex.ParamName == "request";
            }


            //Assert
            Assert.IsTrue(nullArgumentException);
        }

        /// <summary>
        /// Tests the ApplyTo with a null query.
        /// </summary>
        [TestMethod]
        public void Test_ApplyTo_NullQuery()
        {
            //Arrange            
            ODataExtensions.Reset();
            ODataExtensions.RegisterEntity<PtasParceldetail>();

            HttpRequest request = RequestFactory.CreateFakeODataRequest("PtasParceldetail", "?$top=5&$skip=5&$select=PtasMajor,PtasMinor");
            HttpRequest internalRequest = request.CreateInternalHttpContext().Request;
            bool nullArgumentException = false;

            //Act
            try
            {
                var oDataQuery = ODataExtensions.ApplyTo<PtasParceldetail>(request, internalRequest, null) as IQueryable<object>;
            }
            catch (System.ArgumentNullException ex)
            {
                nullArgumentException = ex.ParamName == "query";
            }

            //Assert
            Assert.IsTrue(nullArgumentException);
        }

        #endregion

        /// <summary>
        /// Gets the expected entity metadata.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetExpectedEntityMetadata<T>()
        {
            return "<EntityType Name=\"" + typeof(T).Name + "\">";
        }
    }
}