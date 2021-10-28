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
    public class YearsControllerTests
    {
        private const string TableName = "ptas_years";
        private const string GetOpQuery = "$top=1000";

        /// <summary>
        /// Test the Get Method when a get YearsController is succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task YearsController_Get()
        {
            // Arrange
            var calledCRMWrapper = false;
            var calledMapper = false;
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsYear>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsYear[] { new DynamicsYear { YearId = new System.Guid() } };
            });
            moqmapper.Setup(m => m.Map<OutgoingYear>(It.IsAny<DynamicsYear>())).Returns(() =>
            {
                calledMapper = true;
                return new OutgoingYear { YearId = new System.Guid() };
            });

            var controller = new YearsController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<IEnumerable<OutgoingYear>> result = await controller.Get();

            // Assert`

            Assert.IsTrue(((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK) && calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsYear>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{GetOpQuery}")));
        }
    }
}