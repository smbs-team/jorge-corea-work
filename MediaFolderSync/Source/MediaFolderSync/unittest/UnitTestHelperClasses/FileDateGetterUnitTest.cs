namespace UnitTestHelperClasses
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using PTASMediaHelperClasses;

  /// <summary>
  /// Unit testing for the file date getter.
  /// </summary>
  [TestClass]
  public class FileDateGetterUnitTest
  {
    /// <summary>
    /// Test for exceptions in the object.
    /// </summary>
    [TestMethod]
    public void TestExceptions()
    {
      var handledDocfileEmpty = false;
      try
      {
        var tempFileGetter = new FileDateManager(string.Empty);
      }
      catch (ArgumentException ex)
      {
        if (ex.ParamName == "docFile")
        {
          handledDocfileEmpty = true;
        }
      }

      Assert.IsTrue(handledDocfileEmpty);
    }
  }
}
