using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASDynamicsCrudCore.Controllers;
using PTASDynamicsCrudHelperClasses.Classes;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;
using PTASDynamicsCrudCore_UnitTests.HelperClasses;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using PTASCRMHelpers;

namespace PTASDynamicsCrudCore_UnitTests
{
    [TestClass]
    public class ContactsControllerTests
    {
        private const string TableName = "contacts";
        private const string FirstName = "John";
        private const string KeyField = "contactid";
        private const string ValidId = "3E0000B6-DAC1-4AC2-967C-725AAD35BC04";
        private const string NonValidId = "00000000-DAC1-4AC2-967C-725AAD35BC04";

        /// <summary>
        /// Test the Get Method when a get ContactsController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task ContactsController_Get_ReturnsNull()
        {
            // Arrange
            var calledCRMWrapper = false;
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsContact>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsContact[] { };
            });

            var controller = new ContactsController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormContact> result = await controller.Get(NonValidId);

            // Assert`

            Assert.IsTrue(calledCRMWrapper);
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsContact>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{NonValidId}'")));
        }

        /// <summary>
        /// Test the Get Method when a get ContactsController is succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task ContactsController_Get_ReturnsNotNull()
        {
            // Arrange
            var calledCRMWrapper = false;
            var calledMapper = false;
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsContact>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsContact[] { new DynamicsContact { ContactId = new System.Guid().ToString() } };
            });
            moqmapper.Setup(m => m.Map<FormContact>(It.IsAny<DynamicsContact>())).Returns(() =>
            {
                calledMapper = true;
                return new FormContact { ContactId = new System.Guid().ToString() };
            });

            var controller = new ContactsController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormContact> result = await controller.Get(ValidId);

            // Assert`
            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsContact>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{ValidId}'")));
        }

        /// <summary>
        /// Test the Post Method when a post in  ConctactController is not succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task ContactController_Post_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);

            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);

            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsContact>(m => m.Map<DynamicsContact>(It.IsAny<FormContact>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsContact { ContactId = new System.Guid().ToString(), FirstName = FirstName };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsContact>(It.IsAny<string>(), It.IsAny<DynamicsContact>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new ContactsController(moqwrapper.Object, moqmapper.Object);

            // the object to be inserted
            FormContact value = new FormContact { ContactId = new System.Guid().ToString(), FirstName = FirstName };

            // Act

            ActionResult<FormContact> result = await controller.Post(value);

            // Assert

            Assert.IsNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsContact>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsContact>(s => s.FirstName == FirstName),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Post Method when a post in  ContactsController is succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task ContactController_Post_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsContact>(m => m.Map<DynamicsContact>(It.IsAny<FormContact>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsContact { ContactId = new System.Guid().ToString(), FirstName = FirstName };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsContact>(It.IsAny<string>(), It.IsAny<DynamicsContact>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new ContactsController(moqwrapper.Object, moqmapper.Object);

            // the object to be inserted
            FormContact value = new FormContact { ContactId = new System.Guid().ToString(), FirstName = FirstName };

            // Act

            ActionResult<FormContact> result = await controller.Post(value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsContact>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsContact>(s => s.FirstName == FirstName),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Path Method when an update on ConctactsController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task ContactsController_Patch_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsContact>(m => m.Map<DynamicsContact>(It.IsAny<FormContact>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsContact { ContactId = new System.Guid().ToString(), FirstName = FirstName };
            });

            moqwrapper.Setup(m => m.ExecutePatch<DynamicsContact>(It.IsAny<string>(), It.IsAny<DynamicsContact>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new ContactsController(moqwrapper.Object, moqmapper.Object);

            // the object to be updated
            FormContact value = new FormContact { ContactId = ValidId, FirstName = FirstName };

            // Act

            ActionResult<FormContact> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsContact>(It.Is<string>(s => s == $"{TableName}"), It.Is<DynamicsContact>(s => s.FirstName == FirstName),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Path Method when an update on ContactsController is not succefull
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task ContactsController_Patch_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsContact>(m => m.Map<DynamicsContact>(It.IsAny<FormContact>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsContact { ContactId = new System.Guid().ToString(), FirstName = FirstName };
            });

            moqwrapper.Setup(m => m.ExecutePatch<DynamicsContact>(It.IsAny<string>(), It.IsAny<DynamicsContact>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });

            var controller = new ContactsController(moqwrapper.Object, moqmapper.Object);

            // the object to be updated
            FormContact value = new FormContact { ContactId = new System.Guid().ToString(), FirstName = FirstName };

            // Act
            ActionResult<FormContact> result = await controller.Patch(ValidId, value);

            // Assert
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsContact>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsContact>(s => s.FirstName == FirstName),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on ContactsController is succefull.
        /// </summary>
        /// <returns>True.</returns>
        [TestMethod]
        public async Task ContactsController_Delete_ReturnsTrue()
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
            var controller = new ContactsController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormContact value = new FormContact { ContactId = new System.Guid().ToString(), FirstName = FirstName };

            // Act

            ActionResult<FormContact> result = await controller.Delete(ValidId);

            // Assert
            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on ContactsController is not succefull.
        /// </summary>
        /// <returns>false.</returns>
        [TestMethod]
        public async Task ContactsController_Delete_ReturnsFalse()
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
            var controller = new ContactsController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormContact value = new FormContact { ContactId = new System.Guid().ToString(), FirstName = FirstName };

            // Act

            ActionResult<FormContact> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}