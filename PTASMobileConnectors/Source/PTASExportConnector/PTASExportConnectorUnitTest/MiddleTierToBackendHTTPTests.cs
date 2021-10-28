using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASExportConnector;
using PTASExportConnector.Exceptions;
using PTASExportConnector.SDK;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity = PTASExportConnector.Entity;

namespace PTASExportConnectorUnitTest
{
    [TestClass]
    public class MiddleTierToBackendHTTPTests
    {
        [TestMethod]
        public void TestMiddleTierToBackendHTTP_ShouldComplete()
        {
            var connector = new Mock<IConnector>();
            var exporter = new Mock<IExporters>();
            var httpTrigger = new MiddleTierToBackendHTTP(exporter.Object, connector.Object);
            var logger = new Mock<ILogger>();
            var entity = new Entity()
            {
                Name = "ptas_test",
                SyncOrder = 0

            };
            var changesetIds = new List<string>()
            {
                "1"
            };


            IList<Entity> entityList = new List<Entity>();
            entityList.Add(entity);

            exporter.Setup(e => e.GetEntityList(It.IsAny<string>())).Returns(entityList);
            exporter.Setup(e => e.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "ptas_test")).Returns(changesetIds);
            

            var result = httpTrigger.Run(It.IsAny<HttpRequest>(), logger.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), null);

            connector.Verify(c => c.SetChangesetsExported(changesetIds), Times.Once);
            exporter.Verify(e => e.GetEntityList(It.IsAny<string>()), Times.Once);
            exporter.Verify(e => e.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "ptas_test"), Times.Once);

            Assert.IsTrue(result.IsCompletedSuccessfully);
        }

        [TestMethod]
        public void TestMiddleTierToBackendHTTP_ShouldReturnError()
        {
            var connector = new Mock<IConnector>();
            var exporter = new Mock<IExporters>();
            var httpTrigger = new MiddleTierToBackendHTTP(exporter.Object, connector.Object);
            var logger = new Mock<ILogger>();
            var entity = new Entity()
            {
                Name = "ptas_test",
                SyncOrder = 0

            };
            var changesetIds = new List<string>()
            {
                "1"
            };


            IList<Entity> entityList = new List<Entity>();
            entityList.Add(entity);

            exporter.Setup(e => e.GetEntityList(It.IsAny<string>())).Returns(entityList);
            exporter.Setup(e => e.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "ptas_test")).Throws(new Exception());


            var result = httpTrigger.Run(It.IsAny<HttpRequest>(), logger.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), null);

            connector.Verify(c => c.SetChangesetsExported(changesetIds), Times.Never);
            exporter.Verify(e => e.GetEntityList(It.IsAny<string>()), Times.Once);
            exporter.Verify(e => e.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "ptas_test"), Times.Once);

            Assert.ThrowsException<Exception>(() => exporter.Object.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "ptas_test"));
        }

        [TestMethod]
        public void TestMiddleTierToBackendHTTP_ShouldCatchExportConnectorException()
        {
            var connector = new Mock<IConnector>();
            var exporter = new Mock<IExporters>();
            var httpTrigger = new MiddleTierToBackendHTTP(exporter.Object, connector.Object);
            var logger = new Mock<ILogger>();
            var entity = new Entity()
            {
                Name = "ptas_test",
                SyncOrder = 0

            };
            var changesetIds = new List<string>()
            {
                "1"
            };


            IList<Entity> entityList = new List<Entity>();
            entityList.Add(entity);

            exporter.Setup(e => e.GetEntityList(It.IsAny<string>())).Returns(entityList);
            exporter.Setup(e => e.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "ptas_test")).Throws(new ExportConnectorException("Error", 500));


            var result = httpTrigger.Run(It.IsAny<HttpRequest>(), logger.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), null);

            connector.Verify(c => c.SetChangesetsExported(changesetIds), Times.Never);
            exporter.Verify(e => e.GetEntityList(It.IsAny<string>()), Times.Once);
            exporter.Verify(e => e.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "ptas_test"), Times.Once);

            Assert.ThrowsException<ExportConnectorException>(() => exporter.Object.Export(connector.Object, Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3"), "ptas_test"));
        }
    }
}
