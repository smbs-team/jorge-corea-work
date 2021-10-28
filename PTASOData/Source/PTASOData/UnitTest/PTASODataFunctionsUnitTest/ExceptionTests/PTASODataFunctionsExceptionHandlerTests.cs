namespace PTASMapTileFunctionsUnitTest.ExceptionTests
{
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Net;
    using PTASODataFunctions.Exception;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using PtasUnitTestCommon;

    [TestClass]
    public class PTASODataFunctionsExceptionHandlerTests
    {
        private const string ExParameterName = "ex";
        private const string ReqParameterName = "req";
        private const string LogParameterName = "log";

        private const string MockErrorMessage = "mockMessage";
        private const string TestParamName = "testParameterName";

        #region HandleUntypedException Tests

        /// <summary>
        /// Tests handling of untyped exceptions
        /// </summary>
        [TestMethod]
        public void TestHandleUntypedException()
        {
            //Arrange            
            System.Exception exception = new System.Exception();
            Mock<ILogger> logger = MockObjectFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();

            //Act
            ObjectResult result = (ObjectResult)PTASODataFunctionsExceptionHandler.HandleUntypedException(exception, httpRequest.Object, logger.Object);

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
            Mock<ILogger> logger = MockObjectFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)PTASODataFunctionsExceptionHandler.HandleUntypedException(null, httpRequest.Object, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == PTASODataFunctionsExceptionHandlerTests.ExParameterName)
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
            Mock<ILogger> logger = MockObjectFactory.CreateMockLogger();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)PTASODataFunctionsExceptionHandler.HandleUntypedException(exception, null, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == PTASODataFunctionsExceptionHandlerTests.ReqParameterName)
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
                ObjectResult result = (ObjectResult)PTASODataFunctionsExceptionHandler.HandleUntypedException(exception, httpRequest.Object, null);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == PTASODataFunctionsExceptionHandlerTests.LogParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        #endregion

        #region HandleArgumentNullException tests

        /// <summary>
        /// Tests handling of ArgumentNull exceptions
        /// </summary>
        [TestMethod]
        public void TestHandleArgumentNullException()
        {
            //Arrange            
            System.ArgumentNullException exception = new System.ArgumentNullException(PTASODataFunctionsExceptionHandlerTests.TestParamName);
            Mock<ILogger> logger = MockObjectFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();

            //Act
            ObjectResult result = (ObjectResult)PTASODataFunctionsExceptionHandler.HandleArgumentNullException(exception, httpRequest.Object, logger.Object);

            //Assert
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.BadRequest);

            ErrorResultModel resultValue = result.Value as ErrorResultModel;
            Assert.IsNotNull(resultValue);
            Assert.IsTrue(resultValue.Message.Contains(PTASODataFunctionsExceptionHandlerTests.TestParamName));
        }

        /// <summary>
        /// Tests handling of ArgumentNull exceptions when exception parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleArgumentNullException_NullExceptionParameter()
        {
            //Arrange                        
            Mock<ILogger> logger = MockObjectFactory.CreateMockLogger();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)PTASODataFunctionsExceptionHandler.HandleArgumentNullException(null, httpRequest.Object, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == PTASODataFunctionsExceptionHandlerTests.ExParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests handling of ArgumentNull exceptions when request parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleArgumentNullException_NullRequestParameter()
        {
            //Arrange                        
            System.ArgumentNullException exception = new System.ArgumentNullException();
            Mock<ILogger> logger = MockObjectFactory.CreateMockLogger();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)PTASODataFunctionsExceptionHandler.HandleArgumentNullException(exception, null, logger.Object);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == PTASODataFunctionsExceptionHandlerTests.ReqParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        /// <summary>
        /// Tests handling of ArgumentNull exceptions when logger parameter is null
        /// </summary>
        [TestMethod]
        public void TestHandleArgumentNullException_NullLoggerParameter()
        {
            //Arrange                        
            System.ArgumentNullException exception = new System.ArgumentNullException();
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            bool exceptionHandled = false;

            //Act
            try
            {
                ObjectResult result = (ObjectResult)PTASODataFunctionsExceptionHandler.HandleArgumentNullException(exception, httpRequest.Object, null);
            }
            catch (ArgumentNullException ex)
            {
                if (ex.ParamName == PTASODataFunctionsExceptionHandlerTests.LogParameterName)
                {
                    exceptionHandled = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionHandled);
        }

        #endregion
    }
}
