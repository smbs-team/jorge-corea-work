using PTASMapTileFunctions.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using PTASWebMappingUnitTestCommon;
using PTASServicesCommon.FileSystem;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
using PTASTileStorageWorkerLibrary.SqlServer.Model;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace PTASMapTileFunctionsUnitTest.FunctionTests
{
    /// <summary>
    /// Tests the OnBlobTileSourceUpdated function
    /// </summary>
    [TestClass]
    public class OnBlobTileSourceUpdatedTests
    {
        private const string DbContextParameterName = "dbContext";
        private const string MockBlobFileName = "MockSourceLocation0";

        #region Constructor Tests

        /// <summary>
        /// Tests constructor when the DBContext is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_DBContext_Null()
        {           
            bool exceptionHandled = false;

            // Act
            try
            {
                OnBlobTileSourceUpdated onBlobUpdatedFunction = new OnBlobTileSourceUpdated(null);
            }
            catch (System.ArgumentNullException ex)
            {
                if (ex.ParamName == OnBlobTileSourceUpdatedTests.DbContextParameterName)
                {
                    exceptionHandled = true;
                }
            }

            // Assert
            Assert.IsTrue(exceptionHandled);
        }

        #endregion ConstructorTetss

        #region Run Tests

        /// <summary>
        /// Tests the run method happy path
        /// </summary>
        [TestMethod]
        public void Test_Run()
        {
            // Arrange
            Mock<DbSet<TileStorageJobQueue>> jobQueueDbSet;
            Mock<TileStorageJobDbContext> dbContext = TileStorageJobDbContextMockFactory.CreateMockDbContext(null, out jobQueueDbSet);
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();

            OnBlobTileSourceUpdated onBlobUpdatedFunction = new OnBlobTileSourceUpdated(dbContext.Object);

            byte[] content = WebMappingMockFactory.CreateMockByteArray(32, 12);
            MemoryStream stream = new MemoryStream(content);

            // Act
            onBlobUpdatedFunction.Run(stream, MockBlobFileName, logger.Object);

            // Assert
            dbContext.Verify(m => m.SaveChanges(), Times.Once);
            jobQueueDbSet.Verify(m => m.Add(It.IsAny<TileStorageJobQueue>()), Times.Once);
        }

        #endregion Run Tests
    }
}
