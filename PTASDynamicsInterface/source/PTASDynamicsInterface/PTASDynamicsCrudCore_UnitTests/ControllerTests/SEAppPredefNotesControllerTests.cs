using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASDynamicsCrudCore.Controllers;
using PTASDynamicsCrudHelperClasses.Classes;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;
using PTASDynamicsCrudCore_UnitTests.ControllerTests;
using PTASDynamicsCrudCore_UnitTests.HelperClasses;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using PTASCRMHelpers;

namespace PTASDynamicsCrudCore_UnitTests
{
    [TestClass]
    public class SEAppPredefNotesControllerTests
    {
        private const string TableName = "ptas_seapppredefnoteses";
        private const string GetOpQuery = "$top=1000";

        /// <summary>
        /// Test the Get Method when a get SEAppPredefNotesController is succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task SEAppPredefNotesController_Get()
        {
            // Arrange
            var calledCRMWrapper = false;
            var calledMapper = false;
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSEAppPredefNote>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSEAppPredefNote[] { new DynamicsSEAppPredefNote { SEAppPredefNotesId = new System.Guid().ToString() } };
            });
            moqmapper.Setup(m => m.Map<FormSEAppPredefNote>(It.IsAny<DynamicsSEAppPredefNote>())).Returns(() =>
            {
                calledMapper = true;
                return new FormSEAppPredefNote { SEAppPredefNotesId = new System.Guid().ToString() };
            });

            var controller = new SEAppPredefNotesController(moqwrapper.Object, moqmapper.Object);
            // Act

            ActionResult<IEnumerable<FormSEAppPredefNote>> result = await controller.Get();

            // Assert`

            Assert.IsTrue(((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK) && calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSEAppPredefNote>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{GetOpQuery}")));
        }
    }
}