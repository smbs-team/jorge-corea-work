
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASSketchFileMigratorConsoleApp;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading;

namespace PTASSketchFileMigratorConsoleAppUnitTest
{
    [TestClass]
    public class VCADDManagerTests
    {
        private readonly Mock<ISettingsManager> settings;
        private readonly Mock<IVCADDEngine> engine;
        private readonly Mock<IFileSystem> fileSystem;
        private readonly Mock<IFileInfo> file;
        private readonly VCADDManager vcaddManager;
        private IEnumerable<IFileInfo> list;

        public VCADDManagerTests()
        {
            settings = new Mock<ISettingsManager>();
            engine = new Mock<IVCADDEngine>();
            fileSystem = new Mock<IFileSystem>();
            file = new Mock<IFileInfo>();
            vcaddManager = new VCADDManager(settings.Object, engine.Object, fileSystem.Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            file.Setup(f => f.DirectoryName).Returns(@"Z:\a\b");
            file.Setup(f => f.Name).Returns("file.xml");
            file.Setup(f => f.FullName).Returns(@"Z:\a\file.xml");
            file.Setup(f => f.CreationTime).Returns(DateTime.Parse("12/01/1901 12:00:00 AM"));
            fileSystem.Setup(fs => fs.Directory.CreateDirectory(It.IsAny<string>())).Returns(It.IsAny<IDirectoryInfo>());
            list = new List<IFileInfo>() { file.Object };
        }

        [TestMethod]
        public void TestExportXML_FileArrayIsNullShouldReturn()
        {
            vcaddManager.ExportXML(null, "a", "b", DateTime.Now);
            engine.Verify(e => e.InitVCADD(), Times.Never);
        }

        [TestMethod]
        public void TestExportXML_ExportXMLIsCalled()
        {
            vcaddManager.ExportXML(list, @".\input\", @".\output\", DateTime.Parse("12/01/1900 12:00:00 AM"), false);

            engine.Verify(e => e.InitVCADD(), Times.Once);
            engine.Verify(e => e.LoadVCDFromFile(@"Z:\a\file.xml"), Times.Once);
            engine.Verify(e => e.ExportXML(@".\output\\b\", "file.xml"), Times.Once);
            engine.Verify(e => e.ClearDrawing(), Times.Once);
            settings.Verify(s => s.UpdateAppConfig("ExportMostRecentFileDate", It.IsAny<string>()), Times.Once);
            engine.Verify(e => e.TerminateVCADD(), Times.Once);
        }

        [TestMethod]
        public void TestExportXML_folderNameIsDifferentFromInputFolderNameShouldCreateDirectoryAndAssignNewPath()
        {
            vcaddManager.ExportXML(list, @".\input\", @".\output\", DateTime.Parse("12/01/1900 12:00:00 AM"), false);

            engine.Verify(e => e.ExportXML(@".\output\\b\", "file.xml"), Times.Once);
            fileSystem.Verify(fs => fs.Directory.CreateDirectory(@".\output\\b"), Times.Once);
        }

        [TestMethod]
        public void TestExportXML_FileDateIsNewerThanSuppliedDateShouldAssingNewDate()
        {
            vcaddManager.ExportXML(list, @".\input\", @".\output\", DateTime.Parse("12/01/1900 12:00:00 AM"));

            settings.Verify(s => s.UpdateAppConfig("ExportMostRecentFileDate", "12 01 1901 12:00:00.0000000 AM"), Times.Once);
        }

        [TestMethod]
        public void TestExportXML_LoadingVCDFileTakesMoreThanTwoSecondsShouldAddToErrorList()
        {
            fileSystem.Setup(fs => fs.File.AppendAllText(It.IsAny<string>(), It.IsAny<string>()));
            engine.Setup(e => e.LoadVCDFromFile(It.IsAny<string>()))
                        .Callback(() => Thread.Sleep(2500));

            vcaddManager.ExportXML(list, @".\input\", @".\output\", DateTime.Parse("12/01/1900 12:00:00 AM"));

            Assert.AreEqual(1, vcaddManager.ErrorList.Count);
        }


        [TestMethod]
        public void TestExportXML_UpdateMostRecentExportDateWhen1000FilesHaveBeenProcessed()
        {
            vcaddManager.Counter = 999;

            vcaddManager.ExportXML(list, @".\input\", @".\output\", DateTime.Parse("12/01/1900 12:00:00 AM"));

            settings.Verify(s => s.UpdateAppConfig("ExportMostRecentFileDate", It.IsAny<string>()), Times.Exactly(2));
            Assert.AreEqual(0, vcaddManager.Counter);
        }

        [TestMethod]
        public void TestExportXML_CreateJsonErrorListFileWhenCounterIs1000AndThereIsErrors()
        {
            vcaddManager.Counter = 999;
            vcaddManager.ErrorList.Add(1);
            fileSystem.Setup(fs => fs.File.AppendAllText(It.IsAny<string>(), It.IsAny<string>()));

            vcaddManager.ExportXML(list, @".\input\", @".\output\", DateTime.Parse("12/01/1900 12:00:00 AM"));

            fileSystem.Verify(f => f.File.AppendAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));           
            Assert.AreEqual(0, vcaddManager.ErrorList.Count);
        }

        [TestMethod]
        public void TestExportXML_CreateJsonErrorListFileAfterLoopIfThereIsErrors()
        {
            vcaddManager.ErrorList.Add(1);
            fileSystem.Setup(fs => fs.File.AppendAllText(It.IsAny<string>(), It.IsAny<string>()));

            vcaddManager.ExportXML(list, @".\input\", @".\output\", DateTime.Parse("12/01/1900 12:00:00 AM"));

            fileSystem.Verify(f => f.File.AppendAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestExportXML_SaveDateWhenErrorIsThrown()
        {
            fileSystem.Setup(fs => fs.Directory.CreateDirectory(It.IsAny<string>())).Throws(new Exception());

            vcaddManager.ExportXML(list, @".\input\", @".\output\", DateTime.Parse("12/01/1900 12:00:00 AM"));

            settings.Verify(s => s.UpdateAppConfig("ExportMostRecentFileDate", It.IsAny<string>()), Times.Once);
            Assert.ThrowsException<Exception>(() => vcaddManager.ExportXML(list, @".\input\", @".\output\", DateTime.Parse("12/01/1900 12:00:00 AM")));
        }
    }
}
