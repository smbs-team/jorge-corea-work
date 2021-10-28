using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using D2SSyncHelpers.Interfaces;
using D2SSyncHelpers.Models;
using D2SSyncHelpers.Services;
using Moq;
using NUnit.Framework;

namespace DynamicsToSQLBlazorTests
{
    public class DBTablesServiceTests
    {
        private const string filler = "filler";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstructor()
        {
            // arrange
            var failedWithDignity = false;
            try
            {
                // act
                var t = new D2SSyncHelpers.Services.DBTablesService(null);

            }
            catch (ArgumentNullException)
            {
                failedWithDignity = true;
            }
            // assert
            Assert.IsTrue(failedWithDignity);
        }



        [Test]
        public async System.Threading.Tasks.Task TestCallToSaveData()
        {
            // arrange
            var m = new Mock<IDataAccessLibrary>();
            var calledLoadData = false;
            m.Setup(x => x.SaveData<object>(It.IsAny<string>(), It.IsAny<object>()))
                .Callback(() =>
                {
                    calledLoadData = true;
                })
                .Returns(Task.FromResult<int>(0));

            var tableService = new D2SSyncHelpers.Services.DBTablesService(m.Object);
            D2SSyncHelpers.Models.DBTable table = new D2SSyncHelpers.Models.DBTable
            {
                Created = default,
                Modified = default,
                Name = filler,
                Fields = new DBField[] { new DBField { DataType = "string", Name = filler } }
            };
            // act
            await tableService.CreateTable(table);
            //assert
            Assert.IsTrue(calledLoadData);
        }
    }
}