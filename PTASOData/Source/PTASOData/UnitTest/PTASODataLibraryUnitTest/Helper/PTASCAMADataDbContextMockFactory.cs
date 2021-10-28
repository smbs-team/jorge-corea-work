namespace PTASODataUnitTestCommon.Helper
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using PTASODataLibrary.PtasDbDataProvider.PtasCamaModel;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;
    using PtasUnitTestCommon;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class that mocks the db context for tile feature data service.
    /// </summary>
    public static class PTASCAMADataDbContextMockFactory
    {        

        public static PTASCAMADbContext CreateInMemoryEmptyDbContext()
        {

            DbContextOptions<PTASCAMADbContext> options = MockDbFactory.CreateInMemoryDbContextOptions<PTASCAMADbContext>();
            Mock<IServiceTokenProvider> tokenProvider = MockObjectFactory.CreateMockTokenProvider();
            var dbContext = new PTASCAMADbContext(options, tokenProvider.Object);

            return dbContext;
        }

        public static PTASCamaAndHistoricalDbContext CreateInMemoryEmptyCamaAndHistoricalDbContext()
        {

            DbContextOptions<PTASCamaAndHistoricalDbContext> options = MockDbFactory.CreateInMemoryDbContextOptions<PTASCamaAndHistoricalDbContext>();
            Mock<IServiceTokenProvider> tokenProvider = MockObjectFactory.CreateMockTokenProvider();
            var dbContext = new PTASCamaAndHistoricalDbContext(options, tokenProvider.Object);

            return dbContext;
        }


        public static void AddMockParcelDetails(PTASCAMADbContext dbContext, int count)
        {
            for (int i = 0; i < count; i++)
            {
                dbContext.PtasParceldetail.Add(new PtasParceldetail { PtasParceldetailid = Guid.NewGuid(), PtasMajor = i.ToString() });
            }

            dbContext.SaveChanges();
        }

    }
}
