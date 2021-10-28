using System.Threading.Tasks;

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
    /// FileAttachmentsMetatadaController class test.
    /// </summary>
    [TestClass]
    public class BlobToIlinxFinalizerControllerTests
    {
        private const string TableName = "ptas_fileattachmentmetadatas";
        private const string ErrorResultVal = "Error";
        private const string ErrorMessageNoElementsFound = "No elements found during Blob To Ilinx Finalizer.";
        private const string OKResultVal = "Ok";
        private const string SomeGuid = "00000000-DAC1-4AC2-967C-725AAD35BC04";

        /// <summary>
        /// Test the Path Method when an update on BlobToIlinxFinalizerController is not succefull
        /// because the FileAttachementMetadata are not found.
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task BlobToIlinxFinalizerControllerTests_Patch_NoElementsFound()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteGet<SEApplicationIdAndOriginalFileName>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new SEApplicationIdAndOriginalFileName[] { };
            });

            var controller = new BlobToIlinxFinalizerController(moqwrapper.Object);

            BlobToIlinxFinalizerParams info = new BlobToIlinxFinalizerParams
            {
                SEApplicationid = System.Guid.Parse(SomeGuid),
                SEAppDetailId = new System.Guid(),
                AssignedDocumentId = new System.Guid(),
                Section = "The section",
                Document = "The Document"
            };

            string value = info.SEAppDetailId.Equals(default) ? "null" : info.SEAppDetailId.ToString();
            string query = $"$filter=(ptas_seniorexemptionapplication/ptas_seapplicationid eq {info.SEApplicationid}) and (ptas_seniorexemptionapplicationdetail/ptas_seappdetailid eq {value}) and (ptas_isblob eq true) and ((ptas_isilinx eq false) or (ptas_issharepoint eq false)) and (ptas_portaldocument eq '{info.Document}') and (ptas_portalsection eq '{info.Section}')";

            // Act

            BlobToIlinxFinalizerOperationResult theResult = await controller.Patch(info);

            // Assert

            Assert.IsTrue(theResult.Result == ErrorResultVal && theResult.Message == ErrorMessageNoElementsFound && theResult.Count == 0);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteGet<SEApplicationIdAndOriginalFileName>(It.Is<string>(s => s == $"{TableName}"),
                It.Is<string>(s => s == query)));
        }

        /// <summary>
        /// Test the Path Method when an update on BlobToIlinxFinalizerController is succefull
        /// </summary>
        /// <returns>Not Null.</returns>
        [TestMethod]
        public async Task BlobToIlinxFinalizerControllerTests_Patch_ElementsFound()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var calledExecuteGet = false;
            var calledExecutePatch = false;

            moqwrapper.Setup(m => m.ExecuteGet<SEApplicationIdAndOriginalFileName>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledExecuteGet = true;
                return new SEApplicationIdAndOriginalFileName[]
                {
                    new SEApplicationIdAndOriginalFileName
                    {
                        Id = new System.Guid()
                    }
                };
            });

            moqwrapper.Setup(m => m.ExecutePatch(It.IsAny<string>(), It.IsAny<SEApplicationUpdateFields>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledExecutePatch = true;
                return true;
            });

            var controller = new BlobToIlinxFinalizerController(moqwrapper.Object);

            BlobToIlinxFinalizerParams info = new BlobToIlinxFinalizerParams
            {
                SEApplicationid = new System.Guid(),
                SEAppDetailId = new System.Guid(),
                AssignedDocumentId = new System.Guid()
            };

            string value = info.SEAppDetailId.Equals(default) ? "null" : info.SEAppDetailId.ToString();

            SEApplicationIdAndOriginalFileName newItem = new SEApplicationIdAndOriginalFileName
            {
                Id = new System.Guid()
            };

            string query = $"ptas_fileattachmentmetadataid={newItem.Id}";

            // Act

            BlobToIlinxFinalizerOperationResult theResult = await controller.Patch(info);

            // Assert

            Assert.IsTrue(theResult.Result == OKResultVal && theResult.Message == string.Empty && theResult.Count > 0);
            Assert.IsTrue(calledExecuteGet && calledExecutePatch);
            moqwrapper.Verify(m => m.ExecutePatch(It.Is<string>(s => s == $"{TableName}"),
                It.Is<SEApplicationUpdateFields>(s => s.DocumentId == new System.Guid()),
                It.Is<string>(s => s == query)));
        }
    }
}