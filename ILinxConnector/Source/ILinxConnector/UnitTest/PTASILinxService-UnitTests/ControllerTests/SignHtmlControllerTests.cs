using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PTASIlinxService.Classes.Exceptions;
using PTASIlinxService.Controllers;
using PTASLinxConnectorHelperClasses.Models;

namespace PTASILinxService_UnitTests.ControllerTests
{
  [TestClass]
  public class SignHtmlControllerTests
  {
    [TestMethod]
    public void TestConstructor()
    {
      //arrange
      var FailedOnNoConfig = false;
      try
      {
        //act
        var t = new SignHtmlController(null);
      }
      catch (ArgumentException ex) when (ex.ParamName == "config")
      {
        FailedOnNoConfig = true;
      }

      //assert
      Assert.IsTrue(FailedOnNoConfig, "Should have failed with no config param.");
    }



    [TestMethod]
    public async Task TestPostFailOnBadJson()
    {
      var contentToReturn = "Not JSON";
      var failed = false;
      Mock<SignHtmlController> mockController = CreateMockController(contentToReturn);
      try
      {
        await mockController.Object.PostAsync();
      }
      catch (BadJsonException)
      {
        failed = true;
      }
      Assert.IsTrue(failed, "Should've failed on bad JSON input");
    }

    [TestMethod]
    public async Task TestPostFailOnGoodJSONButEmpty()
    {
      var contentToReturn = "{}";
      var failed = false;
      Mock<SignHtmlController> mockController = CreateMockController(contentToReturn);
      try
      {
        await mockController.Object.PostAsync();
      }
      catch (ArgumentException ex) when (ex.ParamName== "fullName")
      {
        failed = true;
      }
      Assert.IsTrue(failed, "Should've failed on bad JSON input");
    }

    [TestMethod]
    public async Task TestPostFailOnGoodJSONWithNoEmail()
    {
      var contentToReturn = "{'fullName':'Name'}".Replace("'", "\"");
      var failed = false;
      Mock<SignHtmlController> mockController = CreateMockController(contentToReturn);
      try
      {
        await mockController.Object.PostAsync();
      }
      catch (ArgumentException ex) when (ex.ParamName == "email")
      {
        failed = true;
      }
      Assert.IsTrue(failed, "Should've failed on bad email");
    }

    [TestMethod]
    public async Task TestPostFailOnGoodJSONWithNoFullName()
    {
      var contentToReturn = "{}";
      var failed = false;
      Mock<SignHtmlController> mockController = CreateMockController(contentToReturn);
      try
      {
        await mockController.Object.PostAsync();
      }
      catch (ArgumentException ex) when (ex.ParamName == "fullName")
      {
        failed = true;
      }
      Assert.IsTrue(failed, "Should've failed on missing fullName field.");
    }

    [TestMethod]
    public async Task TestPostFailOnGoodJSONWithNoHtmlContent()
    {
      var contentToReturn = "{'signerClientId':'sid','fullName':'name','email':'email','returnUrl':'rurl'}".Replace("'", "\"");
      var failed = false;
      Mock<SignHtmlController> mockController = CreateMockController(contentToReturn);
      try
      {
        await mockController.Object.PostAsync();
      }
      catch (ArgumentException ex) when (ex.ParamName == "htmlContent")
      {
        failed = true;
      }
      Assert.IsTrue(failed, "Should've failed on missing htmlContent field.");
    }

    [TestMethod]
    public async Task TestPostFailOnGoodJSONWithNoreturnUrl()
    {
      var contentToReturn = "{'fullName':'name','email':'email'}".Replace("'", "\"");
      var failed = false;
      Mock<SignHtmlController> mockController = CreateMockController(contentToReturn);
      try
      {
        await mockController.Object.PostAsync();
      }
      catch (ArgumentException ex) when (ex.ParamName == "returnUrl")
      {
        failed = true;
      }
      Assert.IsTrue(failed, "Should've failed on missing returnUrl field.");
    }

    [TestMethod]
    public async Task TestPostFailOnGoodJSONWithNoSignerClientId()
    {
      var contentToReturn = "{'fullName':'name','email':'email','returnUrl':'rurl'}".Replace("'", "\"");
      var failed = false;
      Mock<SignHtmlController> mockController = CreateMockController(contentToReturn);
      try
      {
        await mockController.Object.PostAsync();
      }
      catch (ArgumentException ex) when (ex.ParamName == "signerClientId")
      {
        failed = true;
      }
      Assert.IsTrue(failed, "Should've failed on missing signerClientId field.");
    }
    [TestMethod]
    public async Task TestPostFailOnNoContent()
    {
      var contentToReturn = string.Empty;
      var failed = false;
      Mock<SignHtmlController> mockController = CreateMockController(contentToReturn);
      try
      {
        await mockController.Object.PostAsync();
      }
      catch (ArgumentException ex) when (ex.ParamName == "Content")
      {
        failed = true;
      }
      Assert.IsTrue(failed, "Should've failed on no content");
    }

    private static Mock<SignHtmlController> CreateMockController(string contentToReturn)
    {
      Mock<IConfigParams> mockConfig = new Mock<IConfigParams>();
      mockConfig.SetupGet(m => m.DocuSignApiUrl).Returns("http://test.uri");
      var mockController = new Mock<SignHtmlController>(mockConfig.Object);
      mockController.Protected()
        .Setup<Task<string>>("GetRequestContent")
        .Returns(Task.FromResult(contentToReturn));
      return mockController;
    }
  }
}
