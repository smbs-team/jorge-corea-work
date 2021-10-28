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
    /// SEApplicationDetailsController class test.
    /// </summary>
    [TestClass]
    public class SEApplicationDetailsControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string TableName = "ptas_seappdetails";
        private const string KeyField = "ptas_seappdetailid";
        private const string AccountNumber = "522201212";

        /// <summary>
        /// Test the Get Method when a get in SEApplicationDetailsController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEApplicationDetailsController_Get_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationDetail>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSeniorExemptionApplicationDetail[] { };
            });
            var controller = new SEApplicationDetailsController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormSeniorExemptionApplicationDetail> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationDetail>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{NonvalidIdForGet}'")));
        }

        /// <summary>
        /// Test the Get Method when a get SEApplicationDetailsController is succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEApplicationDetailsController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<FormSeniorExemptionApplicationDetail>(m => m.Map<FormSeniorExemptionApplicationDetail>(It.IsAny<DynamicsSeniorExemptionApplicationDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new FormSeniorExemptionApplicationDetail { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };
            });

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationDetail>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSeniorExemptionApplicationDetail[] {
                    new DynamicsSeniorExemptionApplicationDetail {SEAppdetailid = new System.Guid().ToString(), AccountNumber=AccountNumber } };
            });
            var controller = new SEApplicationDetailsController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormSeniorExemptionApplicationDetail> result = await controller.Get(ValidId);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationDetail>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{ValidId}'")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEApplicationDetailsController is succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEApplicationDetailsController_Post_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationDetailForSave>(m => m.Map<DynamicsSeniorExemptionApplicationDetailForSave>(It.IsAny<FormSeniorExemptionApplicationDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationDetailForSave { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSeniorExemptionApplicationDetailForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationDetailForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEApplicationDetailsController(moqwrapper.Object, moqmapper.Object);

            // the object to be insert
            FormSeniorExemptionApplicationDetail value = new FormSeniorExemptionApplicationDetail { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormSeniorExemptionApplicationDetail> result = await controller.Post(value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSeniorExemptionApplicationDetailForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationDetailForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Post Method when a post in  SEApplicationDetailsController is not succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEApplicationDetailsController_Post_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationDetailForSave>(m => m.Map<DynamicsSeniorExemptionApplicationDetailForSave>(It.IsAny<FormSeniorExemptionApplicationDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationDetailForSave { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsSeniorExemptionApplicationDetailForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationDetailForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEApplicationDetailsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSeniorExemptionApplicationDetail value = new FormSeniorExemptionApplicationDetail { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormSeniorExemptionApplicationDetail> result = await controller.Post(value);

            // Assert

            Assert.IsNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsSeniorExemptionApplicationDetailForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationDetailForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEApplicationDetailsController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task SEApplicationDetailsController_Patch_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationDetailForSave>(m => m.Map<DynamicsSeniorExemptionApplicationDetailForSave>(It.IsAny<FormSeniorExemptionApplicationDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationDetailForSave { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsSeniorExemptionApplicationDetailForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationDetailForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new SEApplicationDetailsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSeniorExemptionApplicationDetail value = new FormSeniorExemptionApplicationDetail { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormSeniorExemptionApplicationDetail> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsSeniorExemptionApplicationDetailForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationDetailForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Path Method when an update on SEApplicationDetailsController is not succefull
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEApplicationDetailsController_Patch_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsSeniorExemptionApplicationDetailForSave>(m => m.Map<DynamicsSeniorExemptionApplicationDetailForSave>(It.IsAny<FormSeniorExemptionApplicationDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsSeniorExemptionApplicationDetailForSave { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsSeniorExemptionApplicationDetailForSave>(It.IsAny<string>(), It.IsAny<DynamicsSeniorExemptionApplicationDetailForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new SEApplicationDetailsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSeniorExemptionApplicationDetail value = new FormSeniorExemptionApplicationDetail { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormSeniorExemptionApplicationDetail> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsSeniorExemptionApplicationDetailForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsSeniorExemptionApplicationDetailForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEApplicationDetailsController is succefull.
        /// </summary>
        /// <returns>True.</returns>
        [TestMethod]
        public async Task SEApplicationDetailsController_Delete_ReturnsTrue()
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
            var controller = new SEApplicationDetailsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSeniorExemptionApplicationDetail value = new FormSeniorExemptionApplicationDetail { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormSeniorExemptionApplicationDetail> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on SEApplicationDetailsController is not succefull.
        /// </summary>
        /// <returns>false.</returns>
        [TestMethod]
        public async Task SEApplicationDetailsController_Delete_ReturnsFalse()
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
            var controller = new SEApplicationDetailsController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormSeniorExemptionApplicationDetail value = new FormSeniorExemptionApplicationDetail { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber };

            // Act
            ActionResult<FormSeniorExemptionApplicationDetail> result = await controller.Delete(ValidId);

            // Assert
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}