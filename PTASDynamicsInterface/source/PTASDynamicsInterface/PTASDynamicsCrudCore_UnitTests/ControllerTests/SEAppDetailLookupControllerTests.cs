using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASDynamicsCrudCore.Controllers;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;

namespace PTASDynamicsCrudCore_UnitTests.ControllerTests
{
    /// <summary>
    /// SEAppDetailLookupController class test.
    /// </summary>
    [TestClass]
    public class SEAppDetailLookupControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string AccountNumber = "522201212";

        /// <summary>
        /// Test the Get Method when a get in SEAppDetailLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppDetailLookupController_Get_ReturnsNull()
        {
            // Arrange
            var moqSEAppDetailManager = new Mock<ISEAppDetailManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEAppDetailFromSEAppId = false;

            moqSEAppDetailManager.Setup(m => m.GetSEAppDetailFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEAppDetailFromSEAppId = true;
                return new List<DynamicsSeniorExemptionApplicationDetail> { };
            });

            var controller = new SEAppDetailLookupController(moqSEAppDetailManager.Object, moqmapper.Object);

            // Act

            ActionResult<FormSeniorExemptionApplicationDetail[]> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledGetSEAppDetailFromSEAppId);
            moqSEAppDetailManager.Verify(m => m.GetSEAppDetailFromSEAppId(It.Is<string>(s => s == $"{NonvalidIdForGet}")));

        }

        /// <summary>
        /// Test the Get Method when a get SEAppDetailLookupController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEAppDetailLookupController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqSEAppDetailManager = new Mock<ISEAppDetailManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEAppDetailFromSEAppId = false;

            moqSEAppDetailManager.Setup(m => m.GetSEAppDetailFromSEAppId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEAppDetailFromSEAppId = true;
                return new List<DynamicsSeniorExemptionApplicationDetail> {
                    new DynamicsSeniorExemptionApplicationDetail
                    { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber },
                    new DynamicsSeniorExemptionApplicationDetail
                    { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber }
                };
            });

            moqmapper.Setup<FormSeniorExemptionApplicationDetail[]>(m => m.Map<FormSeniorExemptionApplicationDetail[]>(It.IsAny<DynamicsSeniorExemptionApplicationDetail[]>())).Returns(() =>
             {
                 //calledMapper = true;
                 var res = new FormSeniorExemptionApplicationDetail[]
                 {
                        new FormSeniorExemptionApplicationDetail { SEAppdetailid = new System.Guid().ToString(), AccountNumber = AccountNumber } };
                 return res;
             });

            var controller = new SEAppDetailLookupController(moqSEAppDetailManager.Object, moqmapper.Object);

            // Act
            ActionResult<FormSeniorExemptionApplicationDetail[]> result = await controller.Get(ValidId);

            // Assert
            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledGetSEAppDetailFromSEAppId);
            moqSEAppDetailManager.Verify(m => m.GetSEAppDetailFromSEAppId(It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}