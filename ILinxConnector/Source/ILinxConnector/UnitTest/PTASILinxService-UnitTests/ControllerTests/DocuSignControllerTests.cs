using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PTASIlinxService.Controllers;
using PTASLinxConnectorHelperClasses.Models;
using PTASServicesCommon.CloudStorage;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;

namespace PTASILinxService_UnitTests.ControllerTests
{
  [TestClass]
  public class DocuSignControllerTests
  {
    [TestMethod]
    public void TestConstructor()
    {
      var config = new Mock<IConfigParams>();
      var cloud = new Mock<ICloudStorageProvider>();
      var mockController = new Mock<DocuSignController>(config.Object, cloud.Object);
      var mockFile = createMockFile();

      mockController.Protected().Setup<NameValueCollection>("GetForm").Returns(new NameValueCollection() { });
      Mock<HttpFileCollectionBase> mockCollection = CreateFileCollection(new HttpPostedFileBase[] { mockFile.Object });

      mockController.Protected().Setup<object>("CreateApiClient").Returns(null);

      mockController.Protected().Setup<HttpFileCollectionBase>("GetFiles").Returns(mockCollection.Object);
      var result = mockController.Object.Post();
      // Must fail on empty form.
      Assert.IsTrue(result.Error == true);
    }

    private static Mock<HttpFileCollectionBase> CreateFileCollection(HttpPostedFileBase[] array)
    {
      var mockCollection = new Mock<HttpFileCollectionBase>();
      mockCollection.Setup(coll => coll.GetEnumerator()).Returns(array.Select(f => f.FileName).GetEnumerator());
      mockCollection.Setup(coll => coll.Count).Returns(array.Count());
      mockCollection.Setup(coll => coll[It.IsAny<int>()]).Returns((int i) => array[i]);
      return mockCollection;
    }

    private static Mock<HttpPostedFileBase> createMockFile()
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