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
    /// SEAppFinancialLookupController class test.
    /// </summary>
    [TestClass]
    public class SEAppFinancialLookupControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string Name = "522201212";

        /// <summary>
        /// Test the Get Method when a get in SEAppFinancialLookupController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppFinancialLookupController_Get_ReturnsNull()
        {
            // Arrange
            var moqSEAppFinancialManager = new Mock<ISEAppFinancialManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEAppFinancialFromSEAppDetailId = false;

            moqSEAppFinancialManager.Setup(m => m.GetSEAppFinancialFromSEAppDetailId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEAppFinancialFromSEAppDetailId = true;
                return new List<DynamicsSeniorExemptionApplicationFinancial> { };
            });

            var controller = new SEAppFinancialLookupController(moqSEAppFinancialManager.Object, moqmapper.Object);

            // Act

            ActionResult<FormSeniorExemptionApplicationFinancial[]> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledGetSEAppFinancialFromSEAppDetailId);
            moqSEAppFinancialManager.Verify(m => m.GetSEAppFinancialFromSEAppDetailId(It.Is<string>(s => s == $"{NonvalidIdForGet}")));

        }

        /// <summary>
        /// Test the Get Method when a get SEAppFinancialLookupController is not succefull.
        /// </summary>
        /// <returns>NOt Null.</returns>
        [TestMethod]
        public async Task SEAppFinancialLookupController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqSEAppFinancialManager = new Mock<ISEAppFinancialManager>();
            var moqmapper = new Mock<IMapper>();
            var calledGetSEAppFinancialFromSEAppDetailId = false;

            moqSEAppFinancialManager.Setup(m => m.GetSEAppFinancialFromSEAppDetailId(It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledGetSEAppFinancialFromSEAppDetailId = true;
                return new List<DynamicsSeniorExemptionApplicationFinancial> {
                    new DynamicsSeniorExemptionApplicationFinancial
                    { SEFinancialFormsId = new System.Guid().ToString(), Name = Name },
                    new DynamicsSeniorExemptionApplicationFinancial
                    { SEFinancialFormsId = new System.Guid().ToString(), Name = Name }
                };
            });

            moqmapper.Setup<FormSeniorExemptionApplicationFinancial[]>(m => m.Map<FormSeniorExemptionApplicationFinancial[]>(It.IsAny<DynamicsSeniorExemptionApplicationFinancial[]>())).Returns(() =>
            {
                //calledMapper = true;
                var res = new FormSeniorExemptionApplicationFinancial[]
                {
                        new FormSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString(), Name = Name } };
                return res;
            });

            var controller = new SEAppFinancialLookupController(moqSEAppFinancialManager.Object, moqmapper.Object);

            // Act
            ActionResult<FormSeniorExemptionApplicationFinancial[]> result = await controller.Get(ValidId);

            // Assert
            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledGetSEAppFinancialFromSEAppDetailId);
            moqSEAppFinancialManager.Verify(m => m.GetSEAppFinancialFromSEAppDetailId(It.Is<string>(s => s == $"{ValidId}")));
        }
    }
}