using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASDynamicsCrudCore.Controllers;
using PTASDynamicsCrudHelperClasses.Classes;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;

namespace PTASDynamicsCrudCore_UnitTests.ControllerTests
{
    /// <summary>
    /// EAppNoteLookupController class test.
    /// </summary>
    [TestClass]
    public class SEAppNoteLookupControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string Name = "522201212";

        /// <summary>
        /// Test the Get Method when a get in SEAppNoteLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppNoteLookupController_Get_ReturnsNull()
        {
            // Arrange
            var moqSEAppNoteManager = new Mock<ISEAppNoteManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEAppNoteFromSEAppId = false;

            moqSEAppNoteManager.Setup(m => m.GetSEAppNoteFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEAppNoteFromSEAppId = true;
                return new List<DynamicsSEAppNote> { };
            });

            var controller = new SEAppNoteLookupController(moqSEAppNoteManager.Object, moqmapper.Object);

            // Act

            ActionResult<FormSEAppNote[]> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledGetSEAppNoteFromSEAppId);
            moqSEAppNoteManager.Verify(m => m.GetSEAppNoteFromSEAppId(It.Is<string>(s => s == $"{NonvalidIdForGet}")));

        }

        /// <summary>
        /// Test the Get Method when a get SEAppNoteLookupController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEAppNoteLookupController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqSEAppNoteManager = new Mock<ISEAppNoteManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEAppNoteFromSEAppId = false;

            moqSEAppNoteManager.Setup(m => m.GetSEAppNoteFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEAppNoteFromSEAppId = true;
                return new List<DynamicsSEAppNote> {
                    new DynamicsSEAppNote
                    {SEAppNoteId = new System.Guid().ToString(), Name = Name },
                    new DynamicsSEAppNote
                    {SEAppNoteId = new System.Guid().ToString(), Name = Name }
                };
            });

            moqmapper.Setup<FormSEAppNote[]>(m => m.Map<FormSEAppNote[]>(It.IsAny<DynamicsSEAppNote[]>())).Returns(() =>
            {
                //calledMapper = true;
                var res = new FormSEAppNote[]
                {
                        new FormSEAppNote {SEAppNoteId = new System.Guid().ToString(), Name = Name } };
                return res;
            });

            var controller = new SEAppNoteLookupController(moqSEAppNoteManager.Object, moqmapper.Object);

            // Act
            ActionResult<FormSEAppNote[]> result = await controller.Get(ValidId);

            // Assert
            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledGetSEAppNoteFromSEAppId);
            moqSEAppNoteManager.Verify(m => m.GetSEAppNoteFromSEAppId(It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}