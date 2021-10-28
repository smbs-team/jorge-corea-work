using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.IO;
using PTASILinxConnectorHelperClasses.Models;
using PTASIlinxService.Classes.Exceptions;
using PTASIlinxService.Classes;
using PTASIlinxService.Controllers;

namespace PTASILinxService_UnitTests.ControllerTests
{
    [TestClass]
    public class TIFFConvertControllerTest
    {
        [TestMethod]
        public void TestConvertTiff()
        {
            // Arrange
            var img = File.ReadAllBytes("./resources/input.tiff");

            var mockFile = new Mock<HttpPostedFileBase>();
            var mockCollection = new Mock<HttpFileCollectionBase>();

            mockFile.Setup(p => p.FileName).Returns("file.tiff");
            mockFile.Setup(p => p.ContentLength).Returns(img.Length);
            mockFile.Setup(p => p.ContentType).Returns("image/tiff");
            mockFile.Setup(p => p.InputStream).Returns(new MemoryStream(img));

            var array = new[] { mockFile.Object };
            mockCollection.Setup(x => x.GetEnumerator()).Returns(array.Select(f => f.FileName).GetEnumerator());
            mockCollection.Setup(x => x.Count).Returns(array.Count());
            mockCollection.Setup(x => x[It.IsAny<int>()]).Returns((int i) => array[i]);

            ICacheManager<ConvertFileResults> cacheManager = new MoqCacheMananger<ConvertFileResults>();

            var mockController = new Mock<TIFFConvertController>(cacheManager);
            mockController.Protected().Setup<HttpFileCollectionBase>("GetFiles").Returns(mockCollection.Object);
            mockController.Protected().Setup<Uri>("GetUri").Returns(new Uri("http://test.com/"));

            mockController.Protected()
              .Setup<HttpContext>("GetHttpContext")
              .Returns(new HttpContext(
                new HttpRequest("what.tiff", "https://www.tempuri.org/", ""),
                new HttpResponse(null)));

            // Act
            ConvertFileResults result = mockController.Object.Post();

            // Assert
            Assert.IsNotNull(result, "Result of image conversion cannot be null");
            Assert.AreEqual(result.Images.Count(), 5, 0, "Input file containes 5 images.");
            foreach (var image in result.Images)
            {
                Assert.IsTrue(image.Length > 0);
            }
        }

        /// <summary>
        /// Must return a file from cache if it is already there.
        /// </summary>

        [TestMethod]
        public void TestGetFromCache()
        {
            // Arrange
            var originalFile = new ConvertFileResults
            {
                Images = new byte[][] { },
            };

            var mockFile = new Mock<HttpPostedFileBase>();

            mockFile.Setup(p => p.FileName).Returns("file.tiff");
            mockFile.Setup(p => p.ContentLength).Returns(10);
            mockFile.Setup(p => p.ContentType).Returns("image/tiff");

            HttpPostedFileBase fileBaset = mockFile.Object;

            var array = new[] { fileBaset };
            var mockCollection = new Mock<HttpFileCollectionBase>();
            mockCollection.Setup(x => x.GetEnumerator()).Returns(array.Select(f => f.FileName).GetEnumerator());
            mockCollection.Setup(x => x.Count).Returns(array.Count());
            mockCollection.Setup(x => x[It.IsAny<int>()]).Returns((int i) => array[i]);

            var hash = StaticHelpers.GetHash(mockFile.Object);

            var cache = new MoqCacheMananger<ConvertFileResults>();
            cache.Get(hash, 0, () => originalFile);

            var mockController = new Mock<TIFFConvertController>(cache);
            mockController.Protected().Setup<HttpFileCollectionBase>("GetFiles").Returns(mockCollection.Object);

            mockController.Protected()
              .Setup<HttpContext>("GetHttpContext")
              .Returns(new HttpContext(
                new HttpRequest("", "https://www.tempuri.org", ""),
                new HttpResponse(null)));

            // Act
            ConvertFileResults returnedFile = mockController.Object.Post();

            // Assert
            Assert.AreEqual(returnedFile, originalFile, "Tiff file converter must return cached file");
        }

        [TestMethod]
        public void TestPostNoFiles()
        {
            // Arrange
            var mockController = new Mock<TIFFConvertController>(new CacheManager<ConvertFileResults>());

            mockController.Protected()
              .Setup<HttpContext>("GetHttpContext")
              .Returns(new HttpContext(
                new HttpRequest("", "http://www.tempuri.org", ""),
                new HttpResponse(null)));
            var managedException = false;
            try
            {
                // Act
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