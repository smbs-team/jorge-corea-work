using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTASExportConnector.Exceptions;
using System;

namespace PTASExportConnectorUnitTest
{
    [TestClass]
    public class TestHelper
    {
        [TestMethod]
        public void TestSqlExceptionBuilder_ShouldReturnText()
        {

            var sqlException = new SqlExceptionBuilder().WithErrorNumber(50000)
                                        .WithErrorMessage("Something went wrong.")
                                        .Build();

            var text = Helper.SqlExceptionBuilder(sqlException);

            StringAssert.Contains(text, "Something went wrong.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSqlExceptionBuilder_ShouldThrowIfExceptionIsNull()
        {
            Helper.SqlExceptionBuilder(null);
        }
    }
}
