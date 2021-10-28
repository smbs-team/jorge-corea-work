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
    /// SEAppOccupantsController class test.
    /// </summary>
    [TestClass]
    public class SEAppOccupantsControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string TableName = "ptas_seappoccupants";
        private const string KeyField = "ptas_seappoccupantid";
        private const string Name = "John Doe";

        /// <summary>
        /// Test the Get Method when a get in SEAppOccupantsController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppOccupantsController_Get_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSEAppOccupant>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSEAppOccupant[] { };
            });
            var controller = new SEAppOccupantsController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormSEAppOccupant> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSEAppOccupant>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{NonvalidIdForGet}'")));
        }

        /// <summary>
        /// Test the Get Method when a get SEAppOccupantsController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEAppOccupantsController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<FormSEAppOccupant>(m => m.Map<FormSEAppOccupant>(It.IsAny<DynamicsSEAppOccupant>())).Returns(() =>
            {
                calledMapper = true;
                return new FormSEAppOccupant { SEAppOccupantId = new System.Guid().ToString(), Name = Name };
            });

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSEAppOccupant>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSEAppOccupant[] {
                    new DynamicsSEAppOccupant {SEAppOccupantId = new System.Guid().ToString(), Name=Name } };
            });
            var controller = new SEAppOccupantsController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormSEAppOccupant> result = await controller.Get(ValidId);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSEAppOccupant>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{ValidId}'")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEAppOccupantsController is succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEAppOccupantsController_Post_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSEAppOccupantForSave>(m => m.Map<DynamicsSEAppOccupantForSave>(It.IsAny<FormSEAppOccupant>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSEAppOccupantForSave { SEAppOccupantId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSEAppOccupantForSave>(It.IsAny<string>(), It.IsAny<DynamicsSEAppOccupantForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEAppOccupantsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSEAppOccupant value = new FormSEAppOccupant { SEAppOccupantId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSEAppOccupant> result = await controller.Post(value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSEAppOccupantForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSEAppOccupantForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEAppOccupantsController is not succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEAppOccupantsController_Post_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSEAppOccupantForSave>(m => m.Map<DynamicsSEAppOccupantForSave>(It.IsAny<FormSEAppOccupant>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSEAppOccupantForSave { SEAppOccupantId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSEAppOccupantForSave>(It.IsAny<string>(), It.IsAny<DynamicsSEAppOccupantForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEAppOccupantsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSEAppOccupant value = new FormSEAppOccupant { SEAppOccupantId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSEAppOccupant> result = await controller.Post(value);

            // Assert

            Assert.IsNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSEAppOccupantForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSEAppOccupantForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEAppOccupantsController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEAppOccupantsController_Patch_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSEAppOccupantForSave>(m => m.Map<DynamicsSEAppOccupantForSave>(It.IsAny<FormSEAppOccupant>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSEAppOccupantForSave { SEAppOccupantId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsSEAppOccupantForSave>(It.IsAny<string>(), It.IsAny<DynamicsSEAppOccupantForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEAppOccupantsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSEAppOccupant value = new FormSEAppOccupant { SEAppOccupantId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSEAppOccupant> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsSEAppOccupantForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSEAppOccupantForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEAppOccupantsController is not succefull
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppOccupantsController_Patch_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSEAppOccupantForSave>(m => m.Map<DynamicsSEAppOccupantForSave>(It.IsAny<FormSEAppOccupant>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSEAppOccupantForSave { SEAppOccupantId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsSEAppOccupantForSave>(It.IsAny<string>(), It.IsAny<DynamicsSEAppOccupantForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEAppOccupantsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSEAppOccupant value = new FormSEAppOccupant { SEAppOccupantId = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormSEAppOccupant> result = await controller.Patch(ValidId, value);

            // Assert
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsSEAppOccupantForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSEAppOccupantForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEAppOccupantsController is succefull.
        /// </summary>
        /// <returns>True.</returns>
        [TestMethod]
        public async Task SEAppOccupantsController_Delete_ReturnsTrue()
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
            var controller = new SEAppOccupantsController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormSEAppOccupant value = new FormSEAppOccupant { SEAppOccupantId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSEAppOccupant> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEAppOccupantsController is not succefull.
        /// </summary>
        /// <returns>false.</returns>
        [TestMethod]
        public async Task SEAppOccupantsController_Delete_ReturnsFalse()
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
            var controller = new SEAppOccupantsController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormSEAppOccupant value = new FormSEAppOccupant { SEAppOccupantId = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormSEAppOccupant> result = await controller.Delete(ValidId);

            // Assert
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}