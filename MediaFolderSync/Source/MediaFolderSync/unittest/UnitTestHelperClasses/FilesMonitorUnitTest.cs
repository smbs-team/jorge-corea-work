namespace UnitTestHelperClasses
{
  using System;
  using System.Collections.Generic;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moq;
  using Moq.Protected;
  using PTASMediaHelperClasses;

  /// <summary>
  /// Unit test for Files Monitor.
  /// </summary>
  [TestClass]
  public class FilesMonitorUnitTest
  {
    private const string FileName = "file.ext";
    private const string OutputPath = "out/";
    private const string SourcePath = "path/";

    /// <summary>
    /// Tests the run of the object.
    /// </summary>
    [TestMethod]
    public void Test_Run()
    {
      Mock<IFilesToProcessProvider> mockProvider = CreateMock();
      FileUploadTemplate testTemplate = new FileUploadTemplate
      {
        Filter = FileName,
        OutputPath = OutputPath,
        Path = SourcePath,
        Recursive = false,
      };
      FileUploadTemplate receivedTemplate = null;
      string receivedFileName = string.Empty;

      mockProvider.Setup(m => m.GetFiles()).Returns(new FileUploadTemplate[]
      {
        testTemplate,
      });

      Mock<ILogger> mockLogger = new Mock<ILogger>();
      var mockItem = new Mock<FilesMonitor>(mockProvider.Object, mockLogger.Object);
      mockItem.Protected()
        .Setup<string[]>("GetFiles", ItExpr.IsAny<FileUploadTemplate>())
        .Returns(new string[] { FileName });
      mockItem.Object.Run((inputFile, fileTemplate) =>
      {
        receivedTemplate = fileTemplate;
        receivedFileName = inputFile;
      });
      Assert.AreSame(receivedTemplate, testTemplate);
      Assert.AreEqual(receivedFileName, FileName);
    }

    /// <summary>
    /// Tests exceptions on the class.
    /// </summary>
    [TestMethod]
    public void Test_ProviderEmptyHandled()
    {
      var providerEmptyHandled = false;
      try
      {
        var temp = new FilesMonitor(null, new Mock<ILogger>().Object);
      }
      catch (ArgumentNullException ex)
      {
        if (ex.ParamName == "provider")
        {
          providerEmptyHandled = true;
        }
      }

      Assert.IsTrue(providerEmptyHandled);
    }

    private static Mock<IFilesToProcessProvider> CreateMock()
    {
      return new Mock<IFilesToProcessProvider>(MockBehavior.Strict);
    }
  }
}
