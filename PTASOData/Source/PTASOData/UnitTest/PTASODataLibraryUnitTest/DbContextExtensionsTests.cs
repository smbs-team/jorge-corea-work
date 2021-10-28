namespace PTASODataLibraryUnitTest.BlobTileProviderTests
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASODataLibrary.Helper;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASODataLibraryUnitTest.Helper;
    using PTASODataUnitTestCommon.Helper;    

    /// <summary>
    /// Tests the ODataExtensions class.
    /// </summary>
    [TestClass]
    public class DbContextExtensionsTests
    {
        /// <summary>
        /// Tests the Reset method happy path.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_Reset()
        {
            //Arrange
            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();

            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));
            DbContextExtensions.Reset();
            string testedEntity = "PtasLanduse";
            HttpRequest request = RequestFactory.CreateFakeODataRequest(testedEntity, "");
            HttpRequest internalRequest = request.CreateInternalHttpContext().Request;

            //Act
            var query = dbContex.GetODataQueryByResourceName(request, internalRequest, testedEntity) as IQueryable<object>;
        }

        /// <summary>
        /// Tests the RegisterDbContext method happy path.
        /// </summary>
        [TestMethod]
        public void Test_RegisterDbContext()
        {
            //Arrange
            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            string testedEntity1 = "PtasArea";
            HttpRequest request1 = RequestFactory.CreateFakeODataRequest(testedEntity1, "");
            HttpRequest internalRequest1 = request1.CreateInternalHttpContext().Request;

            string testedEntity2 = "PtasBuildingdetail";
            HttpRequest request2 = RequestFactory.CreateFakeODataRequest(testedEntity2, "");
            HttpRequest internalRequest2 = request2.CreateInternalHttpContext().Request;

            //Act
            var query1 = dbContex.GetODataQueryByResourceName(request1, internalRequest1, testedEntity1) as IQueryable<object>;
            var query2 = dbContex.GetODataQueryByResourceName(request2, internalRequest2, testedEntity2) as IQueryable<object>;

            //Assert
            Assert.IsNotNull(query1);
            Assert.AreEqual(0, query1.ToList().Count);

            Assert.IsNotNull(query2);
            Assert.AreEqual(0, query2.ToList().Count);
        }

        #region GetODataQueryByResourceName Tests

        /// <summary>
        /// Tests the GetODataQueryByResourceName method happy path.
        /// </summary>
        [TestMethod]
        public void Test_GetODataQueryByResourceName()
        {
            //Arrange
            DbContextExtensions.Reset();

            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));            
            string testedEntity = "PtasLanduse";
            HttpRequest request = RequestFactory.CreateFakeODataRequest(testedEntity, "");
            HttpRequest internalRequest = request.CreateInternalHttpContext().Request;

            //Act
            var query = dbContex.GetODataQueryByResourceName(request, internalRequest, testedEntity) as IQueryable<object>;

            //Assert

            //We test that the query was generated and that it executes.
            Assert.IsNotNull(query);
            Assert.AreEqual(0, query.ToList().Count);
        }

        /// <summary>
        /// Tests the GetODataQueryByResourceName when a null request is sent.
        /// </summary>
        [TestMethod]
        public void Test_GetODataQueryByResourceName_NullRequest()
        {
            //Arrange
            DbContextExtensions.Reset();

            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));
            string testedEntity = "PtasLanduse";
            bool argumentException = false;
            HttpRequest request = RequestFactory.CreateFakeODataRequest(testedEntity, "");
            HttpRequest internalRequest = request.CreateInternalHttpContext().Request;

            //Act
            try
            {
                var query = dbContex.GetODataQueryByResourceName(null, internalRequest, testedEntity) as IQueryable<object>;
            }
            catch (System.ArgumentNullException ex)
            {
                argumentException = ex.ParamName == "req";
            }

            //Assert
            Assert.IsTrue(argumentException);            
        }

        /// <summary>
        /// Tests the GetODataQueryByResourceName when an empty entity is sent.
        /// </summary>
        [TestMethod]
        public void Test_GetODataQueryByResourceName_EmptyEntity()
        {
            //Arrange
            DbContextExtensions.Reset();

            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));
            string testedEntity = " ";
            HttpRequest request = RequestFactory.CreateFakeODataRequest(testedEntity, "");
            HttpRequest internalRequest = request.CreateInternalHttpContext().Request;

            bool argumentException = false;

            //Act
            try
            {
                var query = dbContex.GetODataQueryByResourceName(request, internalRequest, testedEntity) as IQueryable<object>;
            }
            catch (System.ArgumentNullException ex)
            {
                argumentException = ex.ParamName == "resourceName";
            }

            //Assert
            Assert.IsTrue(argumentException);
        }

        #endregion
    }
}