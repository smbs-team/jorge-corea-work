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
    /// ParcelLookupController class test.
    /// </summary>
    [TestClass]
    public class ParcelLookupControllerTests
    {
        private const string SearchValue = "and";
        private const string Name = "the Name value";
        private const string Source = "the source value";

        /// <summary>
        /// Test the Get Method when a get in ParcelLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task ParcelLookupController_Get_ReturnsNull()
        {
            // Arrange
            var moqParcelManager = new Mock<IParcelManager>();
            var moqmapper = new Mock<IMapper>();
            var calledLookupAddress = false;
            var calledMapper = false;

            moqParcelManager.Setup(m => m.LookupParcel(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledLookupAddress = true;
                return new List<ParcelLookupResult> { };
            });

            moqmapper.Setup<List<ParcelLookupResult>>(m => m.Map<List<ParcelLookupResult>>(It.IsAny<List<ParcelLookupResult>>())).Returns(() =>
            {
                calledMapper = true;
                return new List<ParcelLookupResult> { };
            });

            var controller = new ParcelLookupController(moqParcelManager.Object, moqmapper.Object);

            // Act

            ActionResult<List<ParcelLookupResult>> result = await controller.Get(SearchValue);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledLookupAddress && calledMapper);
            moqParcelManager.Verify(m => m.LookupParcel(It.Is<string>(s => s == $"{SearchValue}")));
        }
        /// <summary>
        /// Test the Get Method when a get in ParcelLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task ParcelLookupController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqParcelManager = new Mock<IParcelManager>();
            var moqmapper = new Mock<IMapper>();
            var calledLookupAddress = false;
            var calledMapper = false;

            moqParcelManager.Setup(m => m.LookupParcel(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledLookupAddress = true;
                return new List<ParcelLookupResult>
                {
                    new ParcelLookupResult(new DynamicsParcelDetail { Name = Name}, Source)
                };
            });

            moqmapper.Setup<List<ParcelLookupResult>>(m => m.Map<List<ParcelLookupResult>>(It.IsAny<List<ParcelLookupResult>>())).Returns(() =>
            {
                calledMapper = true;
                return new List<ParcelLookupResult>
                {
                    new ParcelLookupResult(new DynamicsParcelDetail { Name = Name}, Source)
                };
            });

            var controller = new ParcelLookupController(moqParcelManager.Object, moqmapper.Object);

            // Act

            ActionResult<List<ParcelLookupResult>> result = await controller.Get(SearchValue);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledLookupAddress && calledMapper);
            moqParcelManager.Verify(m => m.LookupParcel(It.Is<string>(s => s == $"{SearchValue}")));
        }
    }
}