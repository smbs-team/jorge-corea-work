using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTASTiffs;

namespace PTASTiffs_UnitTests
{
  [TestClass]
  public class TestTiffToImages
  {
    private const string InputTiff = "inputTiff";

    [TestMethod]
    public void TestFailWhenImageIsNull()
    {
      // Arrange
      var failedOnNullImage = false;

      // Act
      try
      {
        TiffManager.TiffToImages(null);
      }
      catch (ArgumentException ex)
      {
        failedOnNullImage = ex.ParamName.Equals(InputTiff);
      }

      // Assert
      Assert.IsTrue(failedOnNullImage);
    }

    [TestMethod]
    public void SucceedsWithRightInfo()
    {
      // Arrange
      string message;
      byte[][] result = null;
      var failed = false;

      using (var bmp = new Bitmap(1, 1))
      {
        using (var mem = new MemoryStream())
        {
          bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Tiff);
          try
          {
            // Act
            result = TiffManager.TiffToImages(mem);
          }
          catch (Exception ex)
          {
            failed = true;
            message = ex.Message;
          }
        }
      }

      // Assert
      Assert.IsNotNull(result, "Result should not be null");
      Assert.IsTrue(result.Length == 1, "Should only return one image.");
      Assert.IsFalse(failed, "Should not have failed with a correctly built image.");
    }

    [TestMethod]
    public void FailsWhenImageDataIsWrong()
    {
      var failedOnIncorrectImageData = false;
      var m = new MemoryStream(new byte[] { });

      // Act
      try
      {
        var image = TiffManager.TiffToImages(m);
      }
      catch (ArgumentException ex)
      {
        failedOnIncorrectImageData = ex.ParamName.Equals(InputTiff);
      }

      // Assert
      Assert.IsTrue(failedOnIncorrectImageData, "Had to fail on incorrect image data.");
    }

  }
}
