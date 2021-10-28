namespace UnitTestHelperClasses
{
  using System;
  using System.Data;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moq;
  using Moq.Protected;
  using PTASMediaHelperClasses;

  /// <summary>
  /// Class for testing the table checker.
  /// </summary>
  [TestClass]
  public class TableChecker_UnitTest
  {
    private const string GenericGuid1 = "12345678-1234-1234-1234-123456789abc";

    private const string GenericGuid2 = "11111111-1111-1111-1111-111111111111";
    private const string GenericExtension = "JPG";
    private const int GenericConst = 666;
    private static readonly Guid FileGuid = new Guid(GenericGuid1);
    private static readonly Guid IndexGuid = new Guid(GenericGuid2);
    private static readonly int ItemId = GenericConst;
    private static readonly string FileExtension = GenericExtension;
    private readonly object[] values = new object[]
    {
      FileGuid,
      IndexGuid,
      ItemId,
      FileExtension,
    };

    /// <summary>
    /// Test the table checker run.
    /// </summary>
    [TestMethod]
    public void Test_Run()
    {
      // Arrange.
      var passed = false;
      var mockReader = new Mock<IDataReader>();
      mockReader.Setup(mr => mr.GetValue(It.IsAny<int>())).Returns((int colIndex) =>
      {
        return this.values[colIndex];
      });

      mockReader.Setup(mr => mr.Read()).Returns(() =>
      {
        if (!passed)
        {
          passed = true;
          return true;
        }

        return false;
      });

      var mockCommand = new Mock<IDbCommand>();
      mockCommand.Setup(m => m.ExecuteReader()).Returns(mockReader.Object);

      var mockConnection = new Mock<IDbConnection>();

      var mockChecker = new Mock<TableChecker>("accy", "ConnectionString");

      mockChecker.Protected().Setup<IDbCommand>("CreateCommand", ItExpr.IsAny<string>(), ItExpr.IsAny<IDbConnection>()).Returns(mockCommand.Object);
      mockChecker.Protected().Setup<IDbConnection>("CreateConnection").Returns(mockConnection.Object);

      Guid receivedFileGuid = Guid.Empty;
      string receivedExtension = string.Empty;

      // Act.
      var r = mockChecker.Object.Check(0, out int lasId, (
        fileGuid, extension) =>
      {
        receivedFileGuid = fileGuid;
        receivedExtension = extension;
      });

      // Assert.
      Assert.IsTrue(passed);
      Assert.AreEqual(receivedFileGuid.ToString(), GenericGuid1);
      Assert.AreEqual(receivedExtension, GenericExtension);
    }
  }
}
