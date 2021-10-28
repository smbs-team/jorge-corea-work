using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASSketchFileMigratorConsoleApp;
using PTASVCADDAbstractionLibrary;
using System;

namespace PTASSketchFileMigratorConsoleAppUnitTest
{
    [TestClass]
    public class VCADDEngineTests
    {
        private readonly Mock<IAutomationEngine> engine;
        private readonly VCADDEngine vcadd;

        public VCADDEngineTests()
        {
            engine = new Mock<IAutomationEngine>();
            vcadd = new VCADDEngine(engine.Object);
        }

        [TestMethod]
        public void TestInitVCADD_AppropiateMethodsAreCalled()
        {
            vcadd.InitVCADD();

            engine.Verify(e => e.Init(), Times.Once);
            engine.Verify(e => e.NewWorld(It.IsAny<int>()), Times.Once);
            engine.Verify(e => e.SetCurrWorld(It.IsAny<int>()), Times.Once);
            engine.Verify(e => e.SetDisplayHiddenLayersMessage(0), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestInitVCADD_CatchException()
        {
            engine.Setup(e => e.Init()).Throws(new Exception());

            vcadd.InitVCADD();

            engine.Verify(e => e.Init(), Times.Once);
            engine.Verify(e => e.NewWorld(It.IsAny<int>()), Times.Never);

            Assert.ThrowsException<Exception>(() => vcadd.InitVCADD());
        }

        [TestMethod]
        public void TestTerminateVCADD_AppropiateMethodsAreCalled()
        {
            engine.Setup(e => e.GetCurrWorld()).Returns(0);

            vcadd.TerminateVCADD();

            engine.Verify(e => e.DestroyWorld(0), Times.Once);
            engine.Verify(e => e.Terminate(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestTerminateVCADD_CatchException()
        {
            engine.Setup(e => e.GetCurrWorld()).Returns(0);
            engine.Setup(e => e.DestroyWorld(It.IsAny<int>())).Throws(new Exception());

            vcadd.TerminateVCADD();

            engine.Verify(e => e.DestroyWorld(0), Times.Once);
            engine.Verify(e => e.Terminate(), Times.Never);

            Assert.ThrowsException<Exception>(() => vcadd.TerminateVCADD());
        }

        [TestMethod]
        public void TestLoadVCDFromFile_AppropiateMethodsAreCalled()
        {
            vcadd.LoadVCDFromFile("file path");

            engine.Verify(e => e.LoadVCDFromFile("file path"), Times.Once);
        }

        //[TestMethod]
        //[ExpectedException(typeof(Exception))]
        //public void TestLoadVCDFromFile_CatchException()
        //{
        //    engine.Setup(e => e.LoadVCDFromFile("a")).Throws(new Exception());

        //    vcadd.LoadVCDFromFile("a");

        //    engine.Verify(e => e.LoadVCDFromFile("a"), Times.Once);

        //    Assert.ThrowsException<Exception>(() => vcadd.LoadVCDFromFile("a"));
        //}

        [TestMethod]
        public void TestExportXML_AppropiateMethodsAreCalled()
        {
            engine.Setup(e => e.GetXMLStruct()).Returns(It.IsAny<object>());
            
            vcadd.ExportXML("a", "b");

            engine.Verify(e => e.GetXMLStruct(), Times.Once);
            engine.Verify(e => e.ExportXML(It.IsAny<object>(), "ab.xml", 0), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestExportXML_CatchException()
        {
            engine.Setup(e => e.GetXMLStruct()).Returns(It.IsAny<object>());
            engine.Setup(e => e.ExportXML(It.IsAny<object>(), "ab.xml", 0)).Throws(new Exception());

            vcadd.ExportXML("a", "b");

            engine.Verify(e => e.GetXMLStruct(), Times.Once);
            engine.Verify(e => e.ExportXML(It.IsAny<object>(), "ab.xml", 0), Times.Once);

            Assert.ThrowsException<Exception>(() => vcadd.ExportXML("a", "b"));
        }

        [TestMethod]
        public void TestClearDrawing_AppropiateMethodsAreCalled()
        {
            engine.Setup(e => e.GetCurrWorld()).Returns(0);

            vcadd.ClearDrawing();

            engine.Verify(e => e.GetCurrWorld(), Times.Once);
            engine.Verify(e => e.ClearDrawingNoPrompt(0), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestClearDrawing_CatchException()
        {
            engine.Setup(e => e.GetCurrWorld()).Throws(new Exception());

            vcadd.ClearDrawing();

            engine.Verify(e => e.GetCurrWorld(), Times.Once);
            engine.Verify(e => e.ClearDrawingNoPrompt(0), Times.Once);

            Assert.ThrowsException<Exception>(() => vcadd.ClearDrawing());
        }
    }
}
