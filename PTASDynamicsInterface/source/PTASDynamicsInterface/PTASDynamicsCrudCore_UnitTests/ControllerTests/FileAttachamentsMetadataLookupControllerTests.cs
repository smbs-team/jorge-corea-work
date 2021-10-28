using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASDynamicsCrudCore.Controllers;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;

namespace PTASDynamicsCrudCore_UnitTests.ControllerTests
{
    /// <summary>
    /// FileAttachamentsMetadataLookupController class test.
    /// </summary>
    [TestClass]
    public class FileAttachamentsMetadataLookupControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string AccountNumber = "522201212";

        /// <summary>
        /// Test the Get Method when a get in FileAttachamentsMetadataLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task FileAttachamentsMetadataLookupController_Get_ReturnsNull()
        {
            // Arrange
            var moqFileAttachmentMetadataManager = new Mock<IFileAttachmentMetadataManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetFileAttchamentMetadataFromSEAppId = false;

            moqFileAttachmentMetadataManager.Setup(m => m.GetFileAttchamentMetadataFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetFileAttchamentMetadataFromSEAppId = true;
                return new List<DynamicsFileAttachmentMetadata> { };
            });

            var controller = new FileAttachamentsMetadataLookupController(moqFileAttachmentMetadataManager.Object, moqmapper.Object);

            // Act

            ActionResult<FormFileAttachmentMetadata[]> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledGetFileAttchamentMetadataFromSEAppId);
            moqFileAttachmentMetadataManager.Verify(m => m.GetFileAttchamentMetadataFromSEAppId(It.Is<string>(s => s == $"{NonvalidIdForGet}")));

        }

        /// <summary>
        /// Test the Get Method when a get FileAttachamentsMetadataLookupController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task FileAttachamentsMetadataLookupController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqFileAttachmentMetadataManager = new Mock<IFileAttachmentMetadataManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetFileAttchamentMetadataFromSEAppId = false;

            moqFileAttachmentMetadataManager.Setup(m => m.GetFileAttchamentMetadataFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetFileAttchamentMetadataFromSEAppId = true;
                return new List<DynamicsFileAttachmentMetadata> {
                    new DynamicsFileAttachmentMetadata
                    { Id = new System.Guid(), AccountNumber = AccountNumber },
                    new DynamicsFileAttachmentMetadata
                    { Id = new System.Guid(), AccountNumber = AccountNumber }
                };
            });

            moqmapper.Setup<FormFileAttachmentMetadata[]>(m => m.Map<FormFileAttachmentMetadata[]>(It.IsAny<DynamicsFileAttachmentMetadata[]>())).Returns(() =>
            {
                //calledMapper = true;
                var res = new FormFileAttachmentMetadata[]
                {
                        new FormFileAttachmentMetadata { Id = new System.Guid(), AccountNumber = AccountNumber } };
                return res;
            });

            var controller = new FileAttachamentsMetadataLookupController(moqFileAttachmentMetadataManager.Object, moqmapper.Object);

            // Act
            ActionResult<FormFileAttachmentMetadata[]> result = await controller.Get(ValidId);

            // Assert
            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledGetFileAttchamentMetadataFromSEAppId);
            moqFileAttachmentMetadataManager.Verify(m => m.GetFileAttchamentMetadataFromSEAppId(It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}