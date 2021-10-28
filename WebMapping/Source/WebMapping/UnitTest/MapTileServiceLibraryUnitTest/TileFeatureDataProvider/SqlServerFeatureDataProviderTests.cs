using PTASMapTileServicesLibrary.TileProvider.Exception;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTASWebMappingUnitTestCommon;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
using PTASMapTileServicesLibrary.Geography.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;
using PTASServicesCommon.DependencyInjection;

namespace PTASMapTileServiceLibraryUnitTest.BlobTileProviderTests
{
    /// <summary>
    /// Tests the SqlServerFatureDataProvider class.
    /// </summary>
    [TestClass]
    public class SqlServerFeatureDataProviderTests
    {
        private const string DbContextParameterName = "dbContext";
        private const string LoggerParameterName = "logger";

        private const string MockLayerId = "MockLayerId";
        private const int ZoomLevel = 14;
        private const double MinX = 0.1;
        private const double MinY = 0.2;
        private const double MaxX = 0.3;
        private const double MaxY = 0.5;

        private static Extent MockExtent => new Extent(
            SqlServerFeatureDataProviderTests.MinX, 
            SqlServerFeatureDataProviderTests.MinY, 
            SqlServerFeatureDataProviderTests.MaxX, 
            SqlServerFeatureDataProviderTests.MaxY);

        #region "Constructor Tests"

        /// <summary>
        /// Tests the that the constructor throws a null exception when the dbContext parameter is null
        /// </summary>
        //[TestMethod]
        public void Test_Constructor_DbContextNull()
        {
            //Arrange
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
            bool exceptionThrown = false;

            //Act
            try
            {
                SqlServerFeatureDataProvider tileProvider = new SqlServerFeatureDataProvider(null, logger, null);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == SqlServerFeatureDataProviderTests.DbContextParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests the that the constructor throws a null exception when the logger parameter is null
        /// </summary>
        //[TestMethod]
        public void Test_Constructor_LoggerNull()
        {
            //Arrange
            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(
                SqlServerFeatureDataProviderTests.MinX,
                SqlServerFeatureDataProviderTests.MinY,
                SqlServerFeatureDataProviderTests.MaxX,
                SqlServerFeatureDataProviderTests.MaxY).Object;

            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);

            bool exceptionThrown = false;

            //Act
            try
            {
                SqlServerFeatureDataProvider tileProvider = new SqlServerFeatureDataProvider(dbContextFactory, null);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == SqlServerFeatureDataProviderTests.LoggerParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        #endregion

        #region "GetTile Tests"

        /// <summary>
        /// Tests the get tile feature data method, happy path
        /// </summary>
        //[TestMethod]
        public void Test_GetTileFeatureData()
        {
            //Arrange
            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(
                SqlServerFeatureDataProviderTests.MinX,
                SqlServerFeatureDataProviderTests.MinY,
                SqlServerFeatureDataProviderTests.MaxX,
                SqlServerFeatureDataProviderTests.MaxY).Object;
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;

            List<ParcelFeature> expectedFeatures = TileFeatureDataDbContextMockFactory.CreateMockParcelFeatures(
                 SqlServerFeatureDataProviderTests.MinX,
                SqlServerFeatureDataProviderTests.MinY,
                SqlServerFeatureDataProviderTests.MaxX,
                SqlServerFeatureDataProviderTests.MaxY);

            List<ParcelFeatureData> expectedFeatureData = TileFeatureDataDbContextMockFactory.CreateMockParcelFeatureData();
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);


            SqlServerFeatureDataProvider tileFeatureDataProvider = new SqlServerFeatureDataProvider(dbContextFactory, logger);

            //Act
            FeatureDataResponse response = tileFeatureDataProvider.GetTileFeatureData(
                SqlServerFeatureDataProviderTests.MockExtent,
                SqlServerFeatureDataProviderTests.ZoomLevel,
                SqlServerFeatureDataProviderTests.MockLayerId,
                null,
                null,
                null).Result;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.FeaturesDataCollections);
            Assert.AreEqual(SqlServerFeatureDataProviderTests.MockLayerId, response.LayerId);
            
            for (int i = 0; i < expectedFeatures.Count; i++)
            {
                SqlServerFeatureDataProviderTests.AssertParcelFeatureAreEqual(
                    expectedFeatures[i],
                    expectedFeatureData[i],
                    response.FeaturesDataCollections[i]);
            }
        }

        /// <summary>
        /// Tests the get tile feature data method when there's a SQL server error
        /// </summary>
        //[TestMethod]
        public void Test_GetTileFeaturaData_SqlException()
        {
            //Arrange
            //Arrange
            SqlException sqlException = WebMappingMockFactory.InstantiateUnitialized<SqlException>();

            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(sqlException).Object;
            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);


            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
           
            SqlServerFeatureDataProvider tileFeatureDataProvider = new SqlServerFeatureDataProvider(dbContextFactory, logger);

            //Act          
            bool exceptionHandled = false;

            //Act
            try
            {
                FeatureDataResponse response = tileFeatureDataProvider.GetTileFeatureData(
                    SqlServerFeatureDataProviderTests.MockExtent,
                    SqlServerFeatureDataProviderTests.ZoomLevel,
                    SqlServerFeatureDataProviderTests.MockLayerId,
                    null,
                    null,
                    null).Result;
            }
            catch (TileFeatureDataProviderException tileFeatureDataProviderException)
            {
                exceptionHandled =
                   tileFeatureDataProviderException.TileFeatureDataProviderType == typeof(SqlServerFeatureDataProvider) &&
                   tileFeatureDataProviderException.TileFeatureDataProviderExceptionCategory == TileFeatureDataProviderExceptionCategory.SqlServerError;
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests the get tile feature data method when there's an unexpected error
        /// </summary>
        //[TestMethod]
        public void Test_GetTileFeaturaData_OtherException()
        {
            //Arrange
            //Arrange
            Exception unexpectedException = new Exception();

            TileFeatureDataDbContext dbContext = TileFeatureDataDbContextMockFactory.CreateMockDbContext(unexpectedException).Object;

            IFactory<TileFeatureDataDbContext> dbContextFactory = TileFeatureDataDbContextMockFactory.CreateTileDbContextFactory(dbContext);


            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;

            SqlServerFeatureDataProvider tileFeatureDataProvider = new SqlServerFeatureDataProvider(dbContextFactory, logger);

            //Act          
            bool exceptionHandled = false;

            //Act
            try
            {
                FeatureDataResponse response = tileFeatureDataProvider.GetTileFeatureData(
                    SqlServerFeatureDataProviderTests.MockExtent,
                    SqlServerFeatureDataProviderTests.ZoomLevel,
                    SqlServerFeatureDataProviderTests.MockLayerId,
                    null,
                    null,
                    null).Result;
            }
            catch (TileFeatureDataProviderException)
            {
            }
            catch (Exception ex)
            {
                exceptionHandled = unexpectedException == ex;
            }


            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        #endregion

        private static void AssertParcelFeatureAreEqual(ParcelFeature expectedFeature, ParcelFeatureData expectedFeatureData, dynamic actualFeature)
        {
            Assert.IsTrue(
                expectedFeature.Pin == actualFeature.PinNumeric &&
                expectedFeatureData.RealPropId == actualFeature.RealPropId &&
                expectedFeatureData.LandId == actualFeature.LandId &&
                expectedFeatureData.Major == actualFeature.Major &&
                expectedFeature.Major == actualFeature.Major &&
                expectedFeatureData.Minor == actualFeature.Minor &&
                expectedFeature.Minor == actualFeature.Minor &&
                expectedFeatureData.LandValSqFt == actualFeature.LandValSqFt &&
                expectedFeatureData.PropTypeNumeric == actualFeature.PropTypeNumeric &&
                expectedFeatureData.GeneralClassif == actualFeature.GeneralClassif &&
                expectedFeatureData.TaxpayerName == actualFeature.TaxpayerName &&
                expectedFeatureData.TaxStatus == actualFeature.TaxStatus &&
                expectedFeatureData.BldgGrade == actualFeature.BldgGrade &&
                expectedFeatureData.CondDescr == actualFeature.CondDescr &&
                expectedFeatureData.YrBltRen == actualFeature.YrBltRen &&
                expectedFeatureData.SqFtTotLiving == actualFeature.SqFtTotLiving &&
                expectedFeatureData.CurrentZoning == actualFeature.CurrentZoning &&
                expectedFeatureData.SqFtLot == actualFeature.SqFtLot &&
                expectedFeatureData.WfntLabel == actualFeature.WfntLabel &&
                expectedFeatureData.LandProbDescrPart1 == actualFeature.LandProbDescrPart1 &&
                expectedFeatureData.LandProbDescrPart2 == actualFeature.LandProbDescrPart2 &&
                expectedFeatureData.ViewDescr == actualFeature.ViewDescr &&
                expectedFeatureData.BaseLandValTaxYr == actualFeature.BaseLandValTaxYr &&
                expectedFeatureData.BaseLandVal == actualFeature.BaseLandVal &&
                expectedFeatureData.BLVSqFtCalc == actualFeature.BLVSqFtCalc &&
                expectedFeatureData.LandVal == actualFeature.LandVal &&
                expectedFeatureData.ImpsVal == actualFeature.ImpsVal &&
                expectedFeatureData.TotVal == actualFeature.TotVal &&
                expectedFeatureData.PrevLandVal == actualFeature.PrevLandVal &&
                expectedFeatureData.PrevImpsVal == actualFeature.PrevImpsVal &&
                expectedFeatureData.PrevTotVal == actualFeature.PrevTotVal &&
                expectedFeatureData.PcntChgLand == actualFeature.PcntChgLand &&
                expectedFeatureData.PcntChgImps == actualFeature.PcntChgImps &&
                expectedFeatureData.PcntChgTotal == actualFeature.PcntChgTotal &&
                expectedFeatureData.AddrLine == actualFeature.AddrLine &&
                expectedFeatureData.PropType == actualFeature.PropType &&
                expectedFeatureData.ApplGroup == actualFeature.ApplGroup &&
                expectedFeatureData.ResAreaSub == actualFeature.ResAreaSub &&
                expectedFeatureData.PresentUse == actualFeature.PresentUse &&
                expectedFeatureData.CmlPredominantUse == actualFeature.CmlPredominantUse &&
                expectedFeatureData.CmlNetSqFtAllBldg == actualFeature.CmlNetSqFtAllBldg &&
                expectedFeatureData.GeoAreaNbhd == actualFeature.GeoAreaNbhd &&
                expectedFeatureData.SpecAreaNbhd == actualFeature.SpecAreaNbhd &&
                expectedFeatureData.NbrResAccys == actualFeature.NbrResAccys &&
                expectedFeatureData.NbrCmlAccys == actualFeature.NbrCmlAccys &&
                expectedFeatureData.NewBLVSqFtCalc == actualFeature.NewBLVSqFtCalc &&
                expectedFeatureData.PrevLandValSqFt == actualFeature.PrevLandValSqFt &&
                expectedFeatureData.CondoYrBuilt == actualFeature.CondoYrBuilt &&
                expectedFeatureData.NewBaseLandVal == actualFeature.NewBaseLandVal &&
                expectedFeatureData.PropName == actualFeature.PropName &&
                expectedFeatureData.BLVPcntChg == actualFeature.BLVPcntChg &&
                expectedFeatureData.TrafficValDollars == actualFeature.TrafficValDollars &&
                expectedFeatureData.SpecArea == actualFeature.SpecArea &&
                expectedFeatureData.TSP_EMV == actualFeature.TSP_EMV);
        }
    }
}