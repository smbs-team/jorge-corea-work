using ConnectorService;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASImportConnector;
using PTASImportConnector.Exceptions;
using PTASImportConnector.SDK;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PTASImportConnectorUnitTest
{
    [TestClass]
    public class ImportToMiddleTierTests
    {

        private readonly Mock<Connector> connectorSDK;
        private readonly Mock<DbModel> databaseModel;
        private readonly Mock<BackendService> backendService;
        private readonly Mock<ILogger> logger;
        private readonly XElement xElement;
        private readonly EntityModel entity;
        private readonly DatabaseModel model;
        private readonly ImportToMiddleTier import;
        private readonly List<Dictionary<string, object>> uploadList;

        public ImportToMiddleTierTests()
        {
            connectorSDK = new Mock<Connector>();
            databaseModel = new Mock<DbModel>();
            backendService = new Mock<BackendService>();
            logger = new Mock<ILogger>();
            xElement = new XElement("fire", "Content of fire.");
            entity = new EntityModel(xElement)
            {
                SyncOrder = 2,
                BackendQuery = "SELECT",
                Name = "Ptas_parceldetail"
            };
            model = new DatabaseModel(xElement)
            {
                Name = "DbName",
                DatabaseVersion = "1"
            };
            model.Entities.Add(entity);
            import = new ImportToMiddleTier(connectorSDK.Object, databaseModel.Object, backendService.Object, logger.Object);
            uploadList = new List<Dictionary<string, object>> { new Dictionary<string, object>() { { "something", 1 } } };

        }

        //[TestMethod]
        public void TestRun_ShouldComplete()
        {

            Environment.SetEnvironmentVariable("FilterEntityName", "");

            databaseModel.Setup(db => db.GetDatabaseModel()).Returns(model);
            connectorSDK.Setup(c => c.GetLastImportDate()).Returns(DateTime.Today);
            connectorSDK.Setup(c => c.GetLastImportEntityDate("Ptas_parceldetail")).Returns(DateTime.Today);
            connectorSDK.Setup(c => c.GetUploadTicketForBackend()).Returns(1000);
            //backendService.Setup(b => b.GetData(It.IsAny<EntityModel>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(uploadList);

            import.Run(null, null);

            connectorSDK.Verify(c => c.Init(It.IsAny<string>(), It.IsAny<ClientCredential>()), Times.Once); ;
            databaseModel.Verify(db => db.GetDatabaseModel(), Times.Once);
            connectorSDK.Verify(c => c.GetLastImportDate(), Times.Once);
            connectorSDK.Verify(c => c.GetLastImportEntityDate("Ptas_parceldetail"), Times.Once);
            connectorSDK.Verify(c => c.GetUploadTicketForBackend(), Times.Once);
            //backendService.Verify(b => b.GetData(It.IsAny<EntityModel>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
            connectorSDK.Verify(c => c.AddUploadData(It.IsAny<string>(), It.IsAny<List<Dictionary<string, object>>>(), It.IsAny<long>()), Times.Once);
            connectorSDK.Verify(c => c.ProcessDataForTicket(It.IsAny<long>(), false, true), Times.Once);
            connectorSDK.Verify(c => c.SetImportDate(It.IsAny<DateTime>()), Times.Once);
            connectorSDK.Verify(c => c.SetImportEntityDate("Ptas_parceldetail", It.IsAny<DateTime>()), Times.Once);
        }

        /*[TestMethod]
        [ExpectedException(typeof(ImportConnectorException))]
        public void TestRun_ShouldThrowIfSomethingFails()
        {
            ClientCredential cc = new ClientCredential("string","string");
            connectorSDK.Setup(c => c.Init("sql", cc)).Throws(new DivideByZeroException());
            import.Run(null, null);
        }*/
    }
}
