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
    /// AddressLookupController class test.
    /// </summary>
    [TestClass]
    public class AddressLookupControllerTests
    {
        private const string SearchValue = "and";
        private const string Address = "the address value";

        /// <summary>
        /// Test the Get Method when a get in AddressLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task AddressLookupController_Get_ReturnsNull()
        {
            // Arrange
            var moqParcelManager = new Mock<IParcelManager>();
            var moqmapper = new Mock<IMapper>();
            var calledLookupAddress = false;
            var calledMapper = false;

            moqParcelManager.Setup(m => m.LookupAddress(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledLookupAddress = true;
                return new List<FormattedAddress> { };
            });

            moqmapper.Setup<List<FormattedAddress>>(m => m.Map<List<FormattedAddress>>(It.IsAny<List<FormattedAddress>>())).Returns(() =>
            {
                calledMapper = true;
                return new List<FormattedAddress> { };
            });

            var controller = new AddressLookupController(moqParcelManager.Object, moqmapper.Object);

            // Act

            ActionResult<List<FormattedAddress>> result = await controller.Get(SearchValue);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode== (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledLookupAddress && calledMapper);
            moqParcelManager.Verify(m => m.LookupAddress(It.Is<string>(s => s == $"{SearchValue}")));
        }
        /// <summary>
        /// Test the Get Method when a get in AddressLookupController is succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task AddressLookupController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqParcelManager = new Mock<IParcelManager>();
            var moqmapper = new Mock<IMapper>();
            var calledLookupAddress = false;
            var calledMapper = false;

            moqParcelManager.Setup(m => m.LookupAddress(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledLookupAddress = true;
                return new List<FormattedAddress>
                {
                    new FormattedAddress { Address = Address }
                };
            });

            moqmapper.Setup<List<FormattedAddress>>(m => m.Map<List<FormattedAddress>>(It.IsAny<List<FormattedAddress>>())).Returns(() =>
            {
                calledMapper = true;
                return new List<FormattedAddress>
                {
                    new FormattedAddress { Address = Address }
                };
            });

            var controller = new AddressLookupController(moqParcelManager.Object, moqmapper.Object);

            // Act

            ActionResult<List<FormattedAddress>> result = await controller.Get(SearchValue);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledLookupAddress && calledMapper);
            moqParcelManager.Verify(m => m.LookupAddress(It.Is<string>(s => s == $"{SearchValue}")));

        }
    }
}