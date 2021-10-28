namespace DBLoaderClassesUnitTest
{
  using System;
  using System.Collections.Generic;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moq;
  using Moq.Protected;
  using PTASMediaDBLoaderClasses;
  using PTASMediaHelperClasses;

  /// <summary>
  /// Unit test class for the DBRunner.
  /// </summary>
  [TestClass]
  public class DBRunnerUnitTest
  {
    /// <summary>
    /// Mock root folder.
    /// </summary>
    private const string RootFolder = "TestRoot";

    /// <summary>
    /// Mock connections.
    /// </summary>
    private const string ConnectionString = "TestConnection";

    /// <summary>
    /// Test the run method.
    /// </summary>
    [TestMethod]
    public void Test_Run()
    {
      bool gotToMover = false;
      string receivedMessage = string.Empty;

      FileUploadTemplate fileUploadTemplate = new FileUploadTemplate
      {
        Filter = string.Empty,
        OutputPath = "c:/tmp",
        Path = "/out",
        Recursive = true,
      };

      var mockMover = new Mock<IFileCopier>();
      mockMover
        .Setup<bool>(m => m.CopyFiles(It.IsAny<string>(), It.IsAny<FileUploadTemplate>())).Callback<string, FileUploadTemplate>((s, f) => gotToMover = true)
        .Returns(true);

      var testDate = new DateTime(2001, 1, 1);
      var filesProvider = new Mock<IFilesToProcessProvider>();
      filesProvider.Setup<IEnumerable<FileUploadTemplate>>(x => x.GetFiles()).Returns(new FileUploadTemplate[]
        {
          fileUploadTemplate,
        });

      var mockLogger = new Mock<ILogger>();
      mockLogger.Setup(m => m.WriteInfo(It.IsAny<string>())).Callback<string>(s =>
      {
        receivedMessage = s;
      });

      var mockDBRunner = new Mock<DBRunner>(mockMover.Object, RootFolder, ConnectionString, mockLogger.Object);

      mockDBRunner.Protected().Setup<IFilesToProcessProvider>("CreateFilesToProcessProvider", ItExpr.IsAny<DateTime>(), ItExpr.IsAny<string>()).Returns(filesProvider.Object);

      var mockMonitor = new Mock<FilesMonitor>(filesProvider.Object, mockLogger.Object);
      mockMonitor.Protected().Setup<string[]>("GetFiles", ItExpr.IsAny<FileUploadTemplate>()).Returns<FileUploadTemplate>(fu => new string[] { fu.OutputPath });

      mockDBRunner.Protected().Setup<IFilesHandler>("CreateFilesHandler", ItExpr.IsAny<IFilesToProcessProvider>()).Returns(mockMonitor.Object);

      mockDBRunner.Object.Run(testDate, string.Empty);

      Assert.IsTrue(gotToMover);
      Assert.AreEqual("Moved file c:/tmp succesfully.", receivedMessage);
    }

    /// <summary>
    /// Tests for correctly handled exceptions.
    /// </summary>
    [TestMethod]
    public void TestExceptions()
    {
      var loggerMissingHandled = false;
      var mockMover = new Mock<IFileCopier>();

      // logger null
      try
      {
        var testInstance = new DBRunner(mockMover.Object, RootFolder, ConnectionString, null);
      }
      catch (ArgumentNullException ex)
      {
        if (ex.ParamName == "logger")
        {
          loggerMissingHandled = true;
        }
      }

      Assert.IsTrue(loggerMissingHandled);
    }

    /// <summary>
    /// Tests for correctly handled exceptions.
    /// </summary>
    [TestMethod]
    public void Test_Constructor_RootFolderMissing()
    {
      var rootFolderEmptyHandled = false;
      var mockMover = new Mock<IFileCopier>();
      var mockLogger = new Mock<ILogger>();

      try
      {
        var testInstance = new DBRunner(mockMover.Object, string.Empty, ConnectionString, mockLogger.Object);
      }
      catch (ArgumentException ex)
      {
        if (ex.ParamName == "rootFolder")
        {
          rootFolderEmptyHandled = true;
        }
      }

      Assert.IsTrue(rootFolderEmptyHandled);
    }

    /// <summary>
    /// Tests for correctly handled exceptions.
    /// </summary>
    [TestMethod]
    public void Test_Constructor_MoverMissing()
    {
      var moverMissingHandled = false;

      var mockLogger = new Mock<ILogger>();

      // mover null
      try
      {
        var testInstance = new DBRunner(null, RootFolder, ConnectionString, mockLogger.Object);
      }
      catch (ArgumentNullException ex)
      {
        if (ex.ParamName == "mover")
        {
          moverMissingHandled = true;
        }
      }

      Assert.IsTrue(moverMissingHandled);
    }

    /// <summary>
    /// Tests for correctly handled exceptions.
    /// </summary>
    [TestMethod]
    public void Test_Constructor_ConnectionStringMissing()
    {
      var connectionStringEmptyHandled = false;
      var mockMover = new Mock<IFileCopier>();
      var mockLogger = new Mock<ILogger>();

      try
      {
        var testInstance = new DBRunner(mockMover.Object, RootFolder, string.Empty, mockLogger.Object);
      }
      catch (ArgumentException ex)
      {
        if (ex.ParamName == "connectionString")
        {
          connectionStringEmptyHandled = true;
        }
      }

      Assert.IsTrue(connectionStringEmptyHandled);
    }
  }
}
