namespace UnitTestHelperClasses
{
  using System;
  using System.Xml.Linq;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moq;
  using Moq.Protected;
  using PTASMediaHelperClasses;

  /// <summary>
  /// Tests the progress manager.
  /// </summary>
  [TestClass]
  public class ProgressManager_UnitTest
  {
    private const int GenericId = 123;
    private const string ExpectedRoute = "c:\\tmp\\/progress_tmp.xml";


    [TestMethod]
    public void Test_Constructor_OutputPathMissing()
    {
      var managedException = false;
      try
      {
        var l = new Mock<ILogger>();
        var x = new ProgressManager(string.Empty, l.Object);
      }
      catch (ArgumentException ex)
      {
        if (ex.ParamName == "outputPath")
        {
          managedException = true;
        }
        else
        {
          throw;
        }
      }

      Assert.IsTrue(managedException);
    }

    /// <summary>
    /// Test Calls.
    /// </summary>
    [TestMethod]
    public void Test_Calls()
    {
      // Arrange.
      int toSaveId = 0;
      string savedRoute = string.Empty;
      Mock<ILogger> mockLogger = new Mock<ILogger>();
      var testMock = new Mock<ProgressManager>(@"c:\tmp\", mockLogger.Object);

      XDocument returnDoc = new XDocument(
        new XComment($"Mock Document"),
        new XElement("root", new XElement("lastid", GenericId.ToString())));
      testMock.Protected().Setup<XDocument>("LoadDocument", ItExpr.IsAny<string>()).Returns(returnDoc);

      testMock.Protected().Setup<bool>("FileExists", ItExpr.IsAny<string>()).Returns(true);
      testMock.Protected().Setup("SaveDocument", ItExpr.IsAny<string>(), ItExpr.IsAny<XDocument>()).Callback<string, XDocument>((s, doc) =>
      {
        toSaveId = int.Parse(doc.Element("root").Element("lastid").Value);
        savedRoute = s;
      });

      // Act.
      int recordId = testMock.Object.GetLastProcessedRecord("tmp");
      testMock.Object.SaveLastRecordProcessed("tmp", GenericId);

      // Assert.
      Assert.AreEqual(GenericId, recordId);
      Assert.AreEqual(GenericId, toSaveId);
      Assert.AreEqual(ExpectedRoute, savedRoute);

    }

  }
}
