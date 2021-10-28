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
    public class MedicarePlansControllerTests
    {
        private const string TableName = "ptas_medicareplans";
        private const string GetOpQuery = "$top=1000";

        /// <summary>
        /// Test the Get Method when a get MedicarePlansController is succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task MedicarePlansController_Get()
        {
            // Arrange
            var calledCRMWrapper = false;
            var calledMapper = false;
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsMedicarePlan>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsMedicarePlan[] { new DynamicsMedicarePlan { Id = new System.Guid().ToString() } };
            });
            moqmapper.Setup(m => m.Map<FormMedicarePlan>(It.IsAny<DynamicsMedicarePlan>())).Returns(() =>
            {
                calledMapper = true;
                return new FormMedicarePlan { Id = new System.Guid().ToString() };
            });

            var controller = new MedicarePlansController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<IEnumerable<FormMedicarePlan>> result = await controller.Get();

            // Assert`

            Assert.IsTrue(((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK) && calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsMedicarePlan>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{GetOpQuery}")));
        }
    }
}