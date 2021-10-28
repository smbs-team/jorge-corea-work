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
    /// <summary>
    /// Test class for the controller that Map of statuscode, its value and its state code.
    /// </summary>
    [TestClass]
    public class StatusCodesControllerTests
    {
        private const string NoValidStatusCodeValue = "nada";
        private const string StatusCodeValue = "seniordetail";
        private const int VoidStatusCodeSetCountExpected = 0;
        private const int NonVoidStatusCodeSetCountExpected = 2;

        private const string TableNameForState = "statusmaps";
        private const string TableNameForStatus = "stringmaps";
        private const string objecttypecode = "ptas_seappdetail";

        /// <summary>
        /// Map when the entity on the get doenst exists  so there aren't results.
        /// </summary>

        [TestMethod]
        public async Task StatusCodesController_Get_ReturnsVoid()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);

            var calledCRMWrappeForStatusValues = false;

            moqwrapper.Setup(m => m.ExecuteGet<OptionSet>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                // should never get here
                calledCRMWrappeForStatusValues = true;
                return new OptionSet[] { };
            });

            var calledCRMWrappeForStatusCodes = false;

            moqwrapper.Setup(m => m.ExecuteGet<StatusCodeSet>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                // should never get here
                calledCRMWrappeForStatusValues = true;
                return new StatusCodeSet[] { };
            });

            var controller = new Mock<StatusCodesController>(moqwrapper.Object);

            // Act

            ActionResult<IEnumerable<StatusCodeSet>> result = await controller.Object.Get(NoValidStatusCodeValue);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsFalse(calledCRMWrappeForStatusValues && calledCRMWrappeForStatusCodes);
            // there is no verify because the flow doenst call the Execute Get
        }

        /// <summary>
        /// Map when the entity on the get exists  so there are non void results.
        /// </summary>
        [TestMethod]
        public async Task StatusCodesController_Get_ReturnsNonVoid()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);

            var calledCRMWrappeForStatusValues = false;

            moqwrapper.Setup(m => m.ExecuteGet<OptionSet>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrappeForStatusValues = true;
                return new OptionSet[] {
                    new OptionSet
                    {
                        Value = "In progress",
                        Attributename = "statuscode",
                        Versionnumber = "5830273",
                        Langid = "1033",
                        Objecttypecode = "ptas_seappdetail",
                        AttributeValue = 668020000,
                        Stringmapid = "f415a887-b9e3-e911-a989-001dd8009f4b",
                        Organizationid = "8dcf1c09-56fb-4cdd-af75-54b14b04349b",
                        Displayorder = "3"
                    },
                    new OptionSet
                    {
                        Value = "Pending - taxpayer",
                        Attributename = "statuscode",
                        Versionnumber = "5830274",
                        Langid = "1033",
                        Objecttypecode = "ptas_seappdetail",
                        AttributeValue = 668020014,
                        Stringmapid = "f615a887-b9e3-e911-a989-001dd8009f4b",
                        Organizationid = "8dcf1c09-56fb-4cdd-af75-54b14b04349b",
                        Displayorder = "3"
                    }
                };
            });

            var calledCRMWrappeForStatusCodes = false;

            moqwrapper.Setup(m => m.ExecuteGet<StatusCodeSet>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrappeForStatusCodes = true;
                return new StatusCodeSet[]
                {
                    new StatusCodeSet
                    {
                        Status = 668020014,
                        Value = "Completed - void",
                        State = 1
                    },
                    new StatusCodeSet
                    {
                        Status =  668020000,
                        Value = "In progress",
                        State = 0
                    }
                };
            });

            var controller = new Mock<StatusCodesController>(moqwrapper.Object);

            // Act

            ActionResult<IEnumerable<StatusCodeSet>> result = await controller.Object.Get(StatusCodeValue);

            // Assert
            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrappeForStatusValues && calledCRMWrappeForStatusCodes);
            moqwrapper.Verify(m => m.ExecuteGet<OptionSet>(It.Is<string>(s => s == $"{TableNameForStatus}"),
                It.Is<string>(s => s == $"$filter=objecttypecode eq '{objecttypecode}' and attributename eq 'statuscode'")));
            moqwrapper.Verify(m => m.ExecuteGet<StatusCodeSet>(It.Is<string>(s => s == $"{TableNameForState}"),
                It.Is<string>(s => s == $"$filter=objecttypecode eq '{objecttypecode}'")));
        }
    }
}