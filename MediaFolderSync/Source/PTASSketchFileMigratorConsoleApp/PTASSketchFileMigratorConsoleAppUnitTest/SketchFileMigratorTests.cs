using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASketchFileMigratorConsoleApp;
using PTASketchFileMigratorConsoleApp.Types;
using PTASSketchFileMigratorConsoleApp;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace PTASSketchFileMigratorConsoleAppUnitTest
{
    [TestClass]
    public class SketchFileMigratorTests
    {
        private readonly Mock<IBlobUploader> blobUploader;
        private readonly Mock<ISettingsManager> settings;
        private readonly Mock<IDirectoryManager> directory;
        private readonly Mock<IVCADDManager> vcadd;
        private readonly SketchFileMigrator sketchFileMigrator;
        private readonly Mock<IFileInfo> file;
        private readonly Mock<IUtility> utility;

        public SketchFileMigratorTests()
        {
            settings = new Mock<ISettingsManager>();
            blobUploader = new Mock<IBlobUploader>();
            directory = new Mock<IDirectoryManager>();
            vcadd = new Mock<IVCADDManager>();
            file = new Mock<IFileInfo>();
            utility = new Mock<IUtility>();

            sketchFileMigrator = new SketchFileMigrator(blobUploader.Object, settings.Object, vcadd.Object, directory.Object, utility.Object);

        }

        [TestInitialize]
        public void TestInitialize()
        {
            settings.Setup(s => s.ReadSetting("InputFolder")).Returns(@".\vcd\");
            settings.Setup(s => s.ReadSetting("OutputFolder")).Returns(@".\xml\");
            settings.Setup(s => s.ReadSetting("isOfficialToken")).Returns("officialToken");
            settings.Setup(s => s.ReadConfig("UploadMostRecentFileDate")).Returns("12/01/1900 12:00:00 AM");
            settings.Setup(s => s.ReadConfig("ExportMostRecentFileDate")).Returns("12/01/1900 12:00:00 AM");
            file.Setup(f => f.Name).Returns("failedExporting.json");
        }

        [TestMethod]
        public void TestRun_ExportingFlag()
        {
            IOrderedEnumerable<IFileInfo> list = new List<IFileInfo>() { file.Object }.OrderBy(x => x.CreationTime);
            settings.Setup(s => s.ReadConfig("Flag")).Returns("Exporting");
            directory.Setup(d => d.GetFolderFilesByDate(It.IsAny<string>(), "*.vcd", DateTime.Parse("12/1/1900 12:00:00 AM"))).Returns(list);
            directory.Setup(d => d.GetFolderFilesByDate(@".\", "failedExporting.json", DateTime.Parse("12/1/1900 12:00:00 AM"))).Returns(list);

            sketchFileMigrator.Run();

            vcadd.Verify(v => v.ExportXML(It.IsAny<IEnumerable<IFileInfo>>(), @".\vcd\", @".\xml\", DateTime.Parse("12/01/1900 12:00:00 AM"), true), Times.Once);
            blobUploader.Verify(b => b.UploadSingleFile(file.Object, It.IsAny<string>()), Times.Once);
            directory.Verify(d => d.GetFolderFilesByDate(@".\vcd\", "*.vcd", It.IsAny<DateTime>()), Times.Once);
            directory.Verify(d => d.GetFolderFilesByDate(It.IsAny<string>(), "failedExporting.json", It.IsAny<DateTime>()), Times.Once);
            utility.Verify(d => d.FilterByValidFiles(It.IsAny<IEnumerable<IFileInfo>>()), Times.Once);
        }

        [TestMethod]
        public void TestRun_UploadingFlag()
        {
            var officialFiles = new List<OfficialFile>() { new OfficialFile { FileName = "name", Identifier = "id", SketchName = "name" } };

            settings.Setup(s => s.ReadConfig("Flag")).Returns("Uploading");
            utility.Setup(u => u.OfficialFiles).Returns(officialFiles);

            sketchFileMigrator.Run();
            
            directory.Verify(d => d.GetFolderFilesByDate(@".\xml\", "*.xml", DateTime.Parse("12/01/1900 12:00:00 AM")), Times.Once);
            utility.Verify(u => u.RenameFiles(It.IsAny<IEnumerable<IFileInfo>>()), Times.Once);
            blobUploader.Verify(b => b.UploadXML(It.IsAny<IEnumerable<IFileInfo>>(), officialFiles), Times.Once);
        }

        [TestMethod]
        public void TestRun_DeleteFlag()
        {
            settings.Setup(s => s.ReadConfig("Flag")).Returns("Delete");

            sketchFileMigrator.Run();

            directory.Verify(d => d.DeleteFiles(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TestRun_BeforeFlag()
        {
            settings.Setup(s => s.ReadConfig("Flag")).Returns("flag");

            sketchFileMigrator.Run();

            settings.Verify(s => s.ReadConfig("Flag"), Times.Once);
            settings.Verify(s => s.ReadSetting("InputFolder"), Times.Once);
            settings.Verify(s => s.ReadSetting("OutputFolder"), Times.Once);
            settings.Verify(s => s.ReadSetting("isOfficialToken"), Times.Once);
            settings.Verify(s => s.ReadConfig("UploadMostRecentFileDate"), Times.Once);
            settings.Verify(s => s.ReadConfig("ExportMostRecentFileDate"), Times.Once);
        }
    }
}
