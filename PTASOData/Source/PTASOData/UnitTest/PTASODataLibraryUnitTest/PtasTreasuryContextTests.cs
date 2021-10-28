namespace PTASODataLibraryUnitTest.BlobTileProviderTests
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASServicesCommon.TokenProvider;
    using PtasUnitTestCommon;

    /// <summary>
    /// Tests the PtasTreasuryContext class.
    /// </summary>
    [TestClass]
    public class PtasTreasuryContextTests
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
                PtasTreasuryContext dbContext = new PtasTreasuryContext(null, serviceTokenProvider);
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
            DbContextOptions<PtasTreasuryContext> context = new DbContextOptionsBuilder<PtasTreasuryContext>().Options;

            //Act
            try
            {
                PtasTreasuryContext dbContext = new PtasTreasuryContext(context, null);
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
            DbContextOptions<PtasTreasuryContext> context = MockDbFactory.CreateInMemoryDbContextOptions<PtasTreasuryContext>();
            IServiceTokenProvider serviceTokenProvider = MockObjectFactory.CreateMockTokenProvider().Object;

            PtasTreasuryContext dbContext = new PtasTreasuryContext(context, serviceTokenProvider);

            //Try
            int etaxMasterCount = (from e in dbContext.EtaxAccountMaster select e).Count();
            Assert.AreEqual(0, etaxMasterCount);
        }
    }
}