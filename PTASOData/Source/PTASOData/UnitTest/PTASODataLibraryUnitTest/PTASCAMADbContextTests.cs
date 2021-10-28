namespace PTASODataLibraryUnitTest.BlobTileProviderTests
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASServicesCommon.TokenProvider;
    using PtasUnitTestCommon;

    /// <summary>
    /// Tests the PtasCamaDbContext class.
    /// </summary>
    [TestClass]
    public class PtasCamaDbContextTests
    {
        /// <summary>
        /// Tests the constructor with null options.
        /// </summary>
        [TestMethod]
        public void Test_Constructor_NullOptions()
        {
            //Arrange
            IServiceTokenProvider serviceTokenProvider = MockObjectFactory.CreateMockTokenProvider().Object;
            bool nullArgumentException = false;

            //Act
            try
            {
                PTASCAMADbContext dbContext = new PTASCAMADbContext(null, serviceTokenProvider);
            } 
            catch (System.ArgumentNullException ex)
            {
                nullArgumentException = ex.ParamName == "options";
            }                       

            //Try
            Assert.IsTrue(nullArgumentException);
        }

        /// <summary>
        /// Tests the constructor with null options.
        /// </summary>
        [TestMethod]
        public void Test_Constructor_NullServiceTokenProvider()
        {
            //Arrange
            bool nullArgumentException = false;
            DbContextOptions<PTASCAMADbContext> context = new DbContextOptionsBuilder<PTASCAMADbContext>().Options;

            //Act
            try
            {
                PTASCAMADbContext dbContext = new PTASCAMADbContext(context, null);
            }
            catch (System.ArgumentNullException ex)
            {
                nullArgumentException = ex.ParamName == "tokenProvider";
            }

            //Try
            Assert.IsTrue(nullArgumentException);
        }

        /// <summary>
        /// Tests the constructor happy path.
        /// </summary>
        [TestMethod]
        public void Test_Constructor()
        {
            //Arrange
            DbContextOptions<PTASCAMADbContext> context = MockDbFactory.CreateInMemoryDbContextOptions<PTASCAMADbContext>();
            IServiceTokenProvider serviceTokenProvider = MockObjectFactory.CreateMockTokenProvider().Object;

            PTASCAMADbContext dbContext = new PTASCAMADbContext(context, serviceTokenProvider);

            //Try
            int accesoryDetailCount = (from e in dbContext.PtasAccessorydetail select e).Count();
            Assert.AreEqual(0, accesoryDetailCount);
        }
    }
}