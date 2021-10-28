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
    /// SEAOccupantLookupByFinancialFormIdController class test.
    /// </summary>
    [TestClass]
    public class SEAppLookupByContactIdControllerTests
    {
        private const string NonValidContactId = "123";
        private const string ValidContactId = "12344";
        private const string TableName = "ptas_seapplications";
        private const string Name = "John Doe";

        /// <summary>
        /// Test the Get Method when a get in SEAppLookupByContactIdController is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppLookupByContactIdController_Get_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            string query = $"$expand=ptas_ptas_seapplication_ptas_seapppredefnotes&$filter=ptas_contactid/contactid eq '{NonValidContactId}'";

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplication>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return (DynamicsSeniorExemptionApplication[])null;
            });

            moqmapper.Setup<FormSeniorExemptionApplication[]>(m => m.Map<FormSeniorExemptionApplication[]>(It.IsAny<DynamicsSeniorExemptionApplication[]>())).Returns(() =>
            {
                calledMapper = true;
                return (FormSeniorExemptionApplication[])null;
            });

            var controller = new SEAppLookupByContactIdController(moqwrapper.Object, moqmapper.Object, null);

            // Act

            ActionResult<FormSeniorExemptionApplication[]> result = await controller.Get(NonValidContactId);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledCRMWrapper);
            Assert.IsTrue(calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplication>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == query)));
        }

        /// <summary>
        /// Test the Get Method when a get in SEAppLookupByContactIdController is  succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAppLookupByContactIdController_Get_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var moqmapper = new Mock<IMapper>();
            var calledCRMWrapper = false;
            var calledMapper = false;

            string query = $"$expand=ptas_ptas_seapplication_ptas_seapppredefnotes&$filter=ptas_contactid/contactid eq '{ValidContactId}'";

            moqwrapper.Setup(m => m.ExecuteGet<DynamicsSeniorExemptionApplication>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return new DynamicsSeniorExemptionApplication[]
                {new DynamicsSeniorExemptionApplication { SEAapplicationId = new System.Guid().ToString(), Name = Name }};
            });

            moqmapper.Setup<FormSeniorExemptionApplication[]>(m => m.Map<FormSeniorExemptionApplication[]>(It.IsAny<DynamicsSeniorExemptionApplication[]>())).Returns(() =>
            {
                calledMapper = true;
                return new FormSeniorExemptionApplication[]
                {new FormSeniorExemptionApplication { SEAapplicationId = new System.Guid().ToString(), Name = Name }};
            });
            var controller = new SEAppLookupByContactIdController(moqwrapper.Object, moqmapper.Object, null);

            // Act

            ActionResult<FormSeniorExemptionApplication[]> result = await controller.Get(ValidContactId);

            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(calledCRMWrapper && calledMapper);
            moqwrapper.Verify(m => m.ExecuteGet<DynamicsSeniorExemptionApplication>(It.Is<string>(s => s == $"{TableName}"), It.Is<string>(s => s == query)));
        }
    }
}