using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using Moq.Protected;
using PTASIlinxService.Controllers;
using PTASLinxConnectorHelperClasses.Models;
using PTASServicesCommon.CloudStorage;

namespace PTASILinxService_UnitTests.ControllerTests
{
  [TestClass]
  public class JsonStoreControllerTests
  {
    private const string AnyFileName = "root.jpg";
    private const string Route = "1/2";

    [TestMethod]
    public void TestConstructor()
    {
      // arrange
      var bombed = false;
      try
      {
        // act
        using (var controller = new JsonStoreController(null, null))
        {
        }
      }
      catch (ArgumentNullException)
      {
        bombed = true;
      }
      //assert
      Assert.IsTrue(bombed, "Needed to fail on empty constructor parameters.");
    }

    [TestMethod]
    public async Task TestNullRoute()
    {
      // arrange
      var controller = CreateMockController();
      var bombedOnNullRoute = false;
      try
      {
        // act
        var result = await controller.Object.PostAsync(null);
      }
      catch (ArgumentException ex)
      {
        bombedOnNullRoute = ex.Message?.Contains(JsonStoreController.RouteRequired) ?? false;
      }
      Assert.IsTrue(bombedOnNullRoute, "Needed to fail on null route.");
    }

    [TestMethod]
    public async Task TestRootRoute()
    {
      // arrange
      var controller = CreateMockController();
      var bombedOnNoPayload = false;
      try
      {
        // act
        var result = await controller.Object.PostAsync(AnyFileName);
      }
      catch (ArgumentException ex)
      {
        bombedOnNoPayload = ex.Message?.Contains(JsonStoreController.PayloadRequired) ?? false;
      }
      Assert.IsTrue(bombedOnNoPayload, "Needed to fail when no payload.");
    }

    [TestMethod]
    public async Task TestEmptyPayload()
    {
      // arrange
      Mock<JsonStoreController> controller = CreateMockController();
      controller.Protected().Setup<Task<string>>("GetRequestContent").Returns(Task.FromResult(""));
      var bombedOnEmptyPayload = false;
      try
      {
        // act
        var result = await controller.Object.PostAsync(Route);
      }
      catch (ArgumentException ex)
      {
        bombedOnEmptyPayload = ex.Message?.Contains(JsonStoreController.PayloadRequired) ?? false;
      }
      Assert.IsTrue(bombedOnEmptyPayload, "Needed to fail on no payload.");
    }

    private static Mock<JsonStoreController> CreateMockController()
    {
      var m1 = new Mock<ICloudStorageProvider>();
      var m2 = new Mock<IConfigParams>();
      var controller = new Mock<JsonStoreController>(m1.Object, m2.Object);
      return controller;
    }

    [TestMethod]
    public async Task TestCallToSaveContent()
    {
      // arrange
      var controller = CreateMockController();
      var calledSaveContent = false;
      controller.Protected().Setup<Task<string>>("GetRequestContent")
        .Returns(Task.FromResult("{\"test}\":\"test\""));
      controller.Protected().Setup("SaveContent", ItExpr.IsAny<string>(), ItExpr.IsAny<string>()).Callback<string, string>((a, b) =>
      {
        calledSaveContent = true;
        Task.FromResult(default(string));
      });
      var result = await controller.Object.PostAsync(Route);
      Assert.IsTrue(calledSaveContent, "Did not call save content function.");
    }

    [TestMethod]
    public async Task TestCallToGet()
    {
      // arrange
      var controller = CreateMockController();
      var calledGetBlob = false;
      controller.Protected().Setup<Task<string>>("GetBlobContent", ItExpr.IsAny<string>()).Callback<string>(_ =>
      {
        calledGetBlob = true;
      }).Returns(Task.FromResult(""));

      // act
      System.Net.Http.HttpResponseMessage result = await controller.Object.GetAsync(Route);

      // assert
      Assert.IsTrue(calledGetBlob, "Needed to call the get blob.");
    }



    [TestMethod]
    public async Task TestCallToDelete()
    {
      // arrange
      var controller = CreateMockController();
      var calledGetBlobs = false;
      var calledDeleteBlobs = false;


      // IEnumerable<IListBlobItem> files = this.GetBlobsToDelete(route, isDirectory);
      // bool[] result = await this.DeleteBlobsAsync(files);
      var results = new IListBlobItem[] { } as IEnumerable<IListBlobItem>;
      controller.Protected().Setup<IEnumerable<IListBlobItem>>("GetBlobsToDelete", ItExpr.IsAny<string>(), ItExpr.IsAny<bool>()).Callback<string, bool>((a, b) =>
       {
         calledGetBlobs = true;
       }).Returns(results);

      //protected virtual async Task<bool[]> DeleteBlobsAsync(IEnumerable<IListBlobItem> files)
      controller.Protected()
          .Setup<Task<bool[]>>("DeleteBlobsAsync", ItExpr.IsAny<IEnumerable<IListBlobItem>>())
          .Callback<IEnumerable<IListBlobItem>>(_ =>
          {
            calledDeleteBlobs = true;
          });

      // act
      await controller.Object.DeleteAsync(Route);

      // assert
      Assert.IsTrue(calledGetBlobs, "Needed to call the get blob.");
      Assert.IsTrue(calledDeleteBlobs, "Needed to call delete blob.");
    }

  }
}
