using PTASMapTileFunctions.Exception;
using PTASMapTileServicesLibrary.TileProvider.Blob;
using PTASMapTileServicesLibrary.TileProvider.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using PTASWebMappingUnitTestCommon;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer;

namespace PTASMapTileFunctionsUnitTest.ExceptionTests
{
    [TestClass]
    public class MapTileFunctionsExceptionHandlerTests
    {
        private const string ExParameterName = "ex";
        private const string ReqParameterName = "req";
        private const string LogParameterName = "log";

        private const string MockErrorMessage = "mockMessage";

        /// <summary>
        /// Tests handling of untyped exceptions
        /// </summary>
        [TestMethod]
        public void TestHandleUntypedException()
        {
            //Arrange            
            System.Exception exception = new System.Exception();
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();

            //Act
            ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleUntypedException(exception, httpRequest.Object, logger.Object);

            //Assert
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.InternalServerError);
            Assert.IsInstanceOfType(result.Value, typeof(ErrorResultModel));
        }

        /// <summary>
        /// Tests handling of untyped exceptions when exception parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleUntypedException_NullExceptionParameter()
        {
            //Arrange                        
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleUntypedException(null, httpRequest.Object, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == MapTileFunctionsExceptionHandlerTests.ExParameterName)
                {
                    exceptionHandled = true;
                }
            }
            
            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests handling of untyped exceptions when request parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleUntypedException_NullRequestParameter()
        {
            //Arrange                        
            System.Exception exception = new System.Exception();
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleUntypedException(exception, null, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == MapTileFunctionsExceptionHandlerTests.ReqParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests handling of untyped exceptions when logger parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleUntypedException_NullLoggerParameter()
        {
            //Arrange                        
            System.Exception exception = new System.Exception();            
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleUntypedException(exception, httpRequest.Object, null);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == MapTileFunctionsExceptionHandlerTests.LogParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        #region TileProviderException

        /// <summary>
        /// Tests handling of tile provider exceptions.  This is testing that we are handling tile provider exceptions appropriately by 
        /// checking them and returning an internal server error.
        /// </summary>
        [TestMethod]
        public void TestHandleTileProviderException()
        {
            //Arrange            
            TileProviderException exception = new TileProviderException(
                TileProviderExceptionCategory.CloudStorageError, 
                typeof(BlobTileProvider), 
                MapTileFunctionsExceptionHandlerTests. MockErrorMessage,
                new System.Exception());

            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();

            //Act
            ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleTileProviderException(exception, httpRequest.Object, logger.Object);

            //Assert
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.InternalServerError);
            Assert.IsInstanceOfType(result.Value, typeof(ErrorResultModel));
        }

        /// <summary>
        /// Tests handling of tile provider exceptions when exception parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleTileProviderException_NullExceptionParameter()
        {
            //Arrange                        
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleTileProviderException(null, httpRequest.Object, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == MapTileFunctionsExceptionHandlerTests.ExParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests handling of tile provider exceptions when request parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleTileProviderException_NullRequestParameter()
        {
            //Arrange                        
            TileProviderException exception = new TileProviderException(
               TileProviderExceptionCategory.CloudStorageError,
               typeof(BlobTileProvider),
               MapTileFunctionsExceptionHandlerTests.MockErrorMessage,
               new System.Exception());

            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleTileProviderException(exception, null, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == MapTileFunctionsExceptionHandlerTests.ReqParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests handling of tileprovider exceptions when logger parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleTileProviderException_NullLoggerParameter()
        {
            //Arrange                        
            TileProviderException exception = new TileProviderException(
               TileProviderExceptionCategory.CloudStorageError,
               typeof(BlobTileProvider),
               MapTileFunctionsExceptionHandlerTests.MockErrorMessage,
               new System.Exception());

            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleTileProviderException(exception, httpRequest.Object, null);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == MapTileFunctionsExceptionHandlerTests.LogParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        #endregion TileProviderException

        #region TileFeatureDataProviderException

        /// <summary>
        /// Tests handling of tile feature data provider exceptions.  This is testing that we are handling tile provider exceptions appropriately by 
        /// checking them and returning an internal server error.
        /// </summary>
        [TestMethod]
        public void TestHandleTileFeatureDataProviderException()
        {
            //Arrange            
            TileFeatureDataProviderException exception = new TileFeatureDataProviderException(
                TileFeatureDataProviderExceptionCategory.SqlServerError,
                typeof(SqlServerFeatureDataProvider),
                MapTileFunctionsExceptionHandlerTests.MockErrorMessage,
                new System.Exception());

            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();

            //Act
            ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleTileFeatureDataProviderException(exception, httpRequest.Object, logger.Object);

            //Assert
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.InternalServerError);
            Assert.IsInstanceOfType(result.Value, typeof(ErrorResultModel));
        }

        /// <summary>
        /// Tests handling of tile feature data provider exceptions when exception parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleTileFeatureDataProviderException_NullExceptionParameter()
        {
            //Arrange                        
            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleTileFeatureDataProviderException(null, httpRequest.Object, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == MapTileFunctionsExceptionHandlerTests.ExParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests handling of tile feature data provider exceptions when request parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleTileFeatureDataProviderException_NullRequestParameter()
        {
            //Arrange                        
            TileFeatureDataProviderException exception = new TileFeatureDataProviderException(
               TileFeatureDataProviderExceptionCategory.SqlServerError,
               typeof(SqlServerFeatureDataProvider),
               MapTileFunctionsExceptionHandlerTests.MockErrorMessage,
               new System.Exception());

            Mock<ILogger> logger = WebMappingMockFactory.CreateMockLogger();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleTileFeatureDataProviderException(exception, null, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == MapTileFunctionsExceptionHandlerTests.ReqParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests handling of tile feature data provider exceptions when logger parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleTileFeatureDataProviderException_NullLoggerParameter()
        {
            //Arrange                        
            TileFeatureDataProviderException exception = new TileFeatureDataProviderException(
                TileFeatureDataProviderExceptionCategory.SqlServerError,
                typeof(SqlServerFeatureDataProvider),
                MapTileFunctionsExceptionHandlerTests.MockErrorMessage,
                new System.Exception());

            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)MapTileFunctionsExceptionHandler.HandleTileFeatureDataProviderException(exception, httpRequest.Object, null);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == MapTileFunctionsExceptionHandlerTests.LogParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        #endregion TileProviderException
    }
}
