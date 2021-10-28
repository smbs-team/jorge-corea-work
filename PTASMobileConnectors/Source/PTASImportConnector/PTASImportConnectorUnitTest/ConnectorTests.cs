using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTASImportConnector.SDK;
using System;
using System.Collections.Generic;

namespace PTASImportConnectorUnitTest
{
    [TestClass]
    public class ConnectorTests
    {
        private readonly Connector connector;
        private readonly List<Dictionary<string, object>> dataToUpload;
        private readonly Dictionary<string, object> dic;

        public ConnectorTests()
        {
            connector = new Connector();
            dataToUpload = new List<Dictionary<string, object>>();
            dic = new Dictionary<string, object>();

            dic.Add("key", "value");
            dataToUpload.Add(dic);
        }

        [TestMethod]
        [DataRow("", DisplayName = "Connection String")]
        [DataRow(null, DisplayName = "Connection String")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInit_ShouldThrowWhenConnectionStringIsNullOrEmpty(string param)
        {
            connector.Init(param, null);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddUploadData_ShouldThrowWhenEntityKindIsNullOrEmpty(string param)
        {
            connector.AddUploadData(param, dataToUpload, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAddUploadData_ShouldThrowWhenDataToUploadIsRqualToZero()
        {
            connector.AddUploadData("name", dataToUpload, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestProcessDataForTicket_ShouldThrowWhenChangeSetIdIsLessThanZero()
        {
            connector.ProcessDataForTicket(-1, true, true);
        }
    }
}
