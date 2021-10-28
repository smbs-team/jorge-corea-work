namespace PTASWebMappingUnitTestCommon
{
    using GeoAPI.Geometries;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NetTopologySuite.Geometries;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
    using PTASTileStorageWorkerLibrary.SqlServer.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class that mocks the db context for tile storage jobs.
    /// </summary>
    public static class TileStorageJobDbContextMockFactory
    {
        private const string MockSourceLocation = "MockSourceLocation";
        private const string MockTargetLocation = "MockTargetLocation";

        public static TileStorageJobType CreateMockJobType(int jobTypeId, StorageConversionType conversionType)
        {
            return new TileStorageJobType
            {
                JobFormat = conversionType,
                JobTypeId = jobTypeId,
                SourceLocation = TileStorageJobDbContextMockFactory.MockSourceLocation + jobTypeId.ToString(),
                TargetLocation = TileStorageJobDbContextMockFactory.MockTargetLocation + jobTypeId.ToString(),
            };
        }

        public static List<TileStorageJobQueue> CreateMockStorageJobs()
        {
            List<TileStorageJobQueue> toReturn = new List<TileStorageJobQueue>();

            toReturn.Add(new TileStorageJobQueue()
            {
                JobId = 0,
                JobTypeId = 0,
                JobType = TileStorageJobDbContextMockFactory.CreateMockJobType(0, StorageConversionType.SqlServerToGpkg)
            });

            toReturn.Add(new TileStorageJobQueue()
            {
                JobId = 1,
                JobTypeId = 1,
                JobType = TileStorageJobDbContextMockFactory.CreateMockJobType(1, StorageConversionType.BlobFilePassthrough)
            });

            toReturn.Add(new TileStorageJobQueue()
            {
                JobId = 2,
                JobTypeId = 2,
                JobType = TileStorageJobDbContextMockFactory.CreateMockJobType(2, StorageConversionType.SqlServerToSqlServer)
            });

            return toReturn;
        }

        public static List<TileStorageJobType> CreateMockStorageJobTypes()
        {
            List<TileStorageJobType> toReturn = new List<TileStorageJobType>();

            toReturn.Add(TileStorageJobDbContextMockFactory.CreateMockJobType(0, StorageConversionType.SqlServerToGpkg));

            return toReturn;
        }

        public static Mock<DbSet<TileStorageJobQueue>> CreateMockTileStorageJobQueueDbSet()
        {
            var mockSet = new Mock<DbSet<TileStorageJobQueue>>();
            var data = TileStorageJobDbContextMockFactory.CreateMockStorageJobs().AsQueryable();
            mockSet.As<IQueryable<TileStorageJobQueue>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TileStorageJobQueue>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TileStorageJobQueue>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TileStorageJobQueue>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static Mock<DbSet<TileStorageJobQueue>> CreateMockTileStorageJobQueueDbSet(Exception exceptionToThrow)
        {
            var mockSet = new Mock<DbSet<TileStorageJobQueue>>();
            var data = new List<TileStorageJobQueue>().AsQueryable();
            mockSet.As<IQueryable<TileStorageJobQueue>>().Setup(m => m.Provider).Returns(() =>
            {
                throw exceptionToThrow;
            });
            mockSet.As<IQueryable<TileStorageJobQueue>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TileStorageJobQueue>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TileStorageJobQueue>>().Setup(m => m.GetEnumerator()).Returns(() =>
            {
                throw exceptionToThrow;
            });

            return mockSet;
        }

        public static Mock<DbSet<TileStorageJobType>> CreateMockTileStorageJobTypeDbSet()
        {
            var mockSet = new Mock<DbSet<TileStorageJobType>>();
            var data = TileStorageJobDbContextMockFactory.CreateMockStorageJobTypes().AsQueryable();
            mockSet.As<IQueryable<TileStorageJobType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TileStorageJobType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TileStorageJobType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TileStorageJobType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static Mock<DbSet<TileStorageJobType>> CreateMockTileStorageJobTypeDbSet(Exception exceptionToThrow)
        {
            var mockSet = new Mock<DbSet<TileStorageJobType>>();
            var data = new List<TileStorageJobType>().AsQueryable();
            mockSet.As<IQueryable<TileStorageJobType>>().Setup(m => m.Provider).Returns(() =>
            {
                throw exceptionToThrow;
            });
            mockSet.As<IQueryable<TileStorageJobType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TileStorageJobType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TileStorageJobType>>().Setup(m => m.GetEnumerator()).Returns(() =>
            {
                throw exceptionToThrow;
            });

            return mockSet;
        }
        public static Mock<TileStorageJobDbContext> CreateMockDbContext(TileStorageJobQueue nextJob)
        {
            Mock<DbSet<TileStorageJobQueue>> jobQueueDbSet = null;
            return TileStorageJobDbContextMockFactory.CreateMockDbContext(nextJob, out jobQueueDbSet);
        }

        public static Mock<TileStorageJobDbContext> CreateMockDbContext(TileStorageJobQueue nextJob, out Mock<DbSet<TileStorageJobQueue>> jobQueueDbSet)
        {
            jobQueueDbSet = TileStorageJobDbContextMockFactory.CreateMockTileStorageJobQueueDbSet();
            DbContextOptions<TileStorageJobDbContext> options = new DbContextOptionsBuilder<TileStorageJobDbContext>().Options;
            var mockContext = new Mock<TileStorageJobDbContext>(options);
            mockContext.Setup(c => c.TileStorageJobQueue).Returns(jobQueueDbSet.Object);
            mockContext.Setup(c => c.TileStorageJobType).Returns(
               TileStorageJobDbContextMockFactory.CreateMockTileStorageJobTypeDbSet().Object);

            mockContext.Setup(c => c.PopNextStorageJob()).Returns(nextJob);

            return mockContext;
        }

        public static Mock<TileStorageJobDbContext> CreateMockDbContext(Exception exceptionToThrow)
        {
            DbContextOptions<TileStorageJobDbContext> options = new DbContextOptionsBuilder<TileStorageJobDbContext>().Options;
            var mockContext = new Mock<TileStorageJobDbContext>(options);
            mockContext.Setup(c => c.TileStorageJobQueue).Returns(
                TileStorageJobDbContextMockFactory.CreateMockTileStorageJobQueueDbSet(exceptionToThrow).Object);

            mockContext.Setup(c => c.TileStorageJobType).Returns(
               TileStorageJobDbContextMockFactory.CreateMockTileStorageJobTypeDbSet(exceptionToThrow).Object);

            mockContext.Setup(c => c.PopNextStorageJob()).Throws(exceptionToThrow);

            return mockContext;
        }
    }
}
