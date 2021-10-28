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
    /// SEApplicationsController class test.
    /// </summary>
    [TestClass]
    public class SEApplicationsControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "123";
        private const string TableName = "ptas_seapplications";
        private const string KeyField = "ptas_seapplicationid";
        private const string AccountNumber = "522201212";

        /// <summary>
        /// Test the Get Method when a get in SEApplicationsController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEApplicationsController_Get_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplication>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSeniorExemptionApplication[] { };
            });
            var controller = new SEApplicationsController(moqwrapper.Object, moqmapper.Object, null);

            // Act

            ActionResult<FormSeniorExemptionApplication> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplication>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$expand=ptas_ptas_seapplication_ptas_seapppredefnotes&$filter={KeyField} eq '{NonvalidIdForGet}'")));
        }

        /// <summary>
        /// Test the Get Method when a get SEApplicationsController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEApplicationsController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<FormSeniorExemptionApplication>(m => m.Map<FormSeniorExemptionApplication>(It.IsAny<DynamicsSeniorExemptionApplication>())).Returns(() =>
            {
                calledMapper = true;
                return new FormSeniorExemptionApplication { SEAapplicationId = new System.Guid().ToString(), AccountNumber = AccountNumber };
            });

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplication>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSeniorExemptionApplication[] {
                    new DynamicsSeniorExemptionApplication {SEAapplicationId = new System.Guid().ToString(), AccountNumber=AccountNumber } };
            });
            var controller = new SEApplicationsController(moqwrapper.Object, moqmapper.Object, null);

            // Act

            ActionResult<FormSeniorExemptionApplication> result = await controller.Get(ValidId);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplication>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$expand=ptas_ptas_seapplication_ptas_seapppredefnotes&$filter={KeyField} eq '{ValidId}'")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEApplicationsController is succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEApplicationsController_Post_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationForSave>(m => m.Map<DynamicsSeniorExemptionApplicationForSave>(It.IsAny<FormSeniorExemptionApplication>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationForSave { SEAapplicationId = new System.Guid().ToString(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSeniorExemptionApplicationForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEApplicationsController(moqwrapper.Object, moqmapper.Object, null);

            // the object to be i
            FormSeniorExemptionApplication value = new FormSeniorExemptionApplication { SEAapplicationId = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormSeniorExemptionApplication> result = await controller.Post(value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSeniorExemptionApplicationForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEApplicationsController is not succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEApplicationsController_Post_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationForSave>(m => m.Map<DynamicsSeniorExemptionApplicationForSave>(It.IsAny<FormSeniorExemptionApplication>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationForSave { SEAapplicationId = new System.Guid().ToString(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSeniorExemptionApplicationForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEApplicationsController(moqwrapper.Object, moqmapper.Object, null);

            // the object to be i
            FormSeniorExemptionApplication value = new FormSeniorExemptionApplication { SEAapplicationId = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormSeniorExemptionApplication> result = await controller.Post(value);

            // Assert

            Assert.IsNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSeniorExemptionApplicationForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEApplicationsController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEApplicationsController_Patch_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup(m => m.Map<DynamicsSeniorExemptionApplicationForSave>(It.IsAny<FormSeniorExemptionApplication>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationForSave { SEAapplicationId = ValidId, AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePatch(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEApplicationsController(moqwrapper.Object, moqmapper.Object, null);

            // the object to be i
            FormSeniorExemptionApplication value = new FormSeniorExemptionApplication { SEAapplicationId = ValidId, AccountNumber = AccountNumber };

            // Act

            ActionResult<FormSeniorExemptionApplication> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEApplicationsController is not succefull
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEApplicationsController_Patch_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup(m => m.Map<DynamicsSeniorExemptionApplicationForSave>(It.IsAny<FormSeniorExemptionApplication>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationForSave { SEAapplicationId = new System.Guid().ToString(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsSeniorExemptionApplicationForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEApplicationsController(moqwrapper.Object, moqmapper.Object, null);

            // the object to be i
            FormSeniorExemptionApplication value = new FormSeniorExemptionApplication { SEAapplicationId = ValidId, AccountNumber = AccountNumber };

            // Act
            ActionResult<FormSeniorExemptionApplication> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEApplicationsController is succefull.
        /// </summary>
        /// <returns>True.</returns>
        [TestMethod]
        public async Task SEApplicationsController_Delete_ReturnsTrue()
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
            var controller = new SEApplicationsController(moqwrapper.Object, moqmapper.Object, null);

            // the object to be deleted
            FormSeniorExemptionApplication value = new FormSeniorExemptionApplication { SEAapplicationId = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormSeniorExemptionApplication> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEApplicationsController is not succefull.
        /// </summary>
        /// <returns>false.</returns>
        [TestMethod]
        public async Task SEApplicationsController_Delete_ReturnsFalse()
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
            var controller = new SEApplicationsController(moqwrapper.Object, moqmapper.Object, null);

            // the object to be deleted
            FormSeniorExemptionApplication value = new FormSeniorExemptionApplication { SEAapplicationId = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act
            ActionResult<FormSeniorExemptionApplication> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}