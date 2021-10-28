using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using Moq.Protected;
using PTASIlinxService.Classes;
using PTASIlinxService.Controllers;
using PTASLinxConnectorHelperClasses.Models;
using PTASServicesCommon.CloudStorage;
using System;
using System.IO;

namespace PTASILinxService_UnitTests.ControllerTests
{
    [TestClass]
  public class BlobFilesControllerTest
  {
    private const string tempUri = "http://tempuri.org/blob";

    [TestMethod]
    public void TestDelete()
    {
      // Arrange
      var CheckedExistence = false;
      var CalledDelete = false;

      var mockCloud = new Mock<ICloudStorageProvider>();
      Uri uri = new Uri(tempUri);
      var mockContainer = new Mock<CloudBlobContainer>(uri);
      var mockCloudBlockBlob = new Mock<CloudBlockBlob>(uri);

      mockContainer.Setup(m => m.GetBlockBlobReference(It.IsAny<string>())).Returns(() =>
      {
        return mockCloudBlockBlob.Object;
      });

      mockCloudBlockBlob.Setup(m => m.Exists(It.IsAny<BlobRequestOptions>(), It.IsAny<OperationContext>())).Returns(() =>
      {
        CheckedExistence = true;
        return true;
      });

      mockCloudBlockBlob.Setup(m => m.Delete(It.IsAny<DeleteSnapshotsOption>(), It.IsAny<AccessCondition>(), It.IsAny<BlobRequestOptions>(), It.IsAny<OperationContext>())).Callback(() =>
      {
        CalledDelete = true;
      });

      mockCloud
        .Setup(m => m.GetCloudBlobContainer(It.IsAny<string>()))
        .Returns(() =>
        {
          return mockContainer.Object;
        });

      var mockConfig = new Mock<IConfigParams>();
      var controller = new Mock<BlobFilesController>(mockCloud.Object, mockConfig.Object);
      // Act
      controller.Object.Delete("p1.p2.png");

      // Assert
      Assert.IsTrue(CheckedExistence, "Needed to check existence of blob.");
      Assert.IsTrue(CalledDelete, "Delete must be called to complete operation.");
    }

    [TestMethod]
    public void TestFetchImage()
    {
      // Arrange
      var mockCloud = new Mock<ICloudStorageProvider>();
      var mockConfig = new Mock<IConfigParams>();
      var controller = new Mock<BlobFilesController>(mockCloud.Object, mockConfig.Object);
      MemoryStream memoryStream = new MemoryStream();
      controller.Protected().Setup<(MemoryStream, string)>("GetBlobStream", true, new object[] { "0" }).Returns((memoryStream, "rewired.png"));

      // Act
      var result = controller.Object.Get("0");

      // Assert
      FileStreamResult typedResult = result as FileStreamResult;
      Assert.IsNotNull(typedResult, "Result cannot be null.");
      Assert.AreEqual("image/png", typedResult.ContentType);
      Assert.AreSame(memoryStream, typedResult.ContentStream);
    }
  }
}