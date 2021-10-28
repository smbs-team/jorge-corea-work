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
    public class ParcelDetailControllerTests
    {
        private const string TableName = "ptas_parceldetails";
        private const string GetOpQuery = "$top=1000";
        private const string Name = "John Doe";
        private const string KeyField = "ptas_parceldetailid";
        private const string ValidId = "3E0000B6-DAC1-4AC2-967C-725AAD35BC04";
        private const string NonValidId = "00000000-DAC1-4AC2-967C-725AAD35BC04";

        /// <summary>
        /// Test the Get Method when a get ParcelDetailController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task ParcelDetailController_Get_ReturnsVoid()
        {
            // Arrange
            var calledCRMWrapper = false;
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsParcelDetail>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsParcelDetail[] { };
            });

            var controller = new ParcelDetailController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormParcelDetail> result = await controller.Get(NonValidId);

            // Assert`

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsParcelDetail>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{NonValidId}'")));
        }

        /// <summary>
        /// Test the Get Method when a get ParcelDetailController is succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>

        [TestMethod]
        public async Task ParcelDetailController_Get_ReturnsNotNull()
        {
            // Arrange
            var calledCRMWrapper = false;
            var calledMapper = false;
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsParcelDetail>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsParcelDetail[] { new DynamicsParcelDetail { ParcelDetailId = new System.Guid().ToString() } };
            });
            moqmapper.Setup(m => m.Map<FormParcelDetail>(It.IsAny<DynamicsParcelDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new FormParcelDetail { ParcelDetailId = new System.Guid().ToString() };
            });

            var controller = new ParcelDetailController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormParcelDetail> result = await controller.Get(ValidId);

            // Assert`

            Assert.IsTrue(calledCRMWrapper && calledMapper);
            Assert.IsNotNull((result.Result as JsonResult).Value);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsParcelDetail>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{ValidId}'")));
        }

        /// <summary>
        /// Test the Post Method when a post in  ParcelDetailController is not succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task ParcelDetailController_Post_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsParcelDetail>(m => m.Map<DynamicsParcelDetail>(It.IsAny<FormParcelDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsParcelDetail { ParcelDetailId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsParcelDetail>(It.IsAny<string>(), It.IsAny<DynamicsParcelDetail>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new ParcelDetailController(moqwrapper.Object, moqmapper.Object);

            // the object to be inserted
            FormParcelDetail value = new FormParcelDetail { ParcelDetailId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormParcelDetail> result = await controller.Post(value);

            // Assert

            Assert.IsNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsParcelDetail>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsParcelDetail>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Post Method when a post in  ParcelDetailController is succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task ParcelDetailController_Post_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsParcelDetail>(m => m.Map<DynamicsParcelDetail>(It.IsAny<FormParcelDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsParcelDetail { ParcelDetailId = new System.Guid().ToString(), Name = Name };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsParcelDetail>(It.IsAny<string>(), It.IsAny<DynamicsParcelDetail>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new ParcelDetailController(moqwrapper.Object, moqmapper.Object);

            // the object to be inserted
            FormParcelDetail value = new FormParcelDetail { ParcelDetailId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormParcelDetail> result = await controller.Post(value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsParcelDetail>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsParcelDetail>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Path Method when an update on ConctactsController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task ParcelDetailController_Patch_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsParcelDetail>(m => m.Map<DynamicsParcelDetail>(It.IsAny<FormParcelDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsParcelDetail { ParcelDetailId = new System.Guid().ToString(), Name = Name };
            });

            moqwrapper.Setup(m => m.ExecutePatch<DynamicsParcelDetail>(It.IsAny<string>(), It.IsAny<DynamicsParcelDetail>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new ParcelDetailController(moqwrapper.Object, moqmapper.Object);

            // the object to be updated
            FormParcelDetail value = new FormParcelDetail { ParcelDetailId = ValidId, Name = Name };

            // Act

            ActionResult<FormParcelDetail> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsParcelDetail>(It.Is<string>(s => s == $"{TableName}"), It.Is<DynamicsParcelDetail>(s => s.Name == Name),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Path Method when an update on ParcelDetailController is not succefull
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task ParcelDetailController_Patch_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsParcelDetail>(m => m.Map<DynamicsParcelDetail>(It.IsAny<FormParcelDetail>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsParcelDetail { ParcelDetailId = new System.Guid().ToString(), Name = Name };
            });

            moqwrapper.Setup(m => m.ExecutePatch<DynamicsParcelDetail>(It.IsAny<string>(), It.IsAny<DynamicsParcelDetail>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });

            var controller = new ParcelDetailController(moqwrapper.Object, moqmapper.Object);

            // the object to be updated
            FormParcelDetail value = new FormParcelDetail { ParcelDetailId = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormParcelDetail> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            //moqwrapper.Verify(m => m.ExecutePatch<DynamicsParcelDetail>(It.Is<string>(s => s == $"{TableName}"),
            //    It.Is<DynamicsParcelDetail>(s => s.FirstName == FirstName),
            //    It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on ParcelDetailController is succefull.
        /// </summary>
        /// <returns>True.</returns>
        [TestMethod]
        public async Task ParcelDetailController_Delete_ReturnsTrue()
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
            var controller = new ParcelDetailController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormParcelDetail value = new FormParcelDetail { ParcelDetailId = new System.Guid().ToString(), Name = Name };

            // Act

            ActionResult<FormParcelDetail> result = await controller.Delete(ValidId);

            // Assert
            Assert.IsTrue(calledCRMWrapper, "Needed to call CRMWrapper");
            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on ParcelDetailController is not succefull.
        /// </summary>
        /// <returns>false.</returns>
        [TestMethod]
        public async Task ParcelDetailController_Delete_ReturnsFalse()
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
            var controller = new ParcelDetailController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormParcelDetail value = new FormParcelDetail { ParcelDetailId = new System.Guid().ToString(), Name = Name };

            // Act
            ActionResult<FormParcelDetail> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}