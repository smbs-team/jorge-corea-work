using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASDynamicsCrudCore.Controllers;
using PTASDynamicsCrudHelperClasses.Classes;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;
using PTASDynamicsCrudCore_UnitTests.HelperClasses;
using System.Net;
using PTASCRMHelpers;

namespace PTASDynamicsCrudCore_UnitTests
{
    [TestClass]
    public class MedicarePlans1ControllerTests
    {
        private const string TableName = "ptas_medicareplans";
        private const string Name = "John Doe";
        private const string KeyField = "ptas_medicareplanid";
        private const string ValidId = "3E0000B6-DAC1-4AC2-967C-725AAD35BC04";
        private const string NonValidId = "00000000-DAC1-4AC2-967C-725AAD35BC04";

        /// <summary>
        /// Test the Get Method when a get MedicarePlansController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task MedicarePlansController_Get_ReturnsVoid()
        {
            // Arrange
            var calledCRMWrapper = false;
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsMedicarePlan>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsMedicarePlan[] { };
            });

            var controller = new MedicarePlans1Controller(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormMedicarePlan> result = await controller.Get(NonValidId);

            // Assert`

            Assert.IsTrue(calledCRMWrapper);
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsMedicarePlan>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{NonValidId}'")));
        }

        /// <summary>
        /// Test the Get Method when a get MedicarePlansController is succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task MedicarePlansController_Get_ReturnsNotNull()
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

            var controller = new MedicarePlans1Controller(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormMedicarePlan> result = await controller.Get(ValidId);

            // Assert`

            Assert.IsTrue(calledCRMWrapper && calledMapper);
            Assert.IsNotNull((result.Result as JsonResult).Value);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsMedicarePlan>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{ValidId}'")));
        }

        /// <summary>
        /// Test the Post Method when a post in  MedicarePlansController is not succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task MedicarePlansController_Post_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsMedicarePlan>(m => m.Map<DynamicsMedicarePlan>(It.IsAny<FormMedicarePlan>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsMedicarePlan { Id = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsMedicarePlan>(It.IsAny<string>(), It.IsAny<DynamicsMedicarePlan>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new MedicarePlans1Controller(moqwrapper.Object, moqmapper.Object);

            // the object to be inserted
            FormMedicarePlan value = new FormMedicarePlan { Id = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormMedicarePlan> result = await controller.Post(value);

            // Assert

            Assert.IsNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsMedicarePlan>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsMedicarePlan>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Post Method when a post in  MedicarePlansController is succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task MedicarePlansController_Post_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsMedicarePlan>(m => m.Map<DynamicsMedicarePlan>(It.IsAny<FormMedicarePlan>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsMedicarePlan { Id = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsMedicarePlan>(It.IsAny<string>(), It.IsAny<DynamicsMedicarePlan>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new MedicarePlans1Controller(moqwrapper.Object, moqmapper.Object);

            // the object to be inserted
            FormMedicarePlan value = new FormMedicarePlan { Id = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormMedicarePlan> result = await controller.Post(value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsMedicarePlan>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsMedicarePlan>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Path Method when an update on ConctactsController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task MedicarePlansController_Patch_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsMedicarePlan>(m => m.Map<DynamicsMedicarePlan>(It.IsAny<FormMedicarePlan>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsMedicarePlan { Id = new System.Guid().ToString(), Name = Name };
            });

            moqwrapper.Setup(m => m.ExecutePatch<DynamicsMedicarePlan>(It.IsAny<string>(), It.IsAny<DynamicsMedicarePlan>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new MedicarePlans1Controller(moqwrapper.Object, moqmapper.Object);

            // the object to be updated
            FormMedicarePlan value = new FormMedicarePlan { Id = ValidId, Name = Name };

            // Act

            ActionResult<FormMedicarePlan> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsMedicarePlan>(It.Is<string>(s => s == $"{TableName}"), It.Is<DynamicsMedicarePlan>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Path Method when an update on MedicarePlansController is not succefull
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task MedicarePlansController_Patch_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsMedicarePlan>(m => m.Map<DynamicsMedicarePlan>(It.IsAny<FormMedicarePlan>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsMedicarePlan { Id = new System.Guid().ToString(), Name = Name };
            });

            moqwrapper.Setup(m => m.ExecutePatch<DynamicsMedicarePlan>(It.IsAny<string>(), It.IsAny<DynamicsMedicarePlan>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });

            var controller = new MedicarePlans1Controller(moqwrapper.Object, moqmapper.Object);

            // the object to be updated
            FormMedicarePlan value = new FormMedicarePlan { Id = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormMedicarePlan> result = await controller.Patch(ValidId, value);

            // Assert
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            //moqwrapper.Verify(m => m.ExecutePatch<DynamicsMedicarePlan>(It.Is<string>(s => s == $"{TableName}"),
            //    It.Is<DynamicsMedicarePlan>(s => s.FirstName == FirstName),
            //    It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on MedicarePlansController is succefull.
        /// </summary>
        /// <returns>True.</returns>
        [TestMethod]
        public async Task MedicarePlansController_Delete_ReturnsTrue()
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
            var controller = new MedicarePlans1Controller(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormMedicarePlan value = new FormMedicarePlan { Id = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormMedicarePlan> result = await controller.Delete(ValidId);

            // Assert
            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on MedicarePlansController is not succefull.
        /// </summary>
        /// <returns>false.</returns>
        [TestMethod]
        public async Task MedicarePlansController_Delete_ReturnsFalse()
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
            var controller = new MedicarePlans1Controller(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormMedicarePlan value = new FormMedicarePlan { Id = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormMedicarePlan> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}