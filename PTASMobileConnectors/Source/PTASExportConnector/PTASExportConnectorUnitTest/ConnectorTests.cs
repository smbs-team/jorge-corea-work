using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTASExportConnector.Exceptions;
using PTASExportConnector.SDK;
using System;
using System.Collections.Generic;

namespace PTASExportConnectorUnitTest
{
    [TestClass]
    public class ConnectorTests
    {
        private readonly Connector connector;

        public ConnectorTests()
        {
            connector = new Connector();
        }

        [TestMethod]
        [DataRow("", DisplayName = "Connection String")]
        [DataRow(null, DisplayName = "Connection String")]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestInit_ShouldThrowIfConnectionStringIsNullOrEmpty(string param)
        {
            Environment.SetEnvironmentVariable("SqlConnectionString", param);
            connector.Init();
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestGetModifiedEntityData_ShouldThrowIfRootIdIsNullOrEmpty(string param)
        {
            connector.GetModifiedEntityData(param, "entityKind", Guid.NewGuid());
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestGetModifiedEntityData_ShouldThrowIfEntityKindIsNullOrEmpty(string param)
        {
            connector.GetModifiedEntityData("rootId", param, Guid.NewGuid());
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestGetModifiedEntityData_ShouldThrowIfDeviceGuidIsNull()
        {
            connector.GetModifiedEntityData("rootId", "entityKind", Guid.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestSetSetChangesetsExported_ShouldThrowIfListIsEmpty()
        {
            var emptyList = new List<string>();
            connector.SetChangesetsExported(emptyList);
        }

    }
}
