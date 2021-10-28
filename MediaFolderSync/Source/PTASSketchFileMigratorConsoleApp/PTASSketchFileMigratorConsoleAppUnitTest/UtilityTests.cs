using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PTASExportConnector.SDK;
using PTASketchFileMigratorConsoleApp;
using PTASketchFileMigratorConsoleApp.Constants;
using PTASketchFileMigratorConsoleApp.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTASSketchFileMigratorConsoleAppUnitTest
{
    [TestClass]
    public class UtilityTests
    {
        private readonly Mock<IFileSystem> fileSystem;
        private readonly Mock<IOdata> odata;
        private readonly Utility utility;
        private readonly Mock<IFileInfo> file;
        private List<IFileInfo> fileList;

        public UtilityTests()
        {
            fileSystem = new Mock<IFileSystem>();
            var accAtt = new AccAttributes() { Value = new DrawingMetadata[] { new DrawingMetadata { AttributeValue = 33, DrawingId = "CAN", Type = "ptas_commaccessorytype", Value = "Canopies" } } };
            var fileData = new FileType() { AccessoryDrawingIDs = new string[] { "DG", "PC" }, DrawingIds = new Dictionary<string, string>() { { "B", "ptas_buildingdetail" } } };
            fileSystem.Setup(f => f.File.ReadAllText(@"./accAttributes.json")).Returns(JsonConvert.SerializeObject(accAtt));
            fileSystem.Setup(f => f.File.ReadAllText(@"./fileData.json")).Returns(JsonConvert.SerializeObject(fileData));
            fileSystem.Setup(f => f.File.ReadAllText(@"./Logs/invalidFiles.json")).Throws(new FileNotFoundException());
            fileSystem.Setup(f => f.File.ReadAllText(@"./Logs/officialFiles.json")).Throws(new FileNotFoundException());
            odata = new Mock<IOdata>();
            utility = new Utility(fileSystem.Object, odata.Object);
            file = new Mock<IFileInfo>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            fileSystem.Setup(f => f.Path.GetFileNameWithoutExtension(It.IsAny<string>())).Returns("123456-1234C");
            fileSystem.Setup(f => f.Directory.Exists(It.IsAny<string>())).Returns(true);
            fileSystem.Setup(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            file.Setup(f => f.Name).Returns("123456-1234C.xml");
            fileList = new List<IFileInfo>() { file.Object };
        }

        [TestMethod]
        public async Task TestRename_FileArrayIsNull()
        {
            await utility.RenameFiles(null);
            fileSystem.Verify(f => f.Path.GetFileNameWithoutExtension(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task TestRename_FileEntityNameIsNotValid()
        {
            await utility.RenameFiles(fileList);
            odata.Verify(o => o.Get(EntityNames.Sketch, It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task TestRename_FileIsOfficialShouldContinue()
        {
            fileSystem.Setup(f => f.Path.GetFileNameWithoutExtension(It.IsAny<string>())).Returns("123456-1234b");
            file.Setup(f => f.Name).Returns("123456-1234b.xml");
            utility.OfficialFiles = new List<OfficialFile>() { new OfficialFile { FileName = "123456-1234b", Identifier = "b" } };
            await utility.RenameFiles(fileList);
            odata.Verify(o => o.Get(EntityNames.Sketch, It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task TestRename_BuildingSketchNotFoundShouldContinue()
        {
            fileSystem.Setup(f => f.Path.GetFileNameWithoutExtension(It.IsAny<string>())).Returns("123456-1234b");
            file.Setup(f => f.Name).Returns("123456-1234b.xml");
            odata.Setup(o => o.Get(EntityNames.Sketch, It.IsAny<string>())).ReturnsAsync(new OdataResponse { Value = null });

            await utility.RenameFiles(fileList);

            fileSystem.Verify(f => f.Path.GetFileNameWithoutExtension(It.IsAny<string>()), Times.Exactly(1));
            odata.Verify(o => o.Get(EntityNames.Sketch, It.IsAny<string>()), Times.Once);
            fileSystem.Verify(f => f.File.Move(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task TestRename_BuildingSketchFound()
        {
            fileSystem.Setup(f => f.Path.GetFileNameWithoutExtension(It.IsAny<string>())).Returns("123456-1234b");
            file.Setup(f => f.Name).Returns("123456-1234b.xml");
            odata.Setup(o => o.Get(EntityNames.Sketch, It.IsAny<string>())).ReturnsAsync(new OdataResponse { Value = new Sketch[] { new Sketch { SketchId = "1", Version = "4", Building = new Building { Id = "buildingId" } } } });

            await utility.RenameFiles(fileList);

            fileSystem.Verify(f => f.Path.GetFileNameWithoutExtension(It.IsAny<string>()), Times.Exactly(1));
            odata.Verify(o => o.Get(EntityNames.Sketch, It.IsAny<string>()), Times.Once);
            fileSystem.Verify(f => f.File.Move(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TestFilterByValidFiles_ShouldReturnNewestBuilding()
        {
            var file2 = new Mock<IFileInfo>();
            var file3 = new Mock<IFileInfo>();
            var file4 = new Mock<IFileInfo>();
            file2.Setup(f => f.Name).Returns("123456-1234.vcd");
            file3.Setup(f => f.Name).Returns("123456-1234b.vcd");
            file4.Setup(f => f.Name).Returns("123456-1234b1.vcd");
            file2.Setup(f => f.LastWriteTime).Returns(new DateTime(2020, 01, 01));
            file3.Setup(f => f.LastWriteTime).Returns(new DateTime(2020, 02, 01));
            file4.Setup(f => f.LastWriteTime).Returns(new DateTime(2019, 01, 01));
            fileList.AddRange(new List<IFileInfo> {file2.Object, file3.Object, file4.Object });

            var result = utility.FilterByValidFiles(fileList);
            var name = result.FirstOrDefault().Name;

            Assert.AreEqual("123456-1234b.vcd", name);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void TestFilterByValidFiles_ShouldReturnNewestBuildingAndOtherValidBuildings()
        {
            var file2 = new Mock<IFileInfo>();
            var file3 = new Mock<IFileInfo>();
            var file4 = new Mock<IFileInfo>();
            var file5 = new Mock<IFileInfo>();
            file2.Setup(f => f.Name).Returns("123456-1234.vcd");
            file3.Setup(f => f.Name).Returns("123456-1234b.vcd");
            file4.Setup(f => f.Name).Returns("123456-1234b1.vcd");
            file5.Setup(f => f.Name).Returns("123456-1234b2.vcd");
            file2.Setup(f => f.LastWriteTime).Returns(new DateTime(2020, 01, 01));
            file3.Setup(f => f.LastWriteTime).Returns(new DateTime(2020, 02, 01));
            file4.Setup(f => f.LastWriteTime).Returns(new DateTime(2019, 01, 01));
            file5.Setup(f => f.LastWriteTime).Returns(new DateTime(2000, 01, 01));
            fileList.AddRange(new List<IFileInfo> { file2.Object, file3.Object, file4.Object, file5.Object });

            var result = utility.FilterByValidFiles(fileList);
            var otherValidBuildingName = result.FirstOrDefault().Name;
            var newestBuildingName = result.Skip(1).FirstOrDefault().Name;

            Assert.AreEqual("123456-1234b2.vcd", otherValidBuildingName);
            Assert.AreEqual("123456-1234b.vcd", newestBuildingName);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void TestFilterByValidFiles_ShouldReturnNewestBuildingAndOtherValidBuildingsAndOtherValidFiles()
        {
            var file2 = new Mock<IFileInfo>();
            var file3 = new Mock<IFileInfo>();
            var file4 = new Mock<IFileInfo>();
            var file5 = new Mock<IFileInfo>();
            var file6 = new Mock<IFileInfo>();
            var file7 = new Mock<IFileInfo>();
            file2.Setup(f => f.Name).Returns("123456-1234.vcd");
            file3.Setup(f => f.Name).Returns("123456-1234b.vcd");
            file4.Setup(f => f.Name).Returns("123456-1234b1.vcd");
            file5.Setup(f => f.Name).Returns("123456-1234b2.vcd");
            file6.Setup(f => f.Name).Returns("532624-4134pc1.vcd");
            file7.Setup(f => f.Name).Returns("642415-1567pc.vcd");
            file2.Setup(f => f.LastWriteTime).Returns(new DateTime(2020, 01, 01));
            file3.Setup(f => f.LastWriteTime).Returns(new DateTime(2020, 02, 01));
            file4.Setup(f => f.LastWriteTime).Returns(new DateTime(2019, 01, 01));
            file5.Setup(f => f.LastWriteTime).Returns(new DateTime(2000, 01, 01));
            fileList.AddRange(new List<IFileInfo> { file2.Object, file3.Object, file4.Object, file5.Object, file6.Object, file7.Object });

            var result = utility.FilterByValidFiles(fileList);

            // newest building
            Assert.AreEqual("123456-1234b.vcd", result.Last().Name);

            // other valid building
            Assert.AreEqual("123456-1234b2.vcd", result.First().Name);

            // other valid file
            Assert.AreEqual("532624-4134pc1.vcd", result.Skip(1).First().Name);

            // other valid file
            Assert.AreEqual("642415-1567pc.vcd", result.Skip(2).First().Name);

            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public void TestFilterByValidFiles_ShouldFilterInvalidFiles()
        {
            var file2 = new Mock<IFileInfo>();
            var file3 = new Mock<IFileInfo>();
            var file4 = new Mock<IFileInfo>();
            var file5 = new Mock<IFileInfo>();
            var file6 = new Mock<IFileInfo>();
            file2.Setup(f => f.Name).Returns("123456-1234.vcd");
            file3.Setup(f => f.Name).Returns("123456-1234Z.vcd");
            file4.Setup(f => f.Name).Returns("123456525-1234.vcd");
            file5.Setup(f => f.Name).Returns("123456-12345.vcd");
            file6.Setup(f => f.Name).Returns("123456-12345$b1.vcd");
            fileList.AddRange(new List<IFileInfo> { file2.Object, file3.Object, file4.Object, file5.Object, file6.Object });

            var result = utility.FilterByValidFiles(fileList);
            var name = result.FirstOrDefault().Name;

            Assert.AreEqual("123456-1234.vcd", name);
            Assert.AreEqual(1, result.Count());
        }
    }
}
