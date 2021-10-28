namespace DBLoaderClassesUnitTest
{
  using System;
  using System.Data;
  using System.Linq;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moq;
  using Moq.Protected;
  using PTASMediaHelperClasses;

  /// <summary>
  /// Test class for DBLoaderUnitTest.
  /// </summary>
  [TestClass]
  public class DBLoaderUnitTest
  {
    /// <summary>
    /// Mock connection string.
    /// </summary>
    private const string ConnectionString = "ConnectionString";

    /// <summary>
    /// Mock root folder.
    /// </summary>
    private const string RootFolder = "root/";
    private const string MockGuid = "12345678-1234-1234-1234-123456789abc";
    private const string MockQuery = "Query";

    /// <summary>
    /// Test class exceptions.
    /// </summary>
    [TestMethod]
    public void Test_Constructor_QueryToRunNull()
    {
      // Arrange.
      DateTime now = It.IsAny<DateTime>();
      var queryEmptyHandled = false;
      var logger = new Mock<ILogger>();

      // Act.
      try
      {
        var loader = new PTASMediaDBLoaderClasses.DBLoader(ConnectionString, now, RootFolder, logger.Object, string.Empty);
      }
      catch (ArgumentException ex)
      {
        if (ex.ParamName == "queryToRun")
        {
          queryEmptyHandled = true;
        }
      }

      // Assert.
      Assert.IsTrue(queryEmptyHandled);
    }

    /// <summary>
    /// Test class exceptions.
    /// </summary>
    [TestMethod]
    public void Test_Constructor_ConnectionStringNull()
    {
      // Arrange.
      DateTime now = It.IsAny<DateTime>();
      var connectionStringEmptyHandled = false;
      var logger = new Mock<ILogger>();

      // Act.
      try
      {
        var loader = new PTASMediaDBLoaderClasses.DBLoader(string.Empty, now, RootFolder, logger.Object, string.Empty);
      }
      catch (ArgumentException ex)
      {
        if (ex.ParamName == "connectionString")
        {
          connectionStringEmptyHandled = true;
        }
      }

      // Assert.
      Assert.IsTrue(connectionStringEmptyHandled);
    }

    /// <summary>
    /// Test class exceptions.
    /// </summary>
    [TestMethod]
    public void Test_Constructor_RootFolderNull()
    {
      // Arrange.
      DateTime now = new DateTime(2001, 01, 01);
      var rootFolderEmptyHandled = false;
      var logger = new Mock<ILogger>();

      // Act.
      try
      {
        var loader = new PTASMediaDBLoaderClasses.DBLoader(ConnectionString, now, string.Empty, logger.Object, string.Empty);
      }
      catch (ArgumentException ex)
      {
        if (ex.ParamName == "rootFolder")
        {
          rootFolderEmptyHandled = true;
        }
      }

      // Assert.
      Assert.IsTrue(rootFolderEmptyHandled);
    }

    /// <summary>
    /// Tests file getter.
    /// </summary>
    [TestMethod]
    public void Test_GetFiles()
    {
      // Arrange.
      DateTime now = new DateTime(2001, 01, 01);
      var logger = new Mock<ILogger>();
      ILogger loggerInstance = logger.Object;
      var loader = new Mock<PTASMediaDBLoaderClasses.DBLoader>(ConnectionString, now, RootFolder, loggerInstance, MockQuery);

      loader.Protected().Setup<IDbConnection>("CreateSQLConnection", ItExpr.IsAny<string>()).Returns(new Mock<IDbConnection>().Object);

      loader.Protected().Setup("FillDataTable", ItExpr.IsAny<IDataAdapter>(), ItExpr.IsAny<DataTable>()).Callback((IDataAdapter adapter, DataTable table) =>
      {
        table.Columns.Add("name", typeof(string));
        table.Columns.Add("extension", typeof(string));
        table.Rows.Add(new string[] { MockGuid, "JPG" });
      });

      // Act.
      var result = loader.Object.GetFiles();

      // Assert.
      Assert.AreEqual(result.Count(), 1);
      Assert.AreEqual(result.First().OutputPath, "1/2/3/4/");
      Assert.AreEqual(result.First().Path, @"root\1\2\3\4\");
    }
  }
}
