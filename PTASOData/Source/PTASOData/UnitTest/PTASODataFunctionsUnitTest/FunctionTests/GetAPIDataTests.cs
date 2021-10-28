namespace PTASMapTileFunctionsUnitTest.FunctionTests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Linq;
    using System.Net;
    using PTASServicesCommon.DependencyInjection;
    using PTASODataFunctions.Functions;
    using PtasUnitTestCommon;
    using PTASODataLibrary.PtasDbDataProvider.EntityFramework;
    using PTASODataUnitTestCommon.Helper;
    using PTASODataLibraryUnitTest.Helper;
    using PTASODataFunctions.Exception;
    using System.Text;
    using PTASODataLibrary.PtasDbDataProvider.PtasCamaModel;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Tests the GetTileFeatureData function
    /// </summary>
    [TestClass]
    public class GetAPIDataTests
    {
        private const string DbContextParameterName = "dbContext";
        private const string MetadataResource = "$metadata";
        private const string ParcelResource = "PtasParceldetail";
        


        /// <summary>
        /// Tests constructor when dbContextFactory is null
        /// </summary>
        [TestMethod]
        public void Test_Constructor_DBContextNull()
        {
            //Arrange                                   
            bool argumentException = false;

            //Act
            try
            {
                GetAPIData apiData = new GetAPIData(null);
            } catch (System.ArgumentNullException ex)
            {
                argumentException = ex.ParamName == "dbContextFactory";
            }

            //Assert
            Assert.IsTrue(argumentException);
        }

        #region GetAPIData Tests

        /// <summary>
        /// Tests the GetAPIData method with an empty resource.
        /// </summary>
        [TestMethod]
        public void Test_GetAPIData_EmptyResource()
        {
            //Arrange                                   
            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContex);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;

            GetAPIData apiData = new GetAPIData(dbContextFactory);

            //Act
            ObjectResult result = apiData.Run(request, string.Empty, log).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);

            ErrorResultModel resultValue = result.Value as ErrorResultModel;
            Assert.IsNotNull(resultValue);            
            Assert.IsTrue(resultValue.Message.Contains("resource"));
        }

        /// <summary>
        /// Tests the GetAPIData method with a request for metadata.
        /// </summary>
        [TestMethod]
        public void Test_GetAPIData_Metadata()
        {
            //Arrange                                   
            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContex);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            GetAPIData apiData = new GetAPIData(dbContextFactory);

            //Act
            FileContentResult result = apiData.Run(request, GetAPIDataTests.MetadataResource, log).Result as FileContentResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.FileContents);
            string fileContents = Encoding.UTF8.GetString(result.FileContents);

            //Test that some of the entities exist in the medatada.
            Assert.IsTrue(fileContents.Contains(GetAPIDataTests.GetExpectedEntityMetadata<PtasPropertytype>()));
            Assert.IsTrue(fileContents.Contains(GetAPIDataTests.GetExpectedEntityMetadata<PtasUnitbreakdown>()));
            Assert.IsTrue(fileContents.Contains(GetAPIDataTests.GetExpectedEntityMetadata<PtasProjectdock>()));
        }

        /// <summary>
        /// Tests the GetAPIData method with a request for metadata (for cama and historical).
        /// </summary>
        [TestMethod]
        public void Test_GetAPIData_Metadata_CamaAndHistorical()
        {
            //Arrange                                   
            var dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyCamaAndHistoricalDbContext();
            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContex);
            HttpRequest request = RequestFactory.CreateFakeODataRequest(string.Empty, string.Empty);
            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCamaAndHistoricalDbContext));

            GetAPIData apiData = new GetAPIData(dbContextFactory);

            //Act
            FileContentResult result = apiData.Run(request, GetAPIDataTests.MetadataResource, log).Result as FileContentResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.FileContents);
            string fileContents = Encoding.UTF8.GetString(result.FileContents);

            //Test that some of the entities exist in the medatada.
            Assert.IsTrue(fileContents.Contains(GetAPIDataTests.GetExpectedEntityMetadata<PtasPropertytype>()));
            Assert.IsTrue(fileContents.Contains(GetAPIDataTests.GetExpectedEntityMetadata<PtasUnitbreakdown>()));
            Assert.IsTrue(fileContents.Contains(GetAPIDataTests.GetExpectedEntityMetadata<PtasProjectdock>()));
        }

        /// <summary>
        /// Tests the GetAPIData method with a request for resource.
        /// </summary>
        [TestMethod]
        public void Test_GetAPIData_Resource()
        {
            //Arrange                                   
            PTASCAMADbContext dbContex = PTASCAMADataDbContextMockFactory.CreateInMemoryEmptyDbContext();
            int testEntityCount = 25;
            PTASCAMADataDbContextMockFactory.AddMockParcelDetails(dbContex, testEntityCount);

            IFactory<DbContext> dbContextFactory = MockDbFactory.CreateDbContextFactory<DbContext>(dbContex);
            HttpRequest request = RequestFactory.CreateFakeODataRequest("PtasParceldetail", "");
            ILogger log = MockObjectFactory.CreateMockLogger().Object;
            DbContextExtensions.Reset();
            DbContextExtensions.RegisterDbContext(typeof(PTASCAMADbContext));

            GetAPIData apiData = new GetAPIData(dbContextFactory);

            //Act
            FileContentResult result = apiData.Run(request, GetAPIDataTests.ParcelResource, log).Result as FileContentResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("application/json", result.ContentType);
            Assert.IsNotNull(result.FileContents);

            string fileContentsAsText = Encoding.UTF8.GetString(result.FileContents);
            
            for (int i = 0; i < testEntityCount; i++)
            {
                Assert.IsTrue(fileContentsAsText.Contains(GetAPIDataTests.GetExpectedJsonField("PtasMajor", i.ToString())));
            }
        }

        #endregion GetAPIData Tests

        /// <summary>
        /// Gets the expected entity metadata.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetExpectedJsonField(string expectedJsonFieldName, string expectedJsonFieldValue)
        {
            return "\"" + expectedJsonFieldName + "\":\"" + expectedJsonFieldValue + "\"";
        }


        /// <summary>
        /// Gets the expected entity metadata.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetExpectedEntityMetadata<T>()
        {
            return "<EntityType Name=\"" + typeof(T).Name + "\">";
        }
    }
}
