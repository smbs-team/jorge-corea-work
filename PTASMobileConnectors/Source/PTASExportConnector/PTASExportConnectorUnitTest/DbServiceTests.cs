using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTASExportConnector.Exceptions;
using PTASExportConnector.SDK;
using System;

namespace PTASExportConnectorUnitTest
{
    [TestClass]
    public class DbServiceTests
    {
        private readonly DbService dbService;
        public DbServiceTests()
        {
            Environment.SetEnvironmentVariable("SqlConnectionString", "conn password");
            dbService = new DbService();    
        }

        [TestMethod]
        [DataRow("", DisplayName = "Connection String")]
        [DataRow(null, DisplayName = "Connection String")]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestConstructor_ShouldThrowWhenConnectionStringIsNullOrEmpty(string param)
        {
            Environment.SetEnvironmentVariable("SqlConnectionString", param);
            var dbService = new DbService();
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestCleanGetEntityKeys_ShouldThrowWhenEntityGuidIsNull()
        {
            dbService.GetEntityKeys(Guid.Empty, "entityKind");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestCleanGetEntityKeys_ShouldThrowWhenEntityKindIsNullOrEmpty(string param)
        {
            dbService.GetEntityKeys(Guid.NewGuid(), param);
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestUpdateEntityKeys_ShouldThrowWhenEntityGuidIsNull()
        {
            dbService.UpdateEntityKeys(Guid.Empty, "entityKind", "fieldName", Guid.NewGuid());
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestUpdateEntityKeys_ShouldThrowWhenEntityKindIsNullOrEmpty(string param)
        {
            dbService.UpdateEntityKeys(Guid.NewGuid(), param, "fieldName", Guid.NewGuid());
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestUpdateEntityKeys_ShouldThrowWhenFieldNameIsNullOrEmpty(string param)
        {
            dbService.UpdateEntityKeys(Guid.NewGuid(), "entityKind", param, Guid.NewGuid());
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestUpdateEntityKeys_ShouldThrowWhenFieldValueIsNull()
        {
            dbService.UpdateEntityKeys(Guid.NewGuid(), "entityKind", "fieldName", Guid.Empty);
        }

    }
}
