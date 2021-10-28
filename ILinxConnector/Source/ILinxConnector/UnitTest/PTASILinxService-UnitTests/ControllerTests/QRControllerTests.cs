using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PTASIlinxService.Classes;
using PTASIlinxService.Controllers;

namespace PTASILinxService_UnitTests
{
  [TestClass]
  public class QRControllerTests
  {
    private const string jsonValues = "{\"v\":\"a\"}";
    private const string keyPairValues = "v=a";

    [TestMethod]
    public async Task ShouldReturnEmptyImageOnNoContent()
    {
      // arrange.
      var x = new Mock<QRController>();
      x.Protected().Setup<Task<string>>("GetRequestContent").Returns(Task.FromResult("{}"));

      // act.
      var result = await x.Object.PostAsync();

      // assert.
      Assert.AreEqual(0, result.FileBytes.Length, "Should return empty file on no content.");
    }

    [TestMethod]
    public async Task ShouldSerializeProperly()
    {
      // arrange.
      var x = new Mock<QRController>();
      var receivedValues = "";
      x.Protected().Setup<Task<string>>("GetRequestContent").Returns(Task.FromResult(jsonValues));
      x.Protected()
        .Setup<QRCodeResult>("CreateQRCode", ItExpr.IsAny<string>())
        .Callback<string>(xs => receivedValues = xs);
      // act.
      var result = await x.Object.PostAsync();

      // assert.
      Assert.AreEqual(keyPairValues, receivedValues, "JSON should've parsed to \"v = a\"");
    }

    [TestMethod]
    public async Task ShouldReturnANonEmptyImage()
    {
      // arrange.
      var x = new Mock<QRController>();
      x.Protected().Setup<Task<string>>("GetRequestContent").Returns(Task.FromResult(jsonValues));
      x.Protected()
        .Setup<QRCodeResult>("CreateQRCode", ItExpr.IsAny<string>())
        .CallBase();
      // act.
      var result = await x.Object.PostAsync();

      // assert.
      Assert.AreNotEqual(0, result.FileBytes.Length,"Data should've returned a QR Code.");
    }
  }
}
