namespace PTASODataLibraryUnitTest.BlobTileProviderTests
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASServicesCommon.TokenProvider;
    using PtasUnitTestCommon;

    /// <summary>
    /// Tests the PtasHistoricalDbContext class.
    /// </summary>
    [TestClass]
    public class PtasHistoricalDbContextTests
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
                PtasHistoricalDbContext dbContext = new PtasHistoricalDbContext(null, serviceTokenProvider);
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
            DbContextOptions<PtasHistoricalDbContext> context = new DbContextOptionsBuilder<PtasHistoricalDbContext>().Options;

            //Act
            try
            {
                PtasHistoricalDbContext dbContext = new PtasHistoricalDbContext(context, null);
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
            DbContextOptions<PtasHistoricalDbContext> context = MockDbFactory.CreateInMemoryDbContextOptions<PtasHistoricalDbContext>();
            IServiceTokenProvider serviceTokenProvider = MockObjectFactory.CreateMockTokenProvider().Object;

            PtasHistoricalDbContext dbContext = new PtasHistoricalDbContext(context, serviceTokenProvider);

            //Try
            int estimateCount = (from e in dbContext.PtasEstimateHistory select e).Count();
            Assert.AreEqual(0, estimateCount);
        }
    }
}