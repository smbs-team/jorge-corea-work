using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PTASIlinxService.Classes;
using PTASIlinxService.Controllers;
using PTASLinxConnectorHelperClasses.Models;
using PTASServicesCommon.CloudStorage;

namespace PTASILinxService_UnitTests.ControllerTests
{
  [TestClass]
  public class ApplyJsonToDynamicsControllerTests
  {
    private const string badJson = "xxx" + jsonWithController;
    private const string jsonWithController = "{ \"controller\":\"C\" }";
    private const string jsonWithNoController = "{ }";
    private const string parseMessage = "Unexpected character encountered while parsing value";

    [TestMethod]
    public async Task PatchShouldFail()
    {
      var failedOnNoRoute = false;
      var x = new Mock<ApplyJsonToDynamicsController>(new Mock<ICloudStorageProvider>().Object, new Mock<IConfigParams>().Object);
      try
      {
        var y = await x.Object.PatchAsync("", false);
      }
      catch (ArgumentException ex) when (ex.ParamName.Equals("route"))
      {
        failedOnNoRoute = true;
      }
      Assert.IsTrue(failedOnNoRoute, "Had to failed on no route.");
    }

    [TestMethod]
    public async Task PatchShouldFailOnEmptyRoute()
    {
      var failedOnNoRoute = false;
      var x = new Mock<ApplyJsonToDynamicsController>(new Mock<ICloudStorageProvider>().Object, new Mock<IConfigParams>().Object);
      try
      {
        var y = await x.Object.PatchAsync("", false);
      }
      catch (ArgumentException ex) when (ex.ParamName.Equals("route"))
      {
        failedOnNoRoute = true;
      }
      Assert.IsTrue(failedOnNoRoute, "Had to failed on no route.");
    }

    [TestMethod]
    public async Task BadJsonShouldFail()
    {
      // arrange.
      var x = new Mock<ApplyJsonToDynamicsController>(new Mock<ICloudStorageProvider>().Object, new Mock<IConfigParams>().Object);

      x.Protected().Setup<Task<IEnumerable<JsonBlob>>>("GetContents", ItExpr.IsAny<string>(), ItExpr.IsAny<bool>()).Returns(Task.FromResult(new JsonBlob[] {
      new JsonBlob {
        Content = badJson, Route=Guid.Empty.ToString() }
      } as IEnumerable<JsonBlob>));

      x.Protected().Setup<Task>("MoveItem", ItExpr.IsAny<JsonBlob>(), ItExpr.IsAny<string>()).CallBase();
      x.Protected().Setup<Task>("MoveToDynamics", ItExpr.IsAny<string>(), ItExpr.IsAny<string>()).CallBase();

      // act.
      var r = await x.Object.PostAsync("test");

      // assert.
      Assert.IsTrue(r.Error, "Should have had an error on bad json.");
      Assert.IsTrue(r.ErrorMessage.Contains(parseMessage), "Should have a parsing error message.");
    }

    [TestMethod]
    public async Task InvalidaRoutesShouldBeIgnored()
    {
      // arrange.
      var x = new Mock<ApplyJsonToDynamicsController>(new Mock<ICloudStorageProvider>().Object, new Mock<IConfigParams>().Object);
      var calledMoveItem = false;

      x.Protected().Setup<Task<IEnumerable<JsonBlob>>>("GetContents", ItExpr.IsAny<string>(), ItExpr.IsAny<bool>()).Returns(Task.FromResult(new JsonBlob[] {
      new JsonBlob {Content=jsonWithController, Route="INVALID/ROUTE" }
      } as IEnumerable<JsonBlob>));

      x.Protected().Setup<Task>("MoveItem", ItExpr.IsAny<JsonBlob>(), ItExpr.IsAny<string>()).Callback<JsonBlob, string>((a, b) =>
      {
        // should not get here.
        calledMoveItem = true;
      });

      // act.
      var r = await x.Object.PostAsync("test");
      // assert.
      Assert.IsFalse(calledMoveItem, "Should not have called move item.");
      Assert.IsTrue(r.Result.Equals($"Processed {0} items."));
    }

    [TestMethod]
    public async Task JsonWithNoControllerShouldBeIgnored()
    {
      // arrange.
      var x = new Mock<ApplyJsonToDynamicsController>(new Mock<ICloudStorageProvider>().Object, new Mock<IConfigParams>().Object);
      var calledMoveItem = false;

      x.Protected().Setup<Task<IEnumerable<JsonBlob>>>("GetContents", ItExpr.IsAny<string>(), ItExpr.IsAny<bool>()).Returns(Task.FromResult(new JsonBlob[] {
      new JsonBlob {
        Content = jsonWithNoController, Route=Guid.Empty.ToString() }
      } as IEnumerable<JsonBlob>));

      x.Protected().Setup<Task>("MoveItem", ItExpr.IsAny<JsonBlob>(), ItExpr.IsAny<string>()).Callback<JsonBlob, string>((a, b) =>
      {
        // should not get here.
        calledMoveItem = true;
      });

      // act.
      var r = await x.Object.PostAsync("test");
      // assert.
      Assert.IsFalse(calledMoveItem, "Should not have called move item.");
      Assert.IsTrue(r.Result.Equals($"Processed {0} items."));
    }

    [TestMethod]
    public void ShouldFailOnEmptyConfig()
    {
      // arrange.
      var failed = false;
      try
      {
        // act.
        var x = new ApplyJsonToDynamicsController(new Mock<ICloudStorageProvider>().Object, null);
      }
      catch (ArgumentException ex) when (ex.ParamName == "config")
      {
        failed = true;
      }
      // assert.
      Assert.IsTrue(failed, "Should've failed on null config.");
    }


    [TestMethod]
    public void ShouldFailOnEmptyProvider()
    {
      // arrange.
      var failed = false;
      try
      {
        // act.
        var x = new ApplyJsonToDynamicsController(null, new Mock<IConfigParams>().Object);
      }
      catch (ArgumentException ex) when (ex.ParamName == "provider")
      {
        failed = true;
      }
      // assert.
      Assert.IsTrue(failed, "Should've failed on null provider.");
    }

    [TestMethod]
    public async Task ShouldFailOnEmptyRoute()
    {
      // arrange.
      var failed = false;
      var x = new ApplyJsonToDynamicsController(new Mock<ICloudStorageProvider>().Object, new Mock<IConfigParams>().Object);
      try
      {
        // act.
        var r = await x.PostAsync("");
      }
      catch (ArgumentException ex) when (ex.ParamName == "route")
      {
        failed = true;
      }
      // assert.
      Assert.IsTrue(failed, "Should fail on empty route.");
    }

    [TestMethod]
    public async Task ShouldSucceedWithEmptyDataset()
    {
      // arrange.
      var x = new Mock<ApplyJsonToDynamicsController>(new Mock<ICloudStorageProvider>().Object, new Mock<IConfigParams>().Object);
      x.Protected().Setup<Task<IEnumerable<JsonBlob>>>("GetContents", ItExpr.IsAny<string>(), ItExpr.IsAny<bool>()).Returns(Task.FromResult(new JsonBlob[] { } as IEnumerable<JsonBlob>));
      // act.
      var r = await x.Object.PostAsync("test");
      // assert.
      Assert.IsFalse(r.Error, "Should have no error on no records.");
      Assert.IsTrue(r.Result.Equals($"Processed {0} items."));
    }
  }
}
