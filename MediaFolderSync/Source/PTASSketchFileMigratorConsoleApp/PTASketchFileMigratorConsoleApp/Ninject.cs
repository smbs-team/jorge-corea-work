﻿// <auto-generated/>
using Ninject.Modules;
using PTASCRMHelpers;
using PTASExportConnector.SDK;
using PTASketchFileMigratorConsoleApp;
using PTASVCADDAbstractionLibrary;
using System.IO.Abstractions;

namespace PTASSketchFileMigratorConsoleApp
{
    class Ninject : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IBlobUploader>().To<BlobUploader>();
            this.Bind<IDirectoryManager>().To<DirectoryManager>();
            this.Bind<ISettingsManager>().To<SettingsManager>();
            this.Bind<IVCADDManager>().To<VCADDManager>();
            this.Bind<ICloudService>().To<CloudService>();
            this.Bind<IVCADDEngine>().To<VCADDEngine>();
            this.Bind<IFileSystem>().To<FileSystem>();
            this.Bind<IAutomationEngine>().To<AutomationEngineWrapper>();
            this.Bind<IUtility>().To<Utility>();
            this.Bind<IOdata>().To<Odata>();
            this.Bind<ITokenManager>().To<TokenManager>();
            this.Bind<IDataServices>().To<DataServices>();
        }
    }
}
