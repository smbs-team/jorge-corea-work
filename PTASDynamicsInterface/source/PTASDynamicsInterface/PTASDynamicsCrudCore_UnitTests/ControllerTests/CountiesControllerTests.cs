using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using PTASCRMHelpers;

using PTASDynamicsCrudCore.Controllers;

using PTASDynamicsCrudCore_UnitTests.HelperClasses;

using PTASDynamicsCrudHelperClasses.Classes;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;

namespace PTASDynamicsCrudCore_UnitTests
{
    [TestClass]
    public class CountiesControllerTests
    {
        private const string TableName = "ptas_counties";
        private const string GetOpQuery = "$top=1000";

        /// <summary>
        /// Test the Get Method when a get CountiesController is succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task CountiesController_Get()
        {
            // Arrange
            var calledCRMWrapper = false;
            var calledMapper = false;
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsCounty>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsCounty[] { new DynamicsCounty { CountyId = new System.Guid() } };
            });
            moqmapper.Setup(m => m.Map<OutgoingCounty>(It.IsAny<DynamicsCounty>())).Returns(() =>
            {
                calledMapper = true;
                return new OutgoingCounty { CountyId = new System.Guid() };
            });

            var controller = new CountiesController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<IEnumerable<OutgoingCounty>> result = await controller.Get();

            // Assert`

            Assert.IsTrue(((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK) && calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsCounty>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{GetOpQuery}")));
        }
    }
}