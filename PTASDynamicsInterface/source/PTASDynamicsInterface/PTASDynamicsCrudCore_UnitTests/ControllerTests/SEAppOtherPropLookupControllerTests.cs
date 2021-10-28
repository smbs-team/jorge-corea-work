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
    /// SEAppOtherPropLookupController class test.
    /// </summary>
    [TestClass]
    public class SEAppOtherPropLookupControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string Name = "522201212";

        /// <summary>
        /// Test the Get Method when a get in SEAppOtherPropLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppOtherPropLookupController_Get_ReturnsNull()
        {
            // Arrange
            var moqSEAppOtherPropManager = new Mock<ISEAppOtherPropManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEAppOtherPropFromSEAppId = false;

            moqSEAppOtherPropManager.Setup(m => m.GetSEAppOtherPropFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEAppOtherPropFromSEAppId = true;
                return new List<DynamicsSEAppOtherProp> { };
            });

            var controller = new SEAppOtherPropLookupController(moqSEAppOtherPropManager.Object, moqmapper.Object);

            // Act

            ActionResult<FormSEAppOtherProp[]> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledGetSEAppOtherPropFromSEAppId);
            moqSEAppOtherPropManager.Verify(m => m.GetSEAppOtherPropFromSEAppId(It.Is<string>(s => s == $"{NonvalidIdForGet}")));

        }

        /// <summary>
        /// Test the Get Method when a get SEAppOtherPropLookupController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEAppOtherPropLookupController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqSEAppOtherPropManager = new Mock<ISEAppOtherPropManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEAppOtherPropFromSEAppId = false;

            moqSEAppOtherPropManager.Setup(m => m.GetSEAppOtherPropFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEAppOtherPropFromSEAppId = true;
                return new List<DynamicsSEAppOtherProp> {
                    new DynamicsSEAppOtherProp
                    { SEAppOtherPropId = new System.Guid().ToString(), Name = Name },
                    new DynamicsSEAppOtherProp
                    { SEAppOtherPropId = new System.Guid().ToString(), Name = Name }
                };
            });

            moqmapper.Setup<FormSEAppOtherProp[]>(m => m.Map<FormSEAppOtherProp[]>(It.IsAny<DynamicsSEAppOtherProp[]>())).Returns(() =>
            {
                //calledMapper = true;
                var res = new FormSEAppOtherProp[]
                {
                        new FormSEAppOtherProp { SEAppOtherPropId = new System.Guid(). ToString(), Name = Name } };
                return res;
            });

            var controller = new SEAppOtherPropLookupController(moqSEAppOtherPropManager.Object, moqmapper.Object);

            // Act
            ActionResult<FormSEAppOtherProp[]> result = await controller.Get(ValidId);

            // Assert
            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledGetSEAppOtherPropFromSEAppId);
            moqSEAppOtherPropManager.Verify(m => m.GetSEAppOtherPropFromSEAppId(It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}