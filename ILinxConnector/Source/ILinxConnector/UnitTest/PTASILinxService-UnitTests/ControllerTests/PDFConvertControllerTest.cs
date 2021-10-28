using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Moq.Protected;

using PTASILinxConnectorHelperClasses.Models;

using PTASIlinxService.Classes.Exceptions;
using PTASIlinxService.Controllers;

using PTASLinxConnectorHelperClasses.Models;

using System;
using System.IO;
using System.Linq;
using System.Web;

namespace PTASILinxService_UnitTests.ControllerTests
{
    [TestClass]
    public class PDFConvertControllerTest
    {
        [TestMethod]
        public void TestConvertTiff()
        {
            var pdfFile = File.ReadAllBytes("./resources/input.pdf");

            var mockCollection = new Mock<HttpFileCollectionBase>();

            var mockFile = new Mock<HttpPostedFileBase>();
            mockFile.Setup(p => p.FileName).Returns("file.tiff");
            mockFile.Setup(p => p.ContentLength).Returns(pdfFile.Length);
            mockFile.Setup(p => p.ContentType).Returns("image/tiff");
            mockFile.Setup(p => p.InputStream).Returns(new MemoryStream(pdfFile));

            var array = new[] { mockFile.Object };
            mockCollection.Setup(x => x.GetEnumerator()).Returns(array.Select(f => f.FileName).GetEnumerator());
            mockCollection.Setup(x => x.Count).Returns(array.Count());
            mockCollection.Setup(x => x[It.IsAny<int>()]).Returns((int i) => array[i]);

            var mockController = new Mock<PDFConvertController>(new Mock<IConfigParams>().Object);

            mockController.Protected().Setup<HttpFileCollectionBase>("GetFiles").Returns(mockCollection.Object);
            mockController.Protected().Setup<Uri>("GetUri").Returns(new Uri("http://test.com/"));

            mockController.Protected()
              .Setup<HttpContext>("GetHttpContext")
              .Returns(new HttpContext(
                new HttpRequest("input.pdf", "https://www.tempuri.org/", ""),
                new HttpResponse(null)));

            // Act
            ConvertFileResults result = mockController.Object.Post();

            // Assert
            Assert.IsNotNull(result, "Result of image conversion cannot be null");
            Assert.AreEqual(result.Images.Count(), 4, 0, "Input file containes 4 images.");
            foreach (var image in result.Images)
            {
                Assert.IsTrue(image.Length > 0);
            }
        }

        [TestMethod]
        public void TestNoFiles()
        {
            // Arrange
            var mockController = new Mock<PDFConvertController>(new Mock<IConfigParams>().Object);

            mockController.Protected()
              .Setup<HttpContext>("GetHttpContext")
              .Returns(new HttpContext(
                new HttpRequest("", "http://www.tempuri.org", ""),
                new HttpResponse(null)));
            var managedException = false;

            // Act
            try
            {
                ConvertFileResults result = mockController.Object.Post();
            }
            catch (WrongNumberOfFilesException)
            {
                managedException = true;
            }

            // Assert
            Assert.IsTrue(managedException, "Should produce an exception when no files attached to request.");
        }
    }
}