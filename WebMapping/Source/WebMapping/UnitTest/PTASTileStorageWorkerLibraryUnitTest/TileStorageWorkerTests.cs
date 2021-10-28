using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Moq;
using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
using PTASServicesCommon.CloudStorage;
using PTASTileStorageWorkerLibrary.SqlServer.Model;
using PTASTileStorageWorkerLibrary.SystemProcess;
using PTASTileStorageWorkerLibrary.TileStorage;
using PTASWebMappingUnitTestCommon;
using System.Data.SqlClient;
using System.Diagnostics;

namespace PTASTileStorageWorkerLibraryUnitTest
{
    [TestClass]
    public class TileStorageWorkerTests
    {
        private const string LoggerParameterName = "logger";
        private const string StorageProviderParameterName = "storageProvider";
        private const string SignatureProviderParameterName = "blobSasProvider";
        private const string ConfigParameterName = "configuration";
        private const string DbContextParameterName = "tileStorageDbContext";
        private const string ProcessFactoryParameterName = "processFactory";

        private const string MockSignature = "mockSignature";
        private const string MockSasToken = "token";
        private const string MockBlobAccountName = "AccountName";
        private const string MockEndpointSuffix = "Suffix";

        #region Constructor Tests

        // <summary>
        // Tests that the constructor throws a null exception when the logger parameter is null.
        // </summary>
        [TestMethod]
        public void Test_Constructor_LoggerNull()
        {
            //Arrange
            bool exceptionThrown = false;
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("local.settings.json", true, true)
               .Build();
            TileStorageJobDbContext dbContext = TileStorageJobDbContextMockFactory.CreateMockDbContext((TileStorageJobQueue)null).Object;
            Mock<ICloudStorageProvider> storageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(null);
            Mock<ICloudStorageSharedSignatureProvider> signatureProvider = WebMappingMockFactory.CreateMockSharedSignatureProvider(TileStorageWorkerTests.MockSignature);
            Mock<IProcessFactory> processFactory = WebMappingMockFactory.CreateMockProcessFactory();

            //Act
            try
            {
                TileStorageWorker storageWorker = new TileStorageWorker(dbContext, storageProvider.Object, signatureProvider.Object, processFactory.Object, config, null);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == TileStorageWorkerTests.LoggerParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        // <summary>
        // Tests that the constructor throws a null exception when the storage provider parameter is null.
        // </summary>
        [TestMethod]
        public void Test_Constructor_StorageProviderNull()
        {
            //Arrange
            bool exceptionThrown = false;
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("local.settings.json", true, true)
               .Build();
            TileStorageJobDbContext dbContext = TileStorageJobDbContextMockFactory.CreateMockDbContext((TileStorageJobQueue)null).Object;            
            Mock<ICloudStorageSharedSignatureProvider> signatureProvider = WebMappingMockFactory.CreateMockSharedSignatureProvider(TileStorageWorkerTests.MockSignature);
            Mock<IProcessFactory> processFactory = WebMappingMockFactory.CreateMockProcessFactory();
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;

            //Act
            try
            {
                TileStorageWorker storageWorker = new TileStorageWorker(dbContext, null, signatureProvider.Object, processFactory.Object, config, logger);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == TileStorageWorkerTests.StorageProviderParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        // <summary>
        // Tests that the constructor throws a null exception when the signature provider parameter is null.
        // </summary>
        [TestMethod]
        public void Test_Constructor_SignatureProviderNull()
        {
            //Arrange
            bool exceptionThrown = false;
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("local.settings.json", true, true)
               .Build();
            TileStorageJobDbContext dbContext = TileStorageJobDbContextMockFactory.CreateMockDbContext((TileStorageJobQueue)null).Object;
            Mock<ICloudStorageProvider> storageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(null);
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
            Mock<IProcessFactory> processFactory = WebMappingMockFactory.CreateMockProcessFactory();

            //Act
            try
            {
                TileStorageWorker storageWorker = new TileStorageWorker(dbContext, storageProvider.Object, null, processFactory.Object, config, logger);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == TileStorageWorkerTests.SignatureProviderParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        // <summary>
        // Tests that the constructor throws a null exception when the config parameter is null.
        // </summary>
        [TestMethod]
        public void Test_Constructor_ConfigNull()
        {
            //Arrange
            bool exceptionThrown = false;

            TileStorageJobDbContext dbContext = TileStorageJobDbContextMockFactory.CreateMockDbContext((TileStorageJobQueue)null).Object;
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
            Mock<ICloudStorageProvider> storageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(null);
            Mock<ICloudStorageSharedSignatureProvider> signatureProvider = WebMappingMockFactory.CreateMockSharedSignatureProvider(TileStorageWorkerTests.MockSignature);
            Mock<IProcessFactory> processFactory = WebMappingMockFactory.CreateMockProcessFactory();

            //Act
            try
            {
                TileStorageWorker storageWorker = new TileStorageWorker(dbContext, storageProvider.Object, signatureProvider.Object, processFactory.Object, null, logger);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == TileStorageWorkerTests.ConfigParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        // <summary>
        // Tests that the constructor throws a null exception when the dbcontext parameter is null.
        // </summary>
        [TestMethod]
        public void Test_Constructor_DbContextNull()
        {
            //Arrange
            bool exceptionThrown = false;

            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
            IConfiguration config = new ConfigurationBuilder()
              .AddJsonFile("local.settings.json", true, true)
              .Build();

            Mock<ICloudStorageProvider> storageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(null);
            Mock<ICloudStorageSharedSignatureProvider> signatureProvider = WebMappingMockFactory.CreateMockSharedSignatureProvider(TileStorageWorkerTests.MockSignature);
            Mock<IProcessFactory> processFactory = WebMappingMockFactory.CreateMockProcessFactory();

            //Act
            try
            {
                TileStorageWorker storageWorker = new TileStorageWorker(null, storageProvider.Object, signatureProvider.Object, processFactory.Object, config, logger);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == TileStorageWorkerTests.DbContextParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        // <summary>
        // Tests that the constructor throws a null exception when the processfactory parameter is null.
        // </summary>
        [TestMethod]
        public void Test_Constructor_ProcessFactoryNull()
        {
            //Arrange
            bool exceptionThrown = false;

            TileStorageJobDbContext dbContext = TileStorageJobDbContextMockFactory.CreateMockDbContext((TileStorageJobQueue)null).Object;
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
            IConfiguration config = new ConfigurationBuilder()
              .AddJsonFile("local.settings.json", true, true)
              .Build();

            Mock<ICloudStorageProvider> storageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(null);
            Mock<ICloudStorageSharedSignatureProvider> signatureProvider = WebMappingMockFactory.CreateMockSharedSignatureProvider(TileStorageWorkerTests.MockSignature);

            //Act
            try
            {
                TileStorageWorker storageWorker = new TileStorageWorker(dbContext, storageProvider.Object, signatureProvider.Object, null, config, logger);
            }
            catch (System.ArgumentNullException argumentException)
            {
                if (argumentException.ParamName == TileStorageWorkerTests.ProcessFactoryParameterName)
                {
                    exceptionThrown = true;
                }
            }

            //Assert
            Assert.IsTrue(exceptionThrown);
        }

        #endregion Constructor Tests

        #region Execute Next Job

        /// <summary>
        /// Tests the execute next job method SqlServerToGpkg path
        /// </summary>
        [TestMethod]
        public void Test_ExecuteNextJob_SqlServerToGpkg()
        {
            ExecuteNextJobTest(StorageConversionType.SqlServerToGpkg);
        }

        /// <summary>
        /// Tests the execute next job method SqlServerToSqlServer path
        /// </summary>
        [TestMethod]
        public void Test_ExecuteNextJob_SqlServerToSqlServer()
        {
            ExecuteNextJobTest(StorageConversionType.SqlServerToSqlServer);
        }

        /// <summary>
        /// Tests the execute next job method BlobFilePassthrough path
        /// </summary>
        [TestMethod]
        public void Test_ExecuteNextJob_BlobFilePassthrough()
        {
            ExecuteNextJobTest(StorageConversionType.BlobFilePassthrough);
        }

        // <summary>
        // Tests the execute next job method when there's a SQL exception
        // </summary>
        [TestMethod]
        public void Test_ExecuteNextJob_SqlException()
        {
            //Arrange           
            ILogger logger = WebMappingMockFactory.CreateMockLogger().Object;
            IConfiguration config = new ConfigurationBuilder()
              .AddJsonFile("local.settings.json", true, true)
              .Build();

            SqlException sqlException = WebMappingMockFactory.InstantiateUnitialized<SqlException>();
            Mock<TileStorageJobDbContext> dbContext = TileStorageJobDbContextMockFactory.CreateMockDbContext(sqlException);
            Mock<ICloudStorageProvider> storageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(null);
            Mock<ICloudStorageSharedSignatureProvider> signatureProvider = WebMappingMockFactory.CreateMockSharedSignatureProvider(TileStorageWorkerTests.MockSignature);
            Mock<IProcessFactory> processFactory = WebMappingMockFactory.CreateMockProcessFactory();

            TileStorageWorker storageWorker = new TileStorageWorker(dbContext.Object, storageProvider.Object, signatureProvider.Object, processFactory.Object, config, logger);

            //Act
            bool nextJob = storageWorker.ExecuteNextJob();

            //Assert
            Assert.IsFalse(nextJob);
            dbContext.Verify(m => m.PopNextStorageJob(), Times.Once);
            dbContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        #endregion Execute Next Job

        private void ExecuteNextJobTest(StorageConversionType conversionType)
        {
            //Arrange
            ILogger logger = WebMappingMockFactory.CreateConsoleLogger();
            IConfiguration config = new ConfigurationBuilder()
              .AddJsonFile("local.settings.json", true, true)
              .Build();

            TileStorageJobQueue job = new TileStorageJobQueue
            {
                JobId = 0,
                JobType = TileStorageJobDbContextMockFactory.CreateMockJobType(0, conversionType)
            };

            Mock<DbSet<TileStorageJobQueue>> jobQueueDbSet;
            Mock<TileStorageJobDbContext> dbContext = TileStorageJobDbContextMockFactory.CreateMockDbContext(job, out jobQueueDbSet);
            Mock<ICloudStorageProvider> storageProvider = WebMappingMockFactory.CreateMockCloudStorageProvider(null);
            Mock<ICloudStorageSharedSignatureProvider> signatureProvider = WebMappingMockFactory.CreateMockSharedSignatureProvider(TileStorageWorkerTests.MockSignature);

            if (conversionType == StorageConversionType.BlobFilePassthrough)
            {
                StorageCredentials credentials = new StorageCredentials(TileStorageWorkerTests.MockSasToken);
                CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(credentials, TileStorageWorkerTests.MockBlobAccountName, TileStorageWorkerTests.MockEndpointSuffix, false);
                storageProvider.Setup(m => m.GetCloudStorageAccount()).Returns(cloudStorageAccount);
                signatureProvider.Setup(m => m.GetSharedSignature(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(TileStorageWorkerTests.MockSasToken);                
            }

            Mock<IProcess> processMock = WebMappingMockFactory.CreateMockProcess();
            Mock<IProcessFactory> processFactory = WebMappingMockFactory.CreateMockProcessFactory(processMock);

            TileStorageWorker storageWorker = new TileStorageWorker(dbContext.Object, storageProvider.Object, signatureProvider.Object, processFactory.Object, config, logger);

            //Act
            bool nextJob = storageWorker.ExecuteNextJob();

            //Assert
            Assert.IsTrue(nextJob);
            dbContext.Verify(m => m.PopNextStorageJob(), Times.Once);
            dbContext.Verify(m => m.SaveChanges(), Times.Once);
            jobQueueDbSet.Verify(m => m.Remove(It.Is<TileStorageJobQueue>(j => j == job)), Times.Once);

            processFactory.Verify(m => m.CreateProcess(It.IsAny<ProcessStartInfo>()), Times.Once);
            processMock.Verify(m => m.Start(), Times.Once);
            processMock.Verify(m => m.WaitForExit(), Times.Once);

            if (conversionType == StorageConversionType.BlobFilePassthrough)
            {
                storageProvider.Verify(m => m.GetCloudStorageAccount(), Times.Once);
                signatureProvider.Verify(m => m.GetSharedSignature(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            }
        }
    }
}
