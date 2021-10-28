using ConnectorService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTASImportConnector.SDK;
using System;
using System.Xml.Linq;

namespace PTASImportConnectorUnitTest
{
    [TestClass]
    public class BackendServiceTests
    {
        private readonly BackendService service;
        private readonly EntityModel model;
        private readonly XElement xElement;

        public BackendServiceTests()
        {
            service = new BackendService();
            xElement = new XElement("fire", "Content of fire.");
            model = new EntityModel(xElement);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetData_ShouldThrowWhenEntityIsNull()
        {
            Connector connector = new Connector();
            connector.Init("string", null);
            service.GetData(null, "string", "string" ,DateTime.UtcNow, connector, 100, false, null, null);
        }

        [TestMethod]
        [DataRow("", DisplayName = "Connection String")]
        [DataRow(null, DisplayName = "Connection String")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetData_ShouldThrowWhenConnectionStringIsNullOrEmpty(string param)
        {
            Connector connector = new Connector();
            connector.Init("string", null);
            service.GetData(model, param, param, DateTime.UtcNow, connector, 100, false, null, null);
        }
    }
}
