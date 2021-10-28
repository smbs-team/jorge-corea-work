using Moq;
using PTASDynamicsCrudHelperClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTASDynamicsCrudCore_UnitTests.HelperClasses
{
    public static class MockClassFiller
    {
        public static void ConfigFiller(Mock<IConfigurationParams> moq)
        {
            moq.SetupGet(s => s.CRMUri).Returns("xx");
            moq.SetupGet(s => s.ClientId).Returns("xx");
            moq.SetupGet(s => s.AuthUri).Returns("xx");
            moq.SetupGet(s => s.ClientSecret).Returns("xx");
            moq.SetupGet(s => s.ApiRoute).Returns("xx");
        }
    }
}
