using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ILinxSoapImport;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Moq.Protected;

using PTASILinxConnectorHelperClasses.Models;

using PTASIlinxService.Controllers;

using PTASLinxConnectorHelperClasses.Models;

using PTASServicesCommon.CloudStorage;

namespace PTASILinxService_UnitTests.ControllerTests
{
    [TestClass]
    public class BlobMoveControllerTests
    {
        [TestMethod]
        public async Task TestCallsBlob()
        {
            // Arrange
            var storageMock = new Mock<ICloudStorageProvider>();
            var iLinxMock = new Mock<IILinxHelper>();
            var configMock = new Mock<IConfigParams>();
            var calledBlobContainer = false;
            var calledILinx = false;
            var calledDelete = false;
            var calledMarkRecord = false;

            var mockController = new Mock<BlobMoveController>(storageMock.Object, iLinxMock.Object, configMock.Object);

            // will return a blob list with no blobs.
            mockController.Protected()
              .Setup<List<BlobDocumentContainer>>("GetBlobs", It.IsAny<Guid>())
              .Returns(() =>
              {
                  calledBlobContainer = true;
                  return new List<BlobDocumentContainer>() {
            new BlobDocumentContainer(Guid.Empty, Guid.Empty, string.Empty,string.Empty,string.Empty, new List<BlobFileDetails>())
                };
              });

            mockController.Protected()
              .Setup<(BlobDocumentContainer blobContainer, InsertResponse insertResponse)[]>("InsertIntoTarget", ItExpr.IsAny<BlobMoveInfo>(), ItExpr.IsAny<List<BlobDocumentContainer>>()).Returns(() =>
              {
                  calledILinx = true;
                  return new (BlobDocumentContainer blobContainer, InsertResponse insertResponse)[] { (default, new InsertResponse { Error = false, AssignedId = "0", ErrorMessage = "" }) };
              });

            mockController
              .Protected()
              .Setup("DeleteMovedBlobs", ItExpr.IsAny<List<BlobDocumentContainer>>(), ItExpr.IsAny<bool[]>()).Callback(() =>
            {
                calledDelete = true;
            });

            mockController.Protected().Setup("MarkAllAsDone", ItExpr.IsAny<(BlobDocumentContainer documentContainer, InsertResponse insertResponse)[]>()).Callback(((BlobDocumentContainer documentContainer, InsertResponse insertResponse)[] insertResult) =>
            {
                calledMarkRecord = true;
            });

            // Act
            var result = await mockController.Object.PostAsync(new BlobMoveInfo
            {
                AccountNumber = default,
                BlobId = default,
                DocType = default,
                RecId = default,
                RollYear = default
            });

            // Assert
            Assert.IsTrue(calledBlobContainer, "Should call the blob container for info.");
            Assert.IsTrue(calledILinx, "Should have called ILinx interface.");
            Assert.IsTrue(calledMarkRecord, "Should have marked the record as processed.");
            Assert.IsTrue(calledDelete, "Should have called delete.");
        }

        [TestMethod]
        public async Task TestReturnsErrorOnNoFiles()
        {
            // Arrange
            var storageMock = new Mock<ICloudStorageProvider>();
            var iLinxMock = new Mock<IILinxHelper>();
            var configMock = new Mock<IConfigParams>();

            var x = new Mock<BlobMoveController>(storageMock.Object, iLinxMock.Object, configMock.Object);

            // will return a blob list with no blobs.
            x.Protected()
              .Setup<List<BlobDocumentContainer>>("GetBlobs", It.IsAny<Guid>())
              .Returns(new List<BlobDocumentContainer>() { });

            // Act
            var result = await x.Object.PostAsync(new BlobMoveInfo
            {
                AccountNumber = default,
                BlobId = default,
                DocType = default,
                RecId = default,
                RollYear = default
            });

            // Assert
            Assert.IsTrue(result.Error, "Should return an error when no files present.");
        }
    }
}
