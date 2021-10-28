using ConnectorService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using PTASExportConnector;
using PTASExportConnector.Exceptions;
using PTASExportConnector.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PTASExportConnectorUnitTest
{
    [TestClass]
    public class ExportersTests
    {
        private readonly Mock<IDbService> dbService;
        private readonly Mock<IOdata> oData;
        private readonly Mock<IFileSystem> fileSystem;
        private readonly Mock<IConnector> connector;
        private Exporters exporters;
        private readonly XElement xElement;
        private readonly EntityModel entity;
        private readonly DatabaseModel model;
        private readonly EntityAttributeModel attribute;
        private Dictionary<string, object> entityAttributes;
        private List<Dictionary<string, object>> attributesList;

        public ExportersTests()
        {
            this.dbService = new Mock<IDbService>();
            this.oData = new Mock<IOdata>();
            this.fileSystem = new Mock<IFileSystem>();
            this.connector = new Mock<IConnector>();
            this.exporters = new Exporters(this.dbService.Object, this.fileSystem.Object, this.oData.Object);

            xElement = new XElement("attribute", "");
            this.attribute = new EntityAttributeModel(xElement);

            // Attributes for the entity
            attribute.IsClientKey = true;
            attribute.Name = "ptas_name";
            attribute.AttributeType = "String";

            xElement = new XElement("entity", "");
            entity = new EntityModel(xElement)
            {
                SyncOrder = 2,
                BackendQuery = "SELECT",
                Name = "Parcel"
            };
            entity.Attributes.Add(attribute);

            this.attribute = new EntityAttributeModel(xElement);
            attribute.IsClientKey = false;
            attribute.Name = "rowStatus_mb";
            attribute.AttributeType = "String";

            entity.Attributes.Add(attribute);

            this.attribute = new EntityAttributeModel(xElement);
            attribute.IsClientKey = false;
            attribute.Name = "changesetId_mb";
            attribute.AttributeType = "Integer 32";

            entity.Attributes.Add(attribute);

            this.attribute = new EntityAttributeModel(xElement);
            attribute.IsClientKey = false;
            attribute.Name = "guid_mb";
            attribute.AttributeType = "GUID";

            entity.Attributes.Add(attribute);

            model = new DatabaseModel(xElement)
            {
                Name = "DbName",
                DatabaseVersion = "1"
            };
            model.Entities.Add(entity);

            this.entityAttributes = new Dictionary<string, object>()
            {
                {"ptas_name", "Joe"},
                {"rowStatus_mb", "I"},
                {"guid_mb",  Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3")},
                {"changesetId_mb",  10}
            };

            this.attributesList = new List<Dictionary<string, object>>();

            Environment.SetEnvironmentVariable("webapiurl", "/api/data/v9.0/");
        }

        [TestMethod]
        public void TestExport_createShouldComplete()
        {
            attributesList.Add(entityAttributes);

            dbService.Setup(d => d.GetDatabaseModel()).Returns(model);
            connector.Setup(c => c.GetModifiedEntityData(null, "Parcel", Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"))).Returns(attributesList);
            oData.Setup(o => o.Create(@"/api/data/v9.0/" + "ptas_parceldetails", It.IsAny<object>(), "")).Returns(Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(@"HTTP/1.1 204 No Content OData-Version: 4.0 OData-EntityId: [Organization URI]/api/data/v9.0/accounts(7eb682f1-ca75-e511-80d4-00155d2a68d1)") }));
          
            exporters.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "Parcel");

            dbService.Verify(d => d.GetDatabaseModel(), Times.Once);
            connector.Verify(c => c.GetModifiedEntityData(null, "Parcel", Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3")), Times.Once);
            oData.Verify(o => o.Create(@"/api/data/v9.0/" + "ptas_parceldetails", It.IsAny<object>(), ""), Times.Once);
            dbService.Verify(d => d.UpdateEntityKeys(Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "Parcel", "ptas_name", Guid.Parse("7eb682f1-ca75-e511-80d4-00155d2a68d1")), Times.Once);

        }

        [TestMethod]
        public void TestExport_updateShouldComplete()
        {
            entityAttributes["rowStatus_mb"] = "U";
            attributesList.Add(entityAttributes);
            var dic = new Dictionary<string, Guid>()
            {
                { "ptas_name", Guid.Parse("5c382132-aa19-4e81-abcc-3909e6f4e33c") }
            };

            dbService.Setup(d => d.GetDatabaseModel()).Returns(model);
            connector.Setup(c => c.GetModifiedEntityData(null, "Parcel", Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"))).Returns(attributesList);
            dbService.Setup(d => d.GetEntityKeys(It.IsAny<Guid>(), It.IsAny<string>())).Returns(dic);

            exporters.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "Parcel");

            dbService.Verify(d => d.GetDatabaseModel(), Times.Once);
            connector.Verify(c => c.GetModifiedEntityData(null, "Parcel", Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3")), Times.Once);
            oData.Verify(o => o.Update(@"/api/data/v9.0/" + "ptas_parceldetails", It.IsAny<object>(), "5c382132-aa19-4e81-abcc-3909e6f4e33c"), Times.Once);
            
        }

        [TestMethod]
        public void TestExport_deleteShouldComplete()
        {
            entityAttributes["rowStatus_mb"] = "D";
            attributesList.Add(entityAttributes);
            var dic = new Dictionary<string, Guid>()
            {
                { "ptas_name", Guid.Parse("5c382132-aa19-4e81-abcc-3909e6f4e33c") }
            };

            dbService.Setup(d => d.GetDatabaseModel()).Returns(model);
            connector.Setup(c => c.GetModifiedEntityData(null, "Parcel", Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"))).Returns(attributesList);
            dbService.Setup(d => d.GetEntityKeys(It.IsAny<Guid>(), It.IsAny<string>())).Returns(dic);

            exporters.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "Parcel");

            dbService.Verify(d => d.GetDatabaseModel(), Times.Once);
            connector.Verify(c => c.GetModifiedEntityData(null, "Parcel", Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3")), Times.Once);
            oData.Verify(o => o.Delete(@"/api/data/v9.0/" + "ptas_parceldetails" + "(5c382132-aa19-4e81-abcc-3909e6f4e33c)"), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestExportConstructor_shouldThrowWhenDataBaseServiceIsNull()
        {
            this.exporters = new Exporters(null, this.fileSystem.Object, this.oData.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestExportConstructor_shouldThrowWhenFileSystemIsNull()
        {
            this.exporters = new Exporters(dbService.Object, null, this.oData.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestExportConstructor_shouldThrowWhenOdataIsNull()
        {
            this.exporters = new Exporters(dbService.Object, fileSystem.Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestExport_shouldThrowWhenConnectorIsNull()
        {
            exporters.Export(null, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "Parcel");
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestExport_shouldThrowWhenGuidIsEmpty()
        {
            exporters.Export(connector.Object, Guid.Empty, "Parcel");
        }

        [TestMethod]
        [DataRow("", DisplayName = "Entity Name")]
        [DataRow(null, DisplayName = "Entity Name")]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestExport_shouldThrowWhenEntityNameIsNullOrEmpty(string entityName)
        {
            exporters.Export(connector.Object, Guid.Empty, entityName);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestExport_shouldThrowWhenWebApiUrlIsNullOrEmpty(string param)
        {
            Environment.SetEnvironmentVariable("webapiurl", param);
            exporters.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "Parcel");
        }

        [TestMethod]
        public void TestGetEntityList_shouldReturnList()
        {
            var json = "{\"Entities\":[{\"Name\":\"Apartment_Region\",\"SyncOrder\":0}]}";

            fileSystem.Setup(f => f.File.ReadAllText(Path.Combine("route", @"\EntityList.json"))).Returns(json);

            var list = exporters.GetEntityList("route");

            Assert.AreEqual("Apartment_Region", list[0].Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestGetEntityList_shouldThrowIfPathIsEmpty()
        {
            exporters.GetEntityList("");
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestGetEntityList_shouldThrowIfDirectoryNotFound()
        {
            fileSystem.Setup(f => f.File.ReadAllText(Path.Combine("route", @"\EntityList.json"))).Throws(new DirectoryNotFoundException());
            exporters.GetEntityList("route");
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestGetEntityList_shouldThrowIfFileNotFound()
        {
            fileSystem.Setup(f => f.File.ReadAllText(Path.Combine("route", @"\EntityList.json"))).Throws(new FileNotFoundException());
            exporters.GetEntityList("route");
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestGetEntityList_shouldThrowIfPathIsInvalid()
        {
            fileSystem.Setup(f => f.File.ReadAllText(Path.Combine("route", @"\EntityList.json"))).Throws(new NotSupportedException());
            exporters.GetEntityList("route");
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestGetEntityList_shouldThrowIfCallerHasNoPermission()
        {
            fileSystem.Setup(f => f.File.ReadAllText(Path.Combine("route", @"\EntityList.json"))).Throws(new SecurityException());
            exporters.GetEntityList("route");
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestGetEntityList_shouldThrowIfSomethingGoesWrong()
        { 
            exporters.GetEntityList("route");
        }
    }
}
