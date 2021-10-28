using System;
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
using PTASDynamicsCrudHelperClasses.Exception;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;

namespace PTASDynamicsCrudCore_UnitTests.ControllerTests
{
    /// <summary>
    /// ContactLookupController class test.
    /// </summary>
    [TestClass]
    public class ContactLookupControllerTests
    {
        private const string NonValidEmailForGet = "123";
        private const string ValidEmail = "1234";
        private const string FirstName = "John";

        /// <summary>
        /// Test the Get Method when a get in ContactLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task ContactLookupController_Get_ReturnsNull()
        {
            // Arrange
            var moqContactManager = new Mock<IContactManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetContactFromEmail = false;

            moqContactManager.Setup(m => m.GetContactFromEmail(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetContactFromEmail = true;
                return (DynamicsContact)null;
            });

            var controller = new ContactLookupController(moqContactManager.Object, moqmapper.Object);

            // Act
            ActionResult<FormContact> result = await controller.Get(NonValidEmailForGet);

            // Assert

            Assert.IsTrue((result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.NotFound);
            Assert.IsTrue(calledGetContactFromEmail);
            moqContactManager.Verify(m => m.GetContactFromEmail(It.Is<string>(s => s == $"{NonValidEmailForGet}")));
            }

        /// <summary>
        /// Test the Get Method when a get ContactLookupController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task ContactLookupController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqContactManager = new Mock<IContactManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetContactFromEmail = false;

            moqContactManager.Setup(m => m.GetContactFromEmail(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetContactFromEmail = true;
                return  new DynamicsContact
                    { ContactId = new System.Guid().ToString(), FirstName = FirstName };
            });

            moqmapper.Setup<FormContact>(m => m.Map<FormContact>(It.IsAny<DynamicsContact>())).Returns(() =>
            {
                var res = new FormContact
                    { ContactId = new System.Guid().ToString(), FirstName = FirstName };
                return res;
            });

            var controller = new ContactLookupController(moqContactManager.Object, moqmapper.Object);

            // Act
            ActionResult<FormContact> result = await controller.Get(ValidEmail);

            // Assert
            Assert.IsNotNull((result.Result as JsonResult).Value);
            Assert.IsTrue(calledGetContactFromEmail);
            moqContactManager.Verify(m => m.GetContactFromEmail(It.Is<string>(s => s == $"{ValidEmail}")));
        }
    }
}