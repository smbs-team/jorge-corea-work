using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASDynamicsCrudCore.Controllers;
using PTASDynamicsCrudHelperClasses.Classes;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;

namespace PTASDynamicsCrudCore_UnitTests.ControllerTests
{
    /// <summary>
    /// EAppNoteLookupController class test.
    /// </summary>
    [TestClass]
    public class SEAppOccupantLookupControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string Name = "522201212";

        /// <summary>
        /// Test the Get Method when a get in SEAppOccupantLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppOccupantLookupController_Get_ReturnsNull()
        {
            // Arrange
            var moqSEAOccupantManager = new Mock<ISEAppOccupantManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEOccupantFromSEAppId = false;

            moqSEAOccupantManager.Setup(m => m.GetSEAppOccupantFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEOccupantFromSEAppId = true;
                return new List<DynamicsSEAppOccupant> { };
            });

            var controller = new SEAppOccupantLookupController(moqSEAOccupantManager.Object, moqmapper.Object);

            // Act

            ActionResult<FormSEAppOccupant[]> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledGetSEOccupantFromSEAppId);
            moqSEAOccupantManager.Verify(m => m.GetSEAppOccupantFromSEAppId(It.Is<string>(s => s == $"{NonvalidIdForGet}")));

        }

        /// <summary>
        /// Test the Get Method when a get SEAppOccupantLookupController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEAppOccupantLookupController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqSEAOccupantManager = new Mock<ISEAppOccupantManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEOccupantFromSEAppId = false;

            moqSEAOccupantManager.Setup(m => m.GetSEAppOccupantFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEOccupantFromSEAppId = true;
                return new List<DynamicsSEAppOccupant> {
                    new DynamicsSEAppOccupant
                    {SEAppOccupantId = new System.Guid().ToString(), Name = Name },
                    new DynamicsSEAppOccupant
                    {SEAppOccupantId = new System.Guid().ToString(), Name = Name }
                };
            });

            moqmapper.Setup<FormSEAppOccupant[]>(m => m.Map<FormSEAppOccupant[]>(It.IsAny<DynamicsSEAppOccupant[]>())).Returns(() =>
            {
                //calledMapper = true;
                var res = new FormSEAppOccupant[]
                {
                    new FormSEAppOccupant {SEAppOccupantId = new System.Guid().ToString(), Name = Name }
                };
                return res;
            });

            var controller = new SEAppOccupantLookupController(moqSEAOccupantManager.Object, moqmapper.Object);

            // Act
            ActionResult<FormSEAppOccupant[]> result = await controller.Get(ValidId);

            // Assert
            // Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledGetSEOccupantFromSEAppId);
            moqSEAOccupantManager.Verify(m => m.GetSEAppOccupantFromSEAppId(It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}