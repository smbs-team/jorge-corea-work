using System.Net;
using System.Threading.Tasks;

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
    /// SEAFinancialFormsSEAOpccupantsRelationship class test.
    /// </summary>
    [TestClass]
    public class SEAFinancialFormsSEAOpccupantsRelationshipControllerTests
    {
        private const string NonValidSEAFinancialFormsId = "123";
        private const string ValidSEAFinancialFormsId = "123";
        private const string SEAppOccupantId = "1234";
        private const string TableName = "ptas_sefinancialformses";
        private const string NavigationProperty = "ptas_seappoccupant_ptas_sefinancialforms";

        /// <summary>
        /// Test the Get Method when a get in SEAFinancialFormsSEAOpccupantsRelationship is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAFinancialFormsSEAOpccupantsRelationshipControllerTests_Put_ReturnsNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteNTONPost(It.IsAny<string>(), It.IsAny<DynamicsNTONRelationship>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
             {
                 calledCRMWrapper = true;
                 return false;
             });

            moqconfig.SetupGet(m => m.CRMUri).Returns("https://ptas-dev.api.crm9.dynamics.com");
            moqconfig.SetupGet(m => m.ApiRoute).Returns("/api/data/v9.1/");

            var controller = new SEAFinancialFormsSEAOpccupantsRelationshipController(moqwrapper.Object, moqconfig.Object);

            FormSEAFinancialFormsSEAOpccupantsRelationship value = new FormSEAFinancialFormsSEAOpccupantsRelationship { SEAppOccupantId = SEAppOccupantId };

            var v = $"https://ptas-dev.api.crm9.dynamics.com/api/data/v9.1/ptas_seappoccupants(" + SEAppOccupantId + ")";

            // Act

            ActionResult<FormSEAFinancialFormsSEAOpccupantsRelationship> result = await controller.Post(NonValidSEAFinancialFormsId, value);
            // Assert

            Assert.IsTrue((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteNTONPost(It.Is<string>(s => s == $"{TableName}"), It.Is<DynamicsNTONRelationship>(s => s.TheRefId == v), It.Is<string>(s => s == $"{NonValidSEAFinancialFormsId}"), It.Is<string>(s => s == $"{NavigationProperty}")));
        }

        /// <summary>
        /// Test the Get Method when a get in SEAFinancialFormsSEAOpccupantsRelationship is not succefull.
        /// </summary>
        /// <returns>Null.</returns>
        [TestMethod]
        public async Task SEAFinancialFormsSEAOpccupantsRelationshipControllerTests_Put_ReturnsNotNull()
        {
            // Arrange
            var moqconfig = new Mock<IConfigurationParams>();
            MockClassFiller.ConfigFiller(moqconfig);
            var moqwrapper = new Mock<CRMWrapper>(moqconfig.Object, new Mock<ITokenManager>().Object);
            var calledCRMWrapper = false;

            moqwrapper.Setup(m => m.ExecuteNTONPost(It.IsAny<string>(), It.IsAny<DynamicsNTONRelationship>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() =>
            {
                calledCRMWrapper = true;
                return true;
            });

            moqconfig.SetupGet(m => m.CRMUri).Returns("https://ptas-dev.api.crm9.dynamics.com");
            moqconfig.SetupGet(m => m.ApiRoute).Returns("/api/data/v9.1/");

            var controller = new SEAFinancialFormsSEAOpccupantsRelationshipController(moqwrapper.Object, moqconfig.Object);

            FormSEAFinancialFormsSEAOpccupantsRelationship value = new FormSEAFinancialFormsSEAOpccupantsRelationship { SEAppOccupantId = SEAppOccupantId };

            var v = $"https://ptas-dev.api.crm9.dynamics.com/api/data/v9.1/ptas_seappoccupants(" + SEAppOccupantId + ")";

            // Act

            ActionResult<FormSEAFinancialFormsSEAOpccupantsRelationship> result = await controller.Post(ValidSEAFinancialFormsId, value);
            // Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(calledCRMWrapper);
            moqwrapper.Verify(m => m.ExecuteNTONPost(It.Is<string>(s => s == $"{TableName}"), It.Is<DynamicsNTONRelationship>(s => s.TheRefId == v), It.Is<string>(s => s == $"{ValidSEAFinancialFormsId}"), It.Is<string>(s => s == $"{NavigationProperty}")));
        }
    }
}