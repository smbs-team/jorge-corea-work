using System;
using System.Collections.Generic;
using System.Text;
using D2SSyncHelpers.Services;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Moq.Protected;
using System.Threading.Tasks;

namespace DynamicsToSQLBlazorTests
{

    class DynamicsMetadataReaderTest
    {
        private const string dummy = "dummy";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstructor()
        {
            // arrange
            var failedOnNoConfig = false;
            try
            {
                // act
                var t = new D2SSyncHelpers.Services.DynamicsMetadataReader(null);

            }
            catch (ArgumentException ex) when (ex.ParamName == "config")
            {
                failedOnNoConfig = true;
            }
            // assert
            Assert.IsTrue(failedOnNoConfig, "Missing params not detected.");
        }

        [Test]
        public async Task TestSucceedConstructionAsync()
        {
            // arrange
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.SetupGet(p => p[It.IsAny<string>()]).Returns("foo");
            var t = new Mock<DynamicsMetadataReader>(mockConfig.Object);
            t.Protected().Setup<Task<string>>("LoadFromClient").Returns(Task.FromResult(dummy));
            // act
            var result = await t.Object.GetContentAsync();
            // assert
            Assert.AreEqual(dummy, result, "Result should match with returned value.");
        }
    }
}
