using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D2SSyncHelpers.Exceptions;
using D2SSyncHelpers.Services;
using Moq;
using NUnit.Framework;

namespace DynamicsToSQLBlazorTests
{
    class MetadataFromDynamicsServiceTests
    {

        const string RootOnlyXML = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
            </edmx:Edmx>
";

        [Test]
        public void Test0()
        {
            // arrange
            var failedOnMissingParams = false;
            try
            {
                // act
                var p = new MetadataFromDynamicsService(null);
            }
            catch (ArgumentException ex) when (ex.ParamName == "reader")
            {
                failedOnMissingParams = true;
            }
            // assert
            Assert.IsTrue(failedOnMissingParams);
        }

        [Test]
        public async System.Threading.Tasks.Task TestEmptyRoot()
        {
            var mustFailOnNoSchema = false;
            try
            {
                var content = string.Empty;
                Mock<IAsyncStringReader> m = SetupReader(RootOnlyXML);
                var p = new MetadataFromDynamicsService(m.Object);
                var n = await p.GetTables();
            }
            catch (DynamicsXMLException)
            {
                mustFailOnNoSchema = true;
            }
            Assert.IsTrue(mustFailOnNoSchema);
        }

        [Test]
        public async System.Threading.Tasks.Task TestEmptyXML()
        {
            var mustFailOnNoRoot = false;
            try
            {
                var content = string.Empty;
                Mock<IAsyncStringReader> m = SetupReader(content);
                var p = new MetadataFromDynamicsService(m.Object);
                var n = await p.GetTables();
            }
            catch (System.Xml.XmlException ex) when (ex.Message == "Root element is missing.")
            {
                mustFailOnNoRoot = true;
            }
            Assert.IsTrue(mustFailOnNoRoot);
        }
        private static Mock<IAsyncStringReader> SetupReader(string content)
        {
            var m = new Mock<IAsyncStringReader>();
            m.Setup(m => m.GetContentAsync()).Returns(Task.FromResult(content));
            return m;
        }
    }
}
