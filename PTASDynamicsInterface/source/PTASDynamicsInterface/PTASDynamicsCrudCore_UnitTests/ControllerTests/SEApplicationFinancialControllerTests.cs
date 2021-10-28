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
    /// SEApplicationFinancialController class test.
    /// </summary>
    [TestClass]
    public class SEApplicationFinancialControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "123";
        private const string TableName = "ptas_sefinancialformses";
        private const string KeyField = "ptas_sefinancialformsid";
        private const string Name = "John Doe";

        /// <summary>
        /// Test the Get Method when a get in SEApplicationFinancialController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEApplicationFinancialController_Get_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSeniorExemptionApplicationFinancial[] { };
            });
            var controller = new SEApplicationFinancialController(moqwrapper.Object, moqmapper.Object, moqconfig.Object);

            // Act

            ActionResult<FormSeniorExemptionApplicationFinancial> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{NonvalidIdForGet}'")));
        }

        /// <summary>
        /// Test the Get Method when a get SEApplicationFinancialController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEApplicationFinancialController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<FormSeniorExemptionApplicationFinancial>(m => m.Map<FormSeniorExemptionApplicationFinancial>(It.IsAny<DynamicsSeniorExemptionApplicationFinancial>())).Returns(() =>
            {
                calledMapper = true;
                return new FormSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString().ToString(), Name = Name };
            });

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSeniorExemptionApplicationFinancial[] {
                    new DynamicsSeniorExemptionApplicationFinancial {SEFinancialFormsId = new System.Guid().ToString(), Name=Name } };
            });
            var controller = new SEApplicationFinancialController(moqwrapper.Object, moqmapper.Object, moqconfig.Object);

            // Act

            ActionResult<FormSeniorExemptionApplicationFinancial> result = await controller.Get(ValidId);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{ValidId}'")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEApplicationFinancialController is succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEApplicationFinancialController_Post_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationFinancialForSave>(m => m.Map<DynamicsSeniorExemptionApplicationFinancialForSave>(It.IsAny<FormSeniorExemptionApplicationFinancial>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationFinancialForSave { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSeniorExemptionApplicationFinancialForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationFinancialForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEApplicationFinancialController(moqwrapper.Object, moqmapper.Object, moqconfig.Object);

            // the object to be i
            FormSeniorExemptionApplicationFinancial value = new FormSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSeniorExemptionApplicationFinancial> result = await controller.Post(value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSeniorExemptionApplicationFinancialForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationFinancialForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEApplicationFinancialController is not succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEApplicationFinancialController_Post_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationFinancialForSave>(m => m.Map<DynamicsSeniorExemptionApplicationFinancialForSave>(It.IsAny<FormSeniorExemptionApplicationFinancial>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationFinancialForSave { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSeniorExemptionApplicationFinancialForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationFinancialForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEApplicationFinancialController(moqwrapper.Object, moqmapper.Object, moqconfig.Object);

            // the object to be i
            FormSeniorExemptionApplicationFinancial value = new FormSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSeniorExemptionApplicationFinancial> result = await controller.Post(value);

            // Assert

            Assert.IsNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSeniorExemptionApplicationFinancialForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationFinancialForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEApplicationFinancialController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEApplicationFinancialController_Patch_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationFinancialForSave>(m => m.Map<DynamicsSeniorExemptionApplicationFinancialForSave>(It.IsAny<FormSeniorExemptionApplicationFinancial>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationFinancialForSave { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsSeniorExemptionApplicationFinancialForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationFinancialForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEApplicationFinancialController(moqwrapper.Object, moqmapper.Object, moqconfig.Object);

            // the object to be i
            FormSeniorExemptionApplicationFinancial value = new FormSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSeniorExemptionApplicationFinancial> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsSeniorExemptionApplicationFinancialForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationFinancialForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEApplicationFinancialController is not succefull
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEApplicationFinancialController_Patch_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationFinancialForSave>(m => m.Map<DynamicsSeniorExemptionApplicationFinancialForSave>(It.IsAny<FormSeniorExemptionApplicationFinancial>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationFinancialForSave { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsSeniorExemptionApplicationFinancialForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationFinancialForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEApplicationFinancialController(moqwrapper.Object, moqmapper.Object, moqconfig.Object);

            // the object to be i
            FormSeniorExemptionApplicationFinancial value = new FormSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormSeniorExemptionApplicationFinancial> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsSeniorExemptionApplicationFinancialForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationFinancialForSave>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEApplicationFinancialController is succefull.
        /// </summary>
        /// <returns>True.</returns>
        [TestMethod]
        public async Task FileAttachementMetadataController_Delete_ReturnsTrue()
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
            var controller = new SEApplicationFinancialController(moqwrapper.Object, moqmapper.Object, moqconfig.Object);

            // the object to be deleted
            FormSeniorExemptionApplicationFinancial value = new FormSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormSeniorExemptionApplicationFinancial> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEApplicationFinancialController is not succefull.
        /// </summary>
        /// <returns>false.</returns>
        [TestMethod]
        public async Task FileAttachementMetadataController_Delete_ReturnsFalse()
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
            var controller = new SEApplicationFinancialController(moqwrapper.Object, moqmapper.Object, moqconfig.Object);

            // the object to be deleted
            FormSeniorExemptionApplicationFinancial value = new FormSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormSeniorExemptionApplicationFinancial> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}