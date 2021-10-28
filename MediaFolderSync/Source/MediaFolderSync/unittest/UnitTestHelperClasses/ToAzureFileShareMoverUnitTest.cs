namespace UnitTestHelperClasses
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moq;
  using PTASMediaHelperClasses;

  /// <summary>
  /// Test the azure mover.
  /// </summary>
  [TestClass]
  public class ToAzureFileShareMoverUnitTest
  {
    /// <summary>
    /// Mock connection string.
    /// </summary>
    private const string StorageConnection = "ConnectionString";


    /// <summary>
    /// Test constructor exception when storage is empty.
    /// </summary>
    [TestMethod]
    public void Test_StorageEmptyHandled()
    {
      var storageConnectionEmptyHandled = false;
      Mock<ILogger> mockLogger = new Mock<ILogger>();
      try
      {
        var m = new ToAzureFileShareMover(string.Empty, mockLogger.Object);
      }
      catch (ArgumentException ex)
      {
        if (ex.ParamName == "storageConnection")
        {
          storageConnectionEmptyHandled = true;
        }
      }
      Assert.IsTrue(storageConnectionEmptyHandled);

    }


    /// <summary>
    /// Test constructor exception when logger is empty.
    /// </summary>
    [TestMethod]
    public void Test_LoggerEmptyHandled()
    {
      var loggerEmptyHandled = false;
      try
      {
        var m = new ToAzureFileShareMover(StorageConnection, null);
      }
      catch (ArgumentNullException ex)
      {
        if (ex.ParamName == "logger")
        {
          loggerEmptyHandled = true;
        }
      }

      Assert.IsTrue(loggerEmptyHandled);
    }


  }
}
