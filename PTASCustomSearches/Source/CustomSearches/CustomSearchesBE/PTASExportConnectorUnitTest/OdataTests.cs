using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTASExportConnector.Exceptions;
using PTASExportConnector.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PTASExportConnectorUnitTest
{
    [TestClass]
    public class OdataTests
    {
        private Odata oData;

        public OdataTests()
        {
            oData = new Odata();
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestConstructor_ShouldThrowWhenCrmUriIsNullOrEmpty(string param)
        {
            oData.Init(param, "one", "two", "three");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestConstructor_ShouldThrowWhenAuthUriIsNullOrEmpty(string param)
        {
            oData.Init("zero", param, "two", "three");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestConstructor_ShouldThrowWhenCliendIdIsNullOrEmpty(string param)
        {
            oData.Init("zero", "one", param, "three");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public void TestConstructor_ShouldThrowWhenCliendSecretIsNullOrEmpty(string param)
        {
            oData.Init("zero", "one", "two", param);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public async Task TestCreate_ShouldThrowWhenApiRequestIsNullOrEmpty(string param)
        {
            await oData.Create(param, "objectToSave", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ExportConnectorException))]
        public async Task TestCreate_ShouldThrowWhenObjectToSaveIsNull()
        {
            await oData.Create("apiRequest", null, null);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public async Task TestUpdate_ShouldThrowWhenQueryStringIsNullOrEmpty(string param)
        {
            await oData.Update(param, "json", "keyStr");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public async Task TestUpdate_ShouldThrowWhenEntityIsNullOrEmpty(string param)
        {
            await oData.Update("queryString", param, "keyStr");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public async Task TestUpdate_ShouldThrowWhenKeyStrIsNullOrEmpty(string param)
        {
            await oData.Update("queryString", "json", param);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ExportConnectorException))]
        public async Task TestDelete_ShouldThrowWhenApiRequestIsNullOrEmpty(string param)
        {
            await oData.Delete(param);
        }

    }
}
