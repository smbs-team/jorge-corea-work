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

namespace PTASDynamicsCrudCore_UnitTests.ControllerTests
{
    /// <summary>
    /// SEAppOtherPropsController class test.
    /// </summary>
    [TestClass]
    public class SEAppOtherPropsControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string TableName = "ptas_seappotherprops";
        private const string KeyField = "ptas_seappotherpropid";
        private const string Name = "522201212";

        /// <summary>
        /// Test the Get Method when a get in SEAppOtherPropsController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppOtherPropsController_Get_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSEAppOtherProp>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSEAppOtherProp[] { };
            });
            var controller = new SEAppOtherPropsController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormSEAppOtherProp> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSEAppOtherProp>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{NonvalidIdForGet}'")));
        }

        /// <summary>
        /// Test the Get Method when a get SEAppOtherPropsController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEAppOtherPropsController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<FormSEAppOtherProp>(m => m.Map<FormSEAppOtherProp>(It.IsAny<DynamicsSEAppOtherProp>())).Returns(() =>
            {
                calledMapper = true;
                return new FormSEAppOtherProp { SEAppOtherPropId = new System.Guid().ToString().ToString(), Name = Name };
            });

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSEAppOtherProp>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSEAppOtherProp[] {
                    new DynamicsSEAppOtherProp {SEAppOtherPropId = new System.Guid().ToString(), Name=Name } };
            });
            var controller = new SEAppOtherPropsController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormSEAppOtherProp> result = await controller.Get(ValidId);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSEAppOtherProp>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{ValidId}'")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEAppOtherPropsController is succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEAppOtherPropsController_Post_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSEAppOtherPropForSave>(m => m.Map<DynamicsSEAppOtherPropForSave>(It.IsAny<FormSEAppOtherProp>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSEAppOtherPropForSave { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSEAppOtherPropForSave>(It.IsAny<string>(), It.IsAny<DynamicsSEAppOtherPropForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEAppOtherPropsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSEAppOtherProp value = new FormSEAppOtherProp { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSEAppOtherProp> result = await controller.Post(value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSEAppOtherPropForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSEAppOtherPropForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEAppOtherPropsController is not succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEAppOtherPropsController_Post_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSEAppOtherPropForSave>(m => m.Map<DynamicsSEAppOtherPropForSave>(It.IsAny<FormSEAppOtherProp>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSEAppOtherPropForSave { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSEAppOtherPropForSave>(It.IsAny<string>(), It.IsAny<DynamicsSEAppOtherPropForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEAppOtherPropsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSEAppOtherProp value = new FormSEAppOtherProp { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSEAppOtherProp> result = await controller.Post(value);

            // Assert

            Assert.IsNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSEAppOtherPropForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSEAppOtherPropForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEAppOtherPropsController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEAppOtherPropsController_Patch_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSEAppOtherPropForSave>(m => m.Map<DynamicsSEAppOtherPropForSave>(It.IsAny<FormSEAppOtherProp>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSEAppOtherPropForSave { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsSEAppOtherPropForSave>(It.IsAny<string>(), It.IsAny<DynamicsSEAppOtherPropForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEAppOtherPropsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSEAppOtherProp value = new FormSEAppOtherProp { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSEAppOtherProp> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsSEAppOtherPropForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSEAppOtherPropForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEAppOtherPropsController is not succefull
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppOtherPropsController_Patch_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSEAppOtherPropForSave>(m => m.Map<DynamicsSEAppOtherPropForSave>(It.IsAny<FormSEAppOtherProp>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSEAppOtherPropForSave { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsSEAppOtherPropForSave>(It.IsAny<string>(), It.IsAny<DynamicsSEAppOtherPropForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEAppOtherPropsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSEAppOtherProp value = new FormSEAppOtherProp { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormSEAppOtherProp> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsSEAppOtherPropForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSEAppOtherPropForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEAppOtherPropsController is succefull.
        /// </summary>
        /// <returns>True.</returns>
        [TestMethod]
        public async Task SEAppOtherPropsController_Delete_ReturnsTrue()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteDelete(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEAppOtherPropsController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormSEAppOtherProp value = new FormSEAppOtherProp { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSEAppOtherProp> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEAppOtherPropsController is not succefull.
        /// </summary>
        /// <returns>false.</returns>
        [TestMethod]
        public async Task SEAppOtherPropsController_Delete_ReturnsFalse()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteDelete(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEAppOtherPropsController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormSEAppOtherProp value = new FormSEAppOtherProp { SEAppOtherPropId = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormSEAppOtherProp> result = await controller.Delete(ValidId);

            // Assert
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}