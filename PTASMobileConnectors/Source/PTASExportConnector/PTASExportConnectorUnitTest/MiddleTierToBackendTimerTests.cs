using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASExportConnector;
using PTASExportConnector.SDK;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTASExportConnectorUnitTest
{
    [TestClass]
    public class MiddleTierToBackendTimerTests
    {
        [TestMethod]
        public void TestMiddleTierToBackendTimer_ShouldComplete()
        {
            var connector = new Mock<IConnector>();
            var dbService = new Mock<IDbService>();
            var logger = new Mock<ILogger>();
            var context = new Mock<IDurableOrchestrationContext>();
            var timerTrigger = new MiddleTierToBackendTimer(connector.Object, dbService.Object);
            var guidList = new List<Guid>()
            {
                Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3")
            };

            connector.Setup(c => c.GetDeviceGuidList()).Returns(guidList);
            context.Setup(c => c.CurrentUtcDateTime).Returns(DateTime.UtcNow);

            timerTrigger.RunAsync(It.IsAny<TimerInfo>(), logger.Object, context.Object);

            connector.Verify(c => c.GetDeviceGuidList(), Times.Once);
            connector.Verify(c => c.Init(), Times.Once);
            context.Verify(c => c.CallActivityAsync<Task<IActionResult>>("MiddleTierToBackendHTTP", Guid.Parse("c625d088-a81c-44f8-b24b-55b78743a4f3")), Times.Once);
            dbService.Verify(db => db.CleanExportedData(), Times.Once);
        }
    }
}
