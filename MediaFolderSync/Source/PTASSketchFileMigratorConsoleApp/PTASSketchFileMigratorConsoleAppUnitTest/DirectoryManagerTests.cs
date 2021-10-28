using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASSketchFileMigratorConsoleApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace PTASSketchFileMigratorConsoleAppUnitTest
{
    [TestClass]
    public class DirectoryManagerTests
    {
        private readonly Mock<ISettingsManager> settings;
        private readonly Mock<IFileSystem> fileSystem;
        private readonly Mock<IFileInfo> file;
        private readonly DirectoryManager directory;
        public DirectoryManagerTests()
        {
            settings = new Mock<ISettingsManager>();
            fileSystem = new Mock<IFileSystem>();
            file = new Mock<IFileInfo>();
            directory = new DirectoryManager(settings.Object, fileSystem.Object);
        }

        [TestMethod]
        public void TestGetFolderFilesByDate_FileCountIsCeroShouldReturnNull()
        {
            fileSystem.Setup(fs => fs.Directory.CreateDirectory(It.IsAny<string>())).Returns(It.IsAny<IDirectoryInfo>());
            fileSystem.Setup(fs => fs.DirectoryInfo.FromDirectoryName("folder").GetFiles("*.xml", SearchOption.AllDirectories)).Returns(new IFileInfo[] { });

            var result = directory.GetFolderFilesByDate("folder", "*.xml", DateTime.Parse("12/01/1900 12:00:00 AM"));
            fileSystem.Verify(fs => fs.Directory.CreateDirectory("folder"));

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetFolderFilesByDate_FileCountIsMoreThanCeroShouldReturnFiles()
        {
            file.Setup(f => f.Name).Returns("file.xml");
            file.Setup(f => f.CreationTime).Returns(DateTime.Parse("12/01/1901 12:00:00 AM"));
            fileSystem.Setup(fs => fs.Directory.CreateDirectory(It.IsAny<string>())).Returns(It.IsAny<IDirectoryInfo>());
            fileSystem.Setup(fs => fs.DirectoryInfo.FromDirectoryName("folder").GetFiles("*.xml", SearchOption.AllDirectories)).Returns(new IFileInfo[] { file.Object });

            var result = directory.GetFolderFilesByDate("folder", "*.xml", DateTime.Parse("12/01/1900 12:00:00 AM"));
            fileSystem.Verify(fs => fs.Directory.CreateDirectory("folder"));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void TestGetFolderFilesByDate_CatchThrownException()
        {
            fileSystem.Setup(fs => fs.Directory.CreateDirectory(It.IsAny<string>())).Returns(It.IsAny<IDirectoryInfo>());
            fileSystem.Setup(fs => fs.DirectoryInfo.FromDirectoryName("folder").GetFiles("*.xml", SearchOption.AllDirectories)).Throws(new IOException());

            var result = directory.GetFolderFilesByDate("folder", "*.xml", DateTime.Parse("12/01/1900 12:00:00 AM"));

            Assert.ThrowsException<IOException>(() => directory.GetFolderFilesByDate("folder", "*.xml", DateTime.Parse("12/01/1900 12:00:00 AM")));
        }

        [TestMethod]
        public void TestDeleteFiles_WhenFolderIsTheSameAsInputFolderShouldReturn()
        {
            settings.Setup(s => s.ReadSetting("InputFolder")).Returns("folder");

            directory.DeleteFiles("folder");

            fileSystem.Verify(fs => fs.Directory.EnumerateFiles("folder", "*", It.IsAny<SearchOption>()), Times.Never);
        }

        [TestMethod]
        public void TestDeleteFiles_WhenCalledDeleteDirectory()
        {
            settings.Setup(s => s.ReadSetting("InputFolder")).Returns("folder2");
            IEnumerable<string> list = new List<string>() { "one" };
            fileSystem.Setup(fs => fs.Directory.EnumerateFiles("folder")).Returns(list);
            fileSystem.Setup(fs => fs.Directory.EnumerateDirectories("folder")).Returns(list);

            directory.DeleteFiles("folder");

            fileSystem.Verify(fs => fs.Directory.Delete("one"), Times.Once);
            settings.Verify(s => s.UpdateAppConfig("Flag", "Exporting"), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDeleteFiles_ShouldCatchException()
        {
            settings.Setup(s => s.ReadSetting("InputFolder")).Returns("folder2");
            IEnumerable<string> list = new List<string>() { "one" };
            fileSystem.Setup(fs => fs.Directory.EnumerateFiles("folder", "*", SearchOption.AllDirectories)).Throws(new ArgumentNullException());

            directory.DeleteFiles("folder");

            Assert.ThrowsException<ArgumentNullException>(() => directory.DeleteFiles("folder"));
        }
    }
}
