using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASDynamicsCrudCore.Controllers;
using PTASDynamicsCrudHelperClasses.Classes;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;
using Microsoft.AspNetCore.Mvc;
using PTASDynamicsCrudCore_UnitTests.HelperClasses;
using System.Net;
using PTASCRMHelpers;

namespace PTASDynamicsCrudCore_UnitTests.ControllerTests
{
    /// <summary>
    /// FileAttachmentsMetatadaController class test.
    /// </summary>
    [TestClass]
    public class FileAttachmentsMetadataControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string TableName = "ptas_fileattachmentmetadatas";
        private const string KeyField = "ptas_fileattachmentmetadataid";
        private const string AccountNumber = "522201212";

        /// <summary>
        /// Test the Get Method when a get in FileAttachmentsMetadataController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task FileAttachementMetadataController_Get_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsFileAttachmentMetadata>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsFileAttachmentMetadata[] { };
            });
            var controller = new FileAttachmentsMetadataController(moqwrapper.Object, moqmapper.Object);

            // Act
            ActionResult<FormFileAttachmentMetadata> result = await controller.Get(NonvalidIdForGet);

            // Assert
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsFileAttachmentMetadata>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{NonvalidIdForGet}'")));
        }

        /// <summary>
        /// Test the Get Method when a get FileAttachmentsMetadataController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task FileAttachementMetadataController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<FormFileAttachmentMetadata>(m => m.Map<FormFileAttachmentMetadata>(It.IsAny<DynamicsFileAttachmentMetadata>())).Returns(() =>
            {
                calledMapper = true;
                return new FormFileAttachmentMetadata { Id = new System.Guid(), AccountNumber = AccountNumber };
            });

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsFileAttachmentMetadata>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsFileAttachmentMetadata[] {
                    new DynamicsFileAttachmentMetadata {Id=new System.Guid(), AccountNumber=AccountNumber } };
            });
            var controller = new FileAttachmentsMetadataController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormFileAttachmentMetadata> result = await controller.Get(ValidId);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsFileAttachmentMetadata>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"$filter={KeyField} eq '{ValidId}'")));
        }

        /// <summary>
        /// Test the Post Method when a post in  FileAttachmentsMetadataController is succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task FileAttachementMetadataController_Post_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsFileAttachmentMetadataForSave>(m => m.Map<DynamicsFileAttachmentMetadataForSave>(It.IsAny<FormFileAttachmentMetadata>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsFileAttachmentMetadataForSave { Id = new System.Guid(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsFileAttachmentMetadataForSave>(It.IsAny<string>(), It.IsAny<DynamicsFileAttachmentMetadataForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new FileAttachmentsMetadataController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormFileAttachmentMetadata value = new FormFileAttachmentMetadata { Id = new System.Guid(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormFileAttachmentMetadata> result = await controller.Post(value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsFileAttachmentMetadataForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsFileAttachmentMetadataForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Post Method when a post in  FileAttachmentsMetadataController is not succefull.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task FileAttachementMetadataController_Post_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsFileAttachmentMetadataForSave>(m => m.Map<DynamicsFileAttachmentMetadataForSave>(It.IsAny<FormFileAttachmentMetadata>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsFileAttachmentMetadataForSave { Id = new System.Guid(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePost<DynamicsFileAttachmentMetadataForSave>(It.IsAny<string>(), It.IsAny<DynamicsFileAttachmentMetadataForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });
            var controller = new FileAttachmentsMetadataController(moqwrapper.Object, moqmapper.Object);

            // the object to be i
            FormFileAttachmentMetadata value = new FormFileAttachmentMetadata { Id = new System.Guid(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormFileAttachmentMetadata> result = await controller.Post(value);

            // Assert

            Assert.IsNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePost<DynamicsFileAttachmentMetadataForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsFileAttachmentMetadataForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}")));
        }

        /// <summary>
        /// Test the Path Method when an update on FileAttachmentsMetadataController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task FileAttachementMetadataController_Patch_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsFileAttachmentMetadataForSave>(m => m.Map<DynamicsFileAttachmentMetadataForSave>(It.IsAny<FormFileAttachmentMetadata>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsFileAttachmentMetadataForSave { Id = new System.Guid(), AccountNumber = AccountNumber };
            });
            moqwrapper.Setup(m => m.ExecutePatch<DynamicsFileAttachmentMetadataForSave>(It.IsAny<string>(), It.IsAny<DynamicsFileAttachmentMetadataForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });
            var controller = new FileAttachmentsMetadataController(moqwrapper.Object, moqmapper.Object);

            // the object to be updated
            FormFileAttachmentMetadata value = new FormFileAttachmentMetadata { Id = new System.Guid(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormFileAttachmentMetadata> result = await controller.Patch(ValidId, value);

            // Assert

            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsFileAttachmentMetadataForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsFileAttachmentMetadataForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Path Method when an update on FileAttachmentsMetadataController is not succefull
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task FileAttachementMetadataController_Patch_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            moqmapper.Setup<DynamicsFileAttachmentMetadataForSave>(m => m.Map<DynamicsFileAttachmentMetadataForSave>(It.IsAny<FormFileAttachmentMetadata>())).Returns(() =>
            {
                calledMapper = true;
                return new DynamicsFileAttachmentMetadataForSave { Id = new System.Guid(), AccountNumber = AccountNumber };
            });

            moqwrapper.Setup(m => m.ExecutePatch<DynamicsFileAttachmentMetadataForSave>(It.IsAny<string>(), It.IsAny<DynamicsFileAttachmentMetadataForSave>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return false;
            });

            var controller = new FileAttachmentsMetadataController(moqwrapper.Object, moqmapper.Object);

            // the object to be updated
            FormFileAttachmentMetadata value = new FormFileAttachmentMetadata { Id = new System.Guid(), AccountNumber = AccountNumber };

            // Act
            ActionResult<FormFileAttachmentMetadata> result = await controller.Patch(ValidId, value);

            // Assert
            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecutePatch<DynamicsFileAttachmentMetadataForSave>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<DynamicsFileAttachmentMetadataForSave>(s => s.AccountNumber == AccountNumber),
                It.Is<string>(s => s == $"{KeyField}={ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on FileAttachmentsMetadataController is succefull.
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
            var controller = new FileAttachmentsMetadataController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormFileAttachmentMetadata value = new FormFileAttachmentMetadata { Id = new System.Guid(), AccountNumber = AccountNumber };

            // Act

            ActionResult<FormFileAttachmentMetadata> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }

        /// <summary>
        /// Test the Delete Method when a delete on FileAttachmentsMetadataController is not succefull.
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
            var controller = new FileAttachmentsMetadataController(moqwrapper.Object, moqmapper.Object);

            // the object to be deleted
            FormFileAttachmentMetadata value = new FormFileAttachmentMetadata { Id = new System.Guid(), AccountNumber = AccountNumber };

            // Act
            ActionResult<FormFileAttachmentMetadata> result = await controller.Delete(ValidId);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteDelete(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}