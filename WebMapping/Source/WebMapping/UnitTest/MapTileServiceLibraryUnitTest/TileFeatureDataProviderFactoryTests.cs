namespace PTASMapTileServiceLibraryUnitTest
{
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASServicesCommon.DependencyInjection;
    using PTASWebMappingUnitTestCommon;

    [TestClass]
    public class TileFeatureDataProviderFactoryTests
    {
        /// <summary>
        /// Tests that a SQL server tile feature data provider can be created
        /// </summary>
        [TestMethod]
        public void Test_CreateSqlServerTileFeatureDataProvider()
        {
            //Arrange
            ITileFeatureDataProviderFactory tileFeatureDataProviderFactory = new TileFeatureDataProviderFactory();
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(0, 0, 1, 1).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            //Act
            ITileFeatureDataProvider tileFeatureDataProvider = tileFeatureDataProviderFactory.CreateSqlServerTileFeatureDataProvider(dbContextFactory, logger);

            //Assert
            Assert.IsNotNull(tileFeatureDataProvider);
            Assert.IsInstanceOfType(tileFeatureDataProvider, typeof(SqlServerFeatureDataProvider));
        }
    }
}