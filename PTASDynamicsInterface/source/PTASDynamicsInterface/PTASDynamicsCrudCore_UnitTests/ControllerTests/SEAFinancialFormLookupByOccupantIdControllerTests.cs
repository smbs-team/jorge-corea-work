using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using PTASCRMHelpers;

using PTASDynamicsCrudCore.Controllers;

using PTASDynamicsCrudCore_UnitTests.HelperClasses;

using PTASDynamicsCrudHelperClasses.Classes;
using PTASDynamicsCrudHelperClasses.Interfaces;
using PTASDynamicsCrudHelperClasses.JSONMappings;

namespace PTASDynamicsCrudCore_UnitTests.ControllerTests
{
    /// <summary>
    /// SEAFinancialFormLookupByOccupantIdController class test.
    /// </summary>
    [TestClass]
    public class SEAFinancialFormLookupByOccupantIdControllerTests
    {
        private const string NonvalidIdForGet = "123";
        private const string ValidId = "1234";
        private const string TableName = "ptas_sefinancialformses";
        private const string Name = "John Doe";

        /// <summary>
        /// Test the Get Method when a get in SEAFinancialFormLookupByOccupantIdController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAFinancialFormLookupByOccupantIdController_Get_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            string query = $"$expand=ptas_seappoccupant_ptas_sefinancialforms&$filter=ptas_seappoccupant_ptas_sefinancialforms/any(o: o/ptas_seappoccupantid eq '{NonvalidIdForGet}')";

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return (DynamicsSeniorExemptionApplicationFinancial[])null;
            });

            moqmapper.Setup<FormSeniorExemptionApplicationFinancial[]>(m => m.Map<FormSeniorExemptionApplicationFinancial[]>(It.IsAny<DynamicsSeniorExemptionApplicationFinancial[]>())).Returns(() =>
            {
                calledMapper = true;
                return (FormSeniorExemptionApplicationFinancial[])null;
            });

            var controller = new SEAFinancialFormLookupByOccupantIdController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormSeniorExemptionApplicationFinancial[]> result = await controller.Get(NonvalidIdForGet);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == query)));
        }

        /// <summary>
        /// Test the Get Method when a get in SEAFinancialFormLookupByOccupantIdController is  succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAFinancialFormLookupByOccupantIdController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            string query = $"$expand=ptas_seappoccupant_ptas_sefinancialforms&$filter=ptas_seappoccupant_ptas_sefinancialforms/any(o: o/ptas_seappoccupantid eq '{ValidId}')";

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSeniorExemptionApplicationFinancial[]
                {new DynamicsSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString(), Name = Name }};
            });

            moqmapper.Setup<FormSeniorExemptionApplicationFinancial[]>(m => m.Map<FormSeniorExemptionApplicationFinancial[]>(It.IsAny<DynamicsSeniorExemptionApplicationFinancial[]>())).Returns(() =>
            {
                calledMapper = true;
                return new FormSeniorExemptionApplicationFinancial[]
                {new FormSeniorExemptionApplicationFinancial { SEFinancialFormsId = new System.Guid().ToString(), Name = Name }};
            });
            var controller = new SEAFinancialFormLookupByOccupantIdController(moqwrapper.Object, moqmapper.Object);

            // Act

            ActionResult<FormSeniorExemptionApplicationFinancial[]> result = await controller.Get(ValidId);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplicationFinancial>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == query)));
        }
    }
}