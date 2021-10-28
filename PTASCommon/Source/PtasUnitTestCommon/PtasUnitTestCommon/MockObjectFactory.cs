namespace PtasUnitTestCommon
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using PTASServicesCommon.TokenProvider;
    using Serilog;
    using Serilog.Extensions.Logging;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    /// <summary>
    /// Helper class to create mock objects.
    /// </summary>
    public class MockObjectFactory
    {
        public const string DefaultToken = "DefaultToken";
        public static ILogger DefaultLogger = null;

        public static Mock<IServiceTokenProvider> CreateMockTokenProvider(string token = null)
        {
            token = token ?? MockObjectFactory.DefaultToken;

            Mock<IServiceTokenProvider> toReturn = new Mock<IServiceTokenProvider>(MockBehavior.Loose);
            toReturn.Setup(m => m.GetAccessTokenAsync(It.IsAny<string>())).ReturnsAsync(token);
            return toReturn;
        }

        public static Mock<ILogger> CreateMockLogger()
        {
            // Logger is created without strict mock behavior because extension methods can't be verified
            Mock<ILogger> toReturn = new Mock<ILogger>();
            return toReturn;
        }
        public static ILogger CreateConsoleLogger()
        {
            if (MockObjectFactory.DefaultLogger == null)
            {
                Log.Logger = new LoggerConfiguration()
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .CreateLogger();

                SerilogLoggerProvider serilogLoggerProvider = new SerilogLoggerProvider(Log.Logger);
                MockObjectFactory.DefaultLogger = serilogLoggerProvider.CreateLogger("ConsoleLogger");
            }

            return MockObjectFactory.DefaultLogger;
        }

        public static T InstantiateUnitialized<T>() where T : class
        {
            return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T)) as T;
        }
    }
}
