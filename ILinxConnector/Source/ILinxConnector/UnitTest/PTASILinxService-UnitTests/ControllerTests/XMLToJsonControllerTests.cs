using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Moq.Protected;

using PTASIlinxService.Classes.Exceptions;
using PTASIlinxService.Controllers;

using PTASLinxConnectorHelperClasses.Models;

using PTASServicesCommon.CloudStorage;

namespace PTASILinxService_UnitTests.ControllerTests
{
    [TestClass]
    public class XMLToJsonControllerTests
    {
        private const string goodXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Drawing></Drawing>";

        [TestMethod]
        public async Task TestGetAsync()
        {
            // arrange
            var cloudStrg = new Mock<ICloudStorageProvider>();
            var config = new Mock<IConfigParams>();
            var x = new Mock<SketchToJsonController>(cloudStrg.Object, config.Object);
            x.Protected()
             .Setup<Task<string>>("GetBlobText", ItExpr.IsAny<string>())
             .Returns(Task.FromResult(goodXML));
            x.Protected().Setup<Task>("SaveJSONAndSVG", ItExpr.IsAny<string>(), ItExpr.IsAny<string>()).Returns(Task.FromResult(0));

            var failed = false;
            // act
            try
            {
                System.Net.Http.HttpResponseMessage result = await x.Object.GetAsync("tmp.xml");
            }
            catch (Exception)
            {
                failed = true;
            }
            // assert
            Assert.IsFalse(failed, "Should succeed.");
        }


        [TestMethod]
        public async Task TestFailOnBadXMLAsync()
        {
            // arrange
            var cloudStrg = new Mock<ICloudStorageProvider>();
            var config = new Mock<IConfigParams>();
            var x = new Mock<SketchToJsonController>(cloudStrg.Object, config.Object);
            x.Protected()
             .Setup<Task<string>>("GetBlobText", ItExpr.IsAny<string>())
             .Returns(Task.FromResult("BAD"));
            var failed = false;
            // act
            try
            {
                _ = await x.Object.GetAsync("tmp.xml");
            }
            catch (XMLToJSonException)
            {
                failed = true;
            }
            // assert
            Assert.IsTrue(failed, "Should've failed on bad XML");
        }


        [TestMethod]
        public async Task TestGetAsyncFailOnNoRoute()
        {
            // arrange
            var cloudStrg = new Mock<ICloudStorageProvider>();
            var config = new Mock<IConfigParams>();
            var x = new SketchToJsonController(cloudStrg.Object, config.Object);
            var failed = false;
            // act
            try
            {
                _ = await x.GetAsync(null);
            }
            catch (ArgumentException ex) when (ex.ParamName == "route")
            {
                failed = true;
            }
            // assert
            Assert.IsTrue(failed, "Sould've failed on no route");
        }

        [TestMethod]
        public void TestConstructor()
        {
            // arrange
            var failedOnNoCloud = false;
            var failedOnNoStorage = false;
            var cloudStrg = new Mock<ICloudStorageProvider>();
            var config = new Mock<IConfigParams>();
            try
            {
                // act
                _ = new SketchToJsonController(null, config.Object);
            }
            catch (ArgumentException ex) when (ex.ParamName == "cloudProvider")
            {
                failedOnNoCloud = true;
            }

            try
            {
                // act 2
                _ = new SketchToJsonController(cloudStrg.Object, null);
            }
            catch (ArgumentException ex) when (ex.ParamName == "config")
            {
                failedOnNoStorage = true;
            }

            // assert
            Assert.IsTrue(failedOnNoCloud, "Had to fail on null cloud provider.");
            Assert.IsTrue(failedOnNoStorage, "Had to fail on null config provider.");
        }
    }
}
