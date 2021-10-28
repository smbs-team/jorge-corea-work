using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using PTASCRMHelpers;

using PTASDynamicsCrudCore.Controllers;

using PTASDynamicsCrudCore_UnitTests.HelperClasses;

using PTASDynamicsCrudHelperClasses.Classes;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;

namespace PTASDynamicsCrudCore_UnitTests.ControllerTests
{
    [TestClass]
    public class OptionSetsControllerTests
    {
        private const string NoValidOptionSetValue = "nada";
        private const string OptionSetValue = "accounttypes";
        private const int NoValidOptionCountExpected = 0;
        private const string TableName = "stringmaps";
        private const string AttributeName = "ptas_accounttype";

        [TestMethod]
        public async Task OptionSetsController_Get_InvalidOptionSet()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);

            var calledCRMWrapper = false;
            OptionSet[] expectedResult = new OptionSet[] { };

            moqwrapper.Setup(m => m.ExecuteGet<OptionSet>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                // should never get here
                calledCRMWrapper = true;
                return new OptionSet[] { };
            });

            var controller = new Mock<OptionSetsController>(moqwrapper.Object);

            // Act

            ActionResult<IEnumerable<PTASDynamicsCrudHelperClasses.JSONMappings.OptionSet>> result = await controller.Object.Get(NoValidOptionSetValue);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsFalse(calledCRMWrapper);
            Assert.AreNotSame(expectedResult, result);

            // there is no verify because the flow doenst call the Execute Get
        }

        [TestMethod]
        public async Task OptionSetsController_Get_ValidOptionSet()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);

            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteGet<OptionSet>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new OptionSet[]
                {
                    new OptionSet
                    {
                        Value = "Real Property",
                        Attributename = "ptas_accounttype",
                        Versionnumber = "1275990",
                        Langid = "1033",
                        Objecttypecode = "ptas_seapplication",
                    Stringmapid = "5b9fe457-f378-e911-a980-001dd800b582",
                        Organizationid = "8dcf1c09-56fb-4cdd-af75-54b14b04349b",
                        Displayorder = "1",
                        AttributeValue = 668020000
                    },
                    new OptionSet
                    {
                        Value = "Real Property",
                        Attributename = "ptas_accounttype",
                        Versionnumber = "1275991",
                        Langid = "1033",
                        Objecttypecode = "ptas_seapplication",
                        Stringmapid = "5c9fe457-f378-e911-a980-001dd800b582",
                        Organizationid = "8dcf1c09-56fb-4cdd-af75-54b14b04349b",
                        Displayorder = "2",
                        AttributeValue = 668020001
                    },
                };
            });

            var controller = new Mock<OptionSetsController>(moqwrapper.Object);

            // Act

            ActionResult<IEnumerable<OptionSet>> result = await controller.Object.Get(OptionSetValue);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteGet<OptionSet>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<string>(s => s == $"$filter=attributename eq '{AttributeName}'")));
        }
    }
}