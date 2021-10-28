namespace PTASWebMappingUnitTestCommon
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query.Internal;
    using Moq;
    using NetTopologySuite.Geometries;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class that mocks the db context for tile feature data service.
    /// </summary>
    public static class TileFeatureDataDbContextMockFactory
    {
        private const int MockParcelFeatureCount = 5;
        private const int MockLayerSourceCount = 5;

        public static IFactory<TileFeatureDataDbContext> CreateTileDbContextFactory(TileFeatureDataDbContext dbContext)
        {
            return new Factory<TileFeatureDataDbContext>(() =>
            {
                return dbContext;
            });
        }

        public static List<ParcelFeature> CreateMockParcelFeatures(double minX, double minY, double maxX, double maxY)
        {
            List<ParcelFeature> toReturn = new List<ParcelFeature>();

            for (int i = 0; i < MockParcelFeatureCount; i++)
            {
                toReturn.Add(new ParcelFeature()
                {
                    Major = (i+1).ToString(),
                    Minor = (i+2).ToString(),
                    Shape = TileFeatureDataDbContextMockFactory.CreateMockGeometryRectangle(minX, minY, maxX, maxY),
                    Pin = (i + 0.5).ToString(),
                });
            }

            return toReturn;
        }

        public static List<ParcelFeatureData> CreateMockParcelFeatureData()
        {
            List<ParcelFeatureData> toReturn = new List<ParcelFeatureData>();

            for (int i = 0; i < TileFeatureDataDbContextMockFactory.MockParcelFeatureCount; i++)
            {
                toReturn.Add(new ParcelFeatureData()
                {
                    PinNumeric = (i + 0.5),
                    LandId = i,
                    Major = (i + 1).ToString(),
                    Minor = (i + 2).ToString(),
                    RealPropId = (i + 3),
                    Pin = (i + 0.5).ToString(),
                    TaxpayerName = (i + 0.5).ToString()
                });
            }

            return toReturn;
        }

        public static List<LayerSource> CreateMockLayerSourceData()
        {
            List<LayerSource> toReturn = new List<LayerSource>();

            for (int i = 0; i < TileFeatureDataDbContextMockFactory.MockLayerSourceCount; i++)
            {
                toReturn.Add(new LayerSource()
                {
                    DbTableName = "TableName",
                    GisLayerName = $"Layer{i.ToString()}",
                    IsParcelSource = false,
                    IsVectorLayer = true,
                    LayerSourceId = i,
                    LayerSourceName = $"LayerSource{i.ToString()}",
                    OgrLayerData = null,
                    ServeFromFileShare = true
                });
            }

            return toReturn;
        }

        public static Mock<DbSet<ParcelFeature>> CreateMockParecelFeatureDbSet(double minX, double minY, double maxX, double maxY)
        {
            var mockSet = new Mock<DbSet<ParcelFeature>>();
            var data = TileFeatureDataDbContextMockFactory.CreateMockParcelFeatures(minX, minY, maxX, maxY).AsQueryable();
            mockSet.As<IQueryable<ParcelFeature>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<ParcelFeature>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ParcelFeature>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ParcelFeature>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static Mock<DbSet<ParcelFeature>> CreateMockParecelFeatureDbSet(Exception exceptionToThrow)
        {
            var mockSet = new Mock<DbSet<ParcelFeature>>();
            var data = new List<ParcelFeature>().AsQueryable();
            mockSet.As<IQueryable<ParcelFeature>>().Setup(m => m.Provider).Returns(() => {
                throw exceptionToThrow;
            });
            mockSet.As<IQueryable<ParcelFeature>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ParcelFeature>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ParcelFeature>>().Setup(m => m.GetEnumerator()).Returns(() => {
                throw exceptionToThrow;
            });

            return mockSet;
        }

        public static Mock<DbSet<ParcelFeatureData>> CreateMockParecelFeatureDataDbSet()
        {
            var mockSet = new Mock<DbSet<ParcelFeatureData>>();            
            var data = TileFeatureDataDbContextMockFactory.CreateMockParcelFeatureData().AsQueryable();
            mockSet.As<IQueryable<ParcelFeatureData>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<ParcelFeatureData>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ParcelFeatureData>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ParcelFeatureData>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static Mock<DbSet<LayerSource>> CreateMockLayerSourceDataDbSet()
        {
            var mockSet = new Mock<DbSet<LayerSource>>();
            var data = TileFeatureDataDbContextMockFactory.CreateMockLayerSourceData().AsQueryable();
            var asyncData= 
            mockSet.As<IQueryable<LayerSource>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<LayerSource>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<LayerSource>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<LayerSource>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static Mock<DbSet<LayerSource>> CreateMockLayerSourceDbSet(Exception exceptionToThrow)
        {
            var mockSet = new Mock<DbSet<LayerSource>>();
            var data = new List<LayerSource>().AsQueryable();
            mockSet.As<IQueryable<LayerSource>>().Setup(m => m.Provider).Returns(() => {
                throw exceptionToThrow;
            });
            mockSet.As<IQueryable<LayerSource>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<LayerSource>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<LayerSource>>().Setup(m => m.GetEnumerator()).Returns(() => {
                throw exceptionToThrow;
            });

            return mockSet;
        }

        public static Mock<DbSet<ParcelFeatureData>> CreateMockParecelFeatureDataDbSet(Exception exceptionToThrow)
        {
            var mockSet = new Mock<DbSet<ParcelFeatureData>>();
            var data = new List<ParcelFeatureData>().AsQueryable();
            mockSet.As<IQueryable<ParcelFeatureData>>().Setup(m => m.Provider).Returns(() => {
                throw exceptionToThrow;
            });
            mockSet.As<IQueryable<ParcelFeatureData>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ParcelFeatureData>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ParcelFeatureData>>().Setup(m => m.GetEnumerator()).Returns(() => {
                throw exceptionToThrow;
            });

            return mockSet;
        }

        public static Mock<TileFeatureDataDbContext> CreateMockDbContext(double minX, double minY, double maxX, double maxY)
        {
            DbContextOptions<TileFeatureDataDbContext> options = new DbContextOptionsBuilder<TileFeatureDataDbContext>().Options;
            Mock<IServiceTokenProvider> tokenProvider = WebMappingMockFactory.CreateMockTokenProvider();
            var mockContext = new Mock<TileFeatureDataDbContext>(options, tokenProvider.Object);
            mockContext.Setup(c => c.Parcel).Returns(
                TileFeatureDataDbContextMockFactory.CreateMockParecelFeatureDbSet(minX, minY, maxX, maxY).Object);

            mockContext.Setup(c => c.LayerSource).Returns(
              TileFeatureDataDbContextMockFactory.CreateMockLayerSourceDataDbSet().Object);

            mockContext.Setup(c => c.GisMapData2).Returns(
               TileFeatureDataDbContextMockFactory.CreateMockParecelFeatureDataDbSet().Object);

            return mockContext;
        }

        public static Mock<TileFeatureDataDbContext> CreateMockDbContext(Exception exceptionToThrow)
        {
            DbContextOptions<TileFeatureDataDbContext> options = new DbContextOptionsBuilder<TileFeatureDataDbContext>().Options;
            Mock<IServiceTokenProvider> tokenProvider = WebMappingMockFactory.CreateMockTokenProvider();
            var mockContext = new Mock<TileFeatureDataDbContext>(options, tokenProvider.Object);
            mockContext.Setup(c => c.Parcel).Returns(
                TileFeatureDataDbContextMockFactory.CreateMockParecelFeatureDbSet(exceptionToThrow).Object);

            mockContext.Setup(c => c.LayerSource).Returns(
              TileFeatureDataDbContextMockFactory.CreateMockLayerSourceDbSet(exceptionToThrow).Object);


            mockContext.Setup(c => c.GisMapData2).Returns(
               TileFeatureDataDbContextMockFactory.CreateMockParecelFeatureDataDbSet(exceptionToThrow).Object);

            return mockContext;
        }

        private static Geometry CreateMockGeometryRectangle(double minX, double minY, double maxX, double maxY)
        {
            GeometryFactory geometryFactory = new GeometryFactory();
            Coordinate[] extentCoordinates = new Coordinate[5];
            extentCoordinates[0] = new Coordinate(minX, minY);
            extentCoordinates[1] = new Coordinate(minX, maxY);
            extentCoordinates[2] = new Coordinate(maxX, maxY);
            extentCoordinates[3] = new Coordinate(maxX, minY);
            extentCoordinates[4] = new Coordinate(minX, minY);

            return geometryFactory.CreatePolygon(extentCoordinates);
        }
    }
}
