using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASSketchFileMigratorConsoleApp;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.IO.Abstractions;
using System.Threading;
using PTASExportConnector.SDK;
using PTASketchFileMigratorConsoleApp.Types;
using PTASketchFileMigratorConsoleApp;

namespace PTASSketchFileMigratorConsoleAppUnitTest
{
    [TestClass]
    public class BlobUploaderTests
    {
        private readonly Mock<ISettingsManager> settings;
        private readonly Mock<ICloudService> cloudService;
        private readonly BlobUploader blobUploader;
        private readonly Mock<IFileInfo> file;
        private readonly Mock<IDataServices> dataServices;
        private IEnumerable<IFileInfo> list;
        private List<OfficialFile> officialFiles;

        public BlobUploaderTests()
        {
            settings = new Mock<ISettingsManager>();
            cloudService = new Mock<ICloudService>();
            file = new Mock<IFileInfo>();
            dataServices = new Mock<IDataServices>();
            blobUploader = new BlobUploader(settings.Object, cloudService.Object, dataServices.Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            settings.Setup(s => s.ReadSetting("BlobContainerName")).Returns("containerName");
            settings.Setup(s => s.ReadSetting("OutputFolder")).Returns(@"xml");
            settings.Setup(s => s.ReadConfig("UploadMostRecentFileDate")).Returns("12/01/1900 12:00:00 AM");
            file.Setup(f => f.CreationTime).Returns(It.IsAny<DateTime>());
            file.Setup(f => f.Name).Returns("file.xml");
            file.Setup(f => f.DirectoryName).Returns(@"\\xml\\folder\\");
            file.Setup(f => f.Length).Returns(500);
            file.Setup(f => f.OpenRead().ReadAsync(It.IsAny<byte[]>(), 0, 100, It.IsAny<CancellationToken>())).Returns(Task.FromResult(100));
            file.Setup(f => f.OpenRead().Read(It.IsAny<byte[]>(), 0, 100)).Returns(100);
            list = new List<IFileInfo>() { file.Object };
            officialFiles = new List<OfficialFile>() { new OfficialFile { FileName = "file", Identifier = "id", SketchName = "file", EntityResult = new EntityResult { EntityName = "name", Id = "id", IsValid = true, SketchId = "file" } } };
        }

        [TestMethod]
        public async Task TestUploadXML_ReturnWhenFileArrayIsNull()
        {

            await blobUploader.UploadXML(null, It.IsAny<List<OfficialFile>>());

            cloudService.Verify(s => s.UploadFile(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task TestUploadXML_ReturnWhenOfficialFileIsNull()
        {
            officialFiles = new List<OfficialFile>() { new OfficialFile { FileName = "file", Identifier = "id", SketchName = "differentName" } };

            await blobUploader.UploadXML(list, officialFiles);

            cloudService.Verify(s => s.UploadFile(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task TestUploadXML_UploadAndIsOfficialAreCalled()
        {

            await blobUploader.UploadXML(list, officialFiles);

            cloudService.Verify(s => s.UploadFile(It.IsAny<byte[]>(), "containerName", @"folder\\file.xml"), Times.Once);
            dataServices.Verify(o => o.SetIsOfficial(It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public async Task TestUploadXML_FolderNameSameAsOutPutFolderNameShouldSetTheCorrectPath()
        {
            file.Setup(f => f.DirectoryName).Returns(@"xml");

            await blobUploader.UploadXML(list, officialFiles);

            cloudService.Verify(s => s.UploadFile(It.IsAny<byte[]>(), "containerName", @"file.xml"), Times.Once);
        }

        [TestMethod]
        public async Task TestUploadXML_FileIsNewerThanCurrentDate()
        {
            file.Setup(f => f.CreationTime).Returns(DateTime.Parse("12/01/1901 12:00:00 AM"));

            await blobUploader.UploadXML(list, officialFiles);

            settings.Verify(s => s.UpdateAppConfig("UploadMostRecentFileDate", "12 01 1901 12:00:00.0000000 AM"), Times.Once);
        }

        [TestMethod]
        public async Task TestUploadXML_CounterIs1000ShouldRecordRecentDate()
        {
            blobUploader.Counter = 999;

            await blobUploader.UploadXML(list, officialFiles);

            settings.Verify(s => s.UpdateAppConfig("UploadMostRecentFileDate", It.IsAny<string>()), Times.Exactly(2));
            Assert.AreEqual(0, blobUploader.Counter);
        }

        [TestMethod]
        public void TestUploadSingleFile_FileIsNull()
        {

            blobUploader.UploadSingleFile(null, It.IsAny<string>());

            cloudService.Verify(s => s.UploadFile(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void TestUploadSingleFile_UploadAFile()
        {
            blobUploader.UploadSingleFile(file.Object, It.IsAny<string>());

            settings.Verify(s => s.ReadSetting(It.IsAny<string>()), Times.Once);
            cloudService.Verify(c => c.UploadFile(It.IsAny<byte[]>(), "containerName", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task TestUploadXML_ThrowsException()
        {
            cloudService.Setup(c => c.UploadFile(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            await blobUploader.UploadXML(list, officialFiles);

            settings.Verify(s => s.UpdateAppConfig("UploadMostRecentFileDate", It.IsAny<string>()), Times.Once);
            Assert.ThrowsException<Exception>(() => blobUploader.UploadXML(It.IsAny<IEnumerable<IFileInfo>>(), It.IsAny<List<OfficialFile>>()));
        }
    }
}
