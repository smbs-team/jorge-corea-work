using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using Moq.Protected;
using PTASILinxConnectorHelperClasses.Models;
using PTASIlinxService.Controllers;
using PTASLinxConnectorHelperClasses.Models;
using PTASServicesCommon.CloudStorage;

namespace PTASILinxService_UnitTests.ControllerTests
{
    [TestClass]
  public class AzureFileBlobControllerTests
  {
    private const string AnyFile = @"c:\tmp";

    [TestMethod]
    public void TestConstructor()
    {
      // arrange
      var configMock = new Mock<IConfigParams>();
      var providerMock = new Mock<ICloudStorageProvider>();
      var ImgAnalyzerMock = new Mock<IImageAnalysisHelper>();
      var failedOnMissingConfig = false;
      var failedOnMissingProvider = false;
      var failedOnMissingImgAnalizer = false;

      // act
      try
      {
        var x = new AzureFileBlobController(null, providerMock.Object, ImgAnalyzerMock.Object);
      }
      catch (ArgumentException ex) when (ex.ParamName == "config")
      {
        failedOnMissingConfig = true;
      }

      try
      {
        var x = new AzureFileBlobController(configMock.Object, null, ImgAnalyzerMock.Object);
      }
      catch (ArgumentException ex) when (ex.ParamName == "cloudProvider")
      {
        failedOnMissingProvider = true;
      }

      try
      {
        var x = new AzureFileBlobController(configMock.Object, providerMock.Object, null);
      }
      catch (ArgumentException ex) when (ex.ParamName == "imageAnalizer")
      {
        failedOnMissingImgAnalizer = true;
      }

      // assert
      Assert.IsTrue(failedOnMissingConfig && failedOnMissingProvider && failedOnMissingImgAnalizer, "Should have failed on any of the missing constructor params.");
    }

    [TestMethod]
    public async Task TestFailOnNoFilesAsync()
    {
      // Arrange
      var handledNoFilesWithException = false;
      Mock<HttpFileCollectionBase> mockCollection = CreateFileCollection(new HttpPostedFileBase[] { });
      Mock<IImageAnalysisHelper> imageAnalyzerMock = CreateImageAnalizer();
      var x = new Mock<AzureFileBlobController>(new Mock<IConfigParams>().Object, new Mock<ICloudStorageProvider>().Object, imageAnalyzerMock.Object);
      x.Protected().Setup<NameValueCollection>("GetForm").Returns(() =>
      {
        return new NameValueCollection { };
      });
      x.Protected().Setup<string>("GetLocalPath", ItExpr.IsAny<Uri>()).Returns(() =>
      {
        return AnyFile;
      });
      x.Protected().Setup<string>("GetAbsoluteUri", ItExpr.IsAny<Uri>()).Returns(() =>
{
  return AnyFile;
});
      x.Protected().Setup<HttpFileCollectionBase>("GetFiles").Returns(mockCollection.Object);

      // Act
      var response = await x.Object.PostAsync();
      handledNoFilesWithException = (response.Error && response.ErrorMessage.Contains("Wrong number of files"));

      // Assert
      Assert.IsTrue(handledNoFilesWithException, "Should have alerted that no files were passed.");
    }


    [TestMethod]
    public async Task TestSaveIsCalled()
    {
      // Arrange

      var calledSave = false;
      Mock<HttpPostedFileBase> mockFile = CreateMockFile();

      Mock<HttpFileCollectionBase> mockCollection = CreateFileCollection(new HttpPostedFileBase[] { mockFile.Object });

      Mock<IImageAnalysisHelper> imageAnalyzerMock = CreateImageAnalizer();
      var mockController = new Mock<AzureFileBlobController>(new Mock<IConfigParams>().Object, new Mock<ICloudStorageProvider>().Object, imageAnalyzerMock.Object);
      mockController.Protected().Setup<Uri>("GetRequestUri").Returns(new Uri("http://tempuri.org/blob"));

      mockController.Protected().Setup<HttpFileCollectionBase>("GetFiles").Returns(mockCollection.Object);
      mockController.Protected().Setup<string>("GetLocalPath", ItExpr.IsAny<Uri>()).Returns(AnyFile);
      mockController.Protected().Setup<string>("GetAbsoluteUri", ItExpr.IsAny<Uri>()).Returns(AnyFile);


      mockController.Protected().Setup<NameValueCollection>("GetForm").Returns(() =>
      {
        return new NameValueCollection {
          { "seniorApplicationId", "123" },
          { "section", "123" },
          { "document", "123" } };
      });

      IEnumerable<UploadFileResult> uploadResults = new List<UploadFileResult> { };

      mockController.Protected()
        .Setup<Task<IEnumerable<UploadFileResult>>>(
          "SaveFiles",
          ItExpr.IsAny<HttpFileCollectionBase>(),
          ItExpr.IsAny<string>(),
          ItExpr.IsAny<CloudBlobContainer>(),
          ItExpr.IsAny<bool>()
        )
        .Callback<HttpFileCollectionBase, string, CloudBlobContainer, bool>((a, b, c, d) =>
           {
             calledSave = true;
             Task.FromResult(0);
           }).Returns(Task.FromResult(uploadResults));

      mockController.Protected().Setup<HttpFileCollectionBase>("GetFiles").Returns(mockCollection.Object);

      // Act
      var response = await mockController.Object.PostAsync();

      // Assert
      Assert.IsTrue(calledSave);
    }

    private static Mock<HttpFileCollectionBase> CreateFileCollection(HttpPostedFileBase[] array)
    {
      var mockCollection = new Mock<HttpFileCollectionBase>();
      mockCollection.Setup(coll => coll.GetEnumerator()).Returns(array.Select(f => f.FileName).GetEnumerator());
      mockCollection.Setup(coll => coll.Count).Returns(array.Count());
      mockCollection.Setup(coll => coll[It.IsAny<int>()]).Returns((int i) => array[i]);
      return mockCollection;
    }

    private static Mock<IImageAnalysisHelper> CreateImageAnalizer()
    {
      Mock<IImageAnalysisHelper> imageAnalyzerMock = new Mock<IImageAnalysisHelper>();
      imageAnalyzerMock.Setup(t => t.ImageIsAcceptable(It.IsAny<byte[]>())).Returns(async () =>
      {
        return await Task.FromResult((true, ""));
      });
      return imageAnalyzerMock;
    }

    private static Mock<HttpPostedFileBase> CreateMockFile()
    {
      var mockFile = new Mock<HttpPostedFileBase>();
      mockFile.Setup(p => p.FileName).Returns("file.tiff");
      mockFile.Setup(p => p.ContentLength).Returns(0);
      mockFile.Setup(p => p.ContentType).Returns("image/png");
      mockFile.Setup(p => p.InputStream).Returns(new MemoryStream());
      return mockFile;
    }
    private static Mock<HttpPostedFileBase> GetMockFile()
    {
      var mockFile = new Mock<HttpPostedFileBase>();
      mockFile.Setup(p => p.FileName).Returns("file.tiff");
      mockFile.Setup(p => p.ContentLength).Returns(0);
      mockFile.Setup(p => p.ContentType).Returns("image/png");
      mockFile.Setup(p => p.InputStream).Returns(new MemoryStream());
      return mockFile;
    }
  }
}
