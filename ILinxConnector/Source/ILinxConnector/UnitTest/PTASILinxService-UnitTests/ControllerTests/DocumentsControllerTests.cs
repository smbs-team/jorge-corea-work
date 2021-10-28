using System;
using System.Collections.Specialized;
using ILinxSoapImport;
using ILinxSoapImport.EdmsService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PTASILinxConnectorHelperClasses.Models;
using PTASIlinxService.Controllers;
using PTASLinxConnectorHelperClasses.Models;

namespace PTASILinxService_UnitTests.ControllerTests
{
    [TestClass]
    public class DocumentsControllerTests
    {

        [TestMethod]
        public void TestPost()
        {
            // Arrange
            var cacheManager = new MoqCacheMananger<DocumentResult>();
            var moqILinx = new Mock<IILinxHelper>();
            var triedToSaveToLinx = false;
            InsertResponse expectedResponse = new InsertResponse
            {
                AssignedId = EmptyGuid(),
                Error = false,
                ErrorMessage = "No Error"
            };
            moqILinx.Setup(ml => ml.SaveDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ReceivedFileInfo[]>()))
              .Returns(() =>
              {
                  triedToSaveToLinx = true;
                  return expectedResponse;
              });

            var controller = new Mock<DocumentsController>(moqILinx.Object, cacheManager);
            NameValueCollection collection = new NameValueCollection {
        { "accountNumber", It.IsAny<string>() },
        { "rollYear", It.IsAny<string>() },
        { "docType", It.IsAny<string>() },
        { "recId", It.IsAny<string>() },
      };
            controller.Protected().Setup<NameValueCollection>("GetFormValues").Returns(collection);
            controller.Protected().Setup<ReceivedFileInfo[]>("GetFiles").Returns((ReceivedFileInfo[])null);

            // Act
            var result = controller.Object.Post();

            // Assert
            Assert.AreSame(expectedResponse, result);
            Assert.IsTrue(triedToSaveToLinx);
        }


        [TestMethod]
        public void TestPatch()
        {
            // Arrange
            var cacheManager = new MoqCacheMananger<DocumentResult>();
            var moqILinx = new Mock<IILinxHelper>();
            var triedToUpdateDocument = false;

            InsertResponse expectedResponse = new InsertResponse
            {
                AssignedId = EmptyGuid(),
                Error = false,
                ErrorMessage = "No Error"
            };

            moqILinx.Setup(ml => ml.UpdateDocument(It.IsAny<Guid>(), It.IsAny<ReceivedFileInfo[]>()))
              .Returns(() =>
              {
                  triedToUpdateDocument = true;
                  return expectedResponse;
              });

            var controller = new Mock<DocumentsController>(moqILinx.Object, cacheManager);
            NameValueCollection collection = new NameValueCollection { { "DocumentId", EmptyGuid() } };
            controller.Protected().Setup<NameValueCollection>("GetFormValues").Returns(collection);
            controller.Protected().Setup<ReceivedFileInfo[]>("GetFiles").Returns((ReceivedFileInfo[])null);

            // Act
            var result = controller.Object.Patch();

            // Assert
            Assert.AreSame(expectedResponse, result);
            Assert.IsTrue(triedToUpdateDocument);
        }

        private static string EmptyGuid() => ((Guid)default).ToString();

        [TestMethod]
        public void TestGetFromCache()
        {
            var cacheManager = new MoqCacheMananger<DocumentResult>();
            var moqILinx = new Mock<IILinxHelper>();
            var calledILinx = false;
            DocumentResult documentResult = new DocumentResult
            {
                DocumentId = "123",
                FileCount = 1,
                Files = new FileDetails[] { }
            };

            cacheManager.Get("123", 100, () => documentResult);

            moqILinx.Setup(ml => ml.FetchDocument(It.IsAny<string>(), false)).Returns(() =>
             {
                 // should never get here.
                 calledILinx = true;
                 return null;
             });
            var controller = new DocumentsController(moqILinx.Object, cacheManager);
            var result = controller.Get("123");

            Assert.AreSame(result, documentResult);
            Assert.IsFalse(calledILinx);
        }

        [TestMethod]
        public void TestGetFromLinx()
        {
            // Arrange
            var cacheManager = new MoqCacheMananger<DocumentResult>();
            var moqILinx = new Mock<IILinxHelper>();
            var calledILinx = false;
            DocumentResult documentResult = new DocumentResult
            {
                DocumentId = "123",
                FileCount = 1,
                Files = new FileDetails[] { }
            };

            moqILinx.Setup(ml => ml.FetchDocument(It.IsAny<string>(), false)).Returns(() =>
             {
                 calledILinx = true;
                 return documentResult;
             });
            var controller = new DocumentsController(moqILinx.Object, cacheManager);

            // Act
            var result = controller.Get("123");

            // Assert
            Assert.AreSame(result, documentResult);
            Assert.IsTrue(calledILinx);
        }
    }


}
