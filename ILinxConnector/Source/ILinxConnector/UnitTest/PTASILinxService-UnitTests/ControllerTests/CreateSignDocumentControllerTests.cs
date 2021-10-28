
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Moq.Protected;

using PTASIlinxService.Classes;
using PTASIlinxService.Controllers;

using PTASLinxConnectorHelperClasses.Models;

using PTASServicesCommon.CloudStorage;

namespace PTASILinxService_UnitTests.ControllerTests
{
    [TestClass]
    public class CreateSignDocumentControllerTests
    {
        private const string test = "test";
        private const string nonExistingVar = "%nonExistingVar%";
        private const string existingVar = "%existingVar%";
        private const string value = "value";
        private const string fileContent = "File content.";

        [TestMethod]
        public void ShouldFailOnEmptyConfig()
        {
            // arrange.
            bool failed = false;
            try
            {
                // act.
                var t = new CreateSignDocumentController(new Mock<ICloudStorageProvider>().Object, null);
            }
            catch (System.ArgumentNullException ex) when (ex.ParamName == "config")
            {
                failed = true;
            }

            // assert.
            Assert.IsTrue(failed, "Should've failed on empty config.");
        }

        [TestMethod]
        public void ShouldFailOnEmptyProvider()
        {
            // arrange.
            bool failed = false;
            try
            {
                // act.
                var t = new CreateSignDocumentController(null, new Mock<IConfigParams>().Object);
            }
            catch (System.ArgumentNullException ex) when (ex.ParamName == "provider")
            {
                failed = true;
            }

            // assert.
            Assert.IsTrue(failed, "Should've failed on empty provider.");
        }

        [TestMethod]
        public async Task TestExtractFileName()
        {
            // arrange.
            var mockProvider = new Mock<ICloudStorageProvider>();
            var mockConfig = new Mock<IConfigParams>();
            var fileName = "";
            var t = new Mock<CreateSignDocumentController>(mockProvider.Object, mockConfig.Object);

            t.Protected().Setup<Task<string>>("GetRequestContent").Returns(Task.FromResult($"{{\"template\":\"{test}\"}}"));

            t.Protected().Setup<string>("GetFromStorage", ItExpr.IsAny<string>()).Returns(fileContent).Callback<string>((c) => fileName = c);

            // act.
            var result = await t.Object.Post();

            // assert.
            Assert.AreEqual(test, fileName, "Should've extracted file name.");
        }



        [TestMethod]
        public async Task TestReplaceNonExistingVars()
        {
            // arrange.
            var mockProvider = new Mock<ICloudStorageProvider>();
            var mockConfig = new Mock<IConfigParams>();
            var t = new Mock<CreateSignDocumentController>(mockProvider.Object, mockConfig.Object);

            t.Protected().Setup<Task<string>>("GetRequestContent").Returns(Task.FromResult($"{{\"template\":\"{test}\"}}"));

            t.Protected().Setup<string>("GetFromStorage", ItExpr.IsAny<string>()).Returns(nonExistingVar);

            // act.
            var result = await t.Object.Post();

            // assert.
            string s = result.HtmlResponse;
            Assert.AreEqual(StaticHelpers.NotAvailable, s, "Non existant variable should be NA.");
        }


        [TestMethod]
        public async Task TestReplaceVars()
        {
            // arrange.
            var mockProvider = new Mock<ICloudStorageProvider>();
            var mockConfig = new Mock<IConfigParams>();
            var t = new Mock<CreateSignDocumentController>(mockProvider.Object, mockConfig.Object);

            t.Protected().Setup<Task<string>>("GetRequestContent").Returns(Task.FromResult($"{{\"template\":\"{test}\",\"existingVar\":\"{value}\"}}"));

            t.Protected().Setup<string>("GetFromStorage", ItExpr.IsAny<string>()).Returns(existingVar);

            // act.
            var result = await t.Object.Post();

            // assert.
            Assert.AreEqual(value, result.HtmlResponse, "Should've replaced variable value.");
        }
    }
}
