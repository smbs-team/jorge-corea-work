using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PTASDBFramework;
using PTASDBFramework.Sync;
using SyncDataBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SyncDataBase.Auth;
using System.Linq;
using System.Windows.Threading;
using System.IO;

namespace SyncDataBase
{
    public class SyncDB
    {
        private bool _syncRunning;
        private double _lastSyncStartTime;
        private DateTime _lastSyncEndTime;
        public PTASDBContext dbContext;
        private PTASDBSyncManager _syncMngr = null;
        private long _syncTimeoutInSeconds = 600;
        public event EventHandler syncStartedEvent;
        private readonly PTASEnvironment environment;
        private string dbName = "PTASdb";
        private string deviceName = "Desktop Sync App";
        private string deviceID = "E131DBDF-5C5C-4650-9394-11F7E216BA95";
        public static int area { get; set; }
        public static string pathFileShare { get; set; }
        private string _syncServer_URL = string.Empty;
        private string _blobServer_URL = string.Empty;
        public string PTASSketchServiceURL = string.Empty;
        public string PTASSAASTokenURL = string.Empty; 
                    
        public SyncDB(int areaParameter, string syncServer_URL, string blobServer_URL, 
            string PTASSketchServiceURL, string PTASSAASTokenURL, string path) {
            this.dbName += areaParameter;
            area = areaParameter;
            this._syncServer_URL = syncServer_URL;
            this._blobServer_URL = blobServer_URL;
            this.PTASSketchServiceURL = PTASSketchServiceURL;
            this.PTASSAASTokenURL = PTASSAASTokenURL;
            pathFileShare = path;
        }

        public enum PTASEnvironment
        {
            Production,
            Testing,
            Development
        }


        public async Task createDBContext(string dbName)
        {
            //Needs to be called to allow sqlite pcl sets the provider.
            SQLitePCL.Batteries.Init();

            string uri = "https://ptasdevpipetools.blob.core.windows.net/pipetools/TransferFiles/DBXModel/PTASdb.dbx?sv=2020-04-08&st=2021-10-18T19%3A51%3A51Z&se=2221-10-19T19%3A51%3A00Z&sr=b&sp=r&sig=%2Fa%2BzXF6hVK7wPusVNE1no8at%2BXjskXf5U6xaudzAIV4%3D";
            Stream xmlStream = Stream.Null;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;
                    xmlStream = await content.ReadAsStreamAsync();
                }
            }

            //Path to store the DB
            String dbPath = getDBPath(dbName);
            Console.WriteLine(string.Format("Database path: {0}", dbPath));

            dbContext = new PTASDBContext(xmlStream, dbPath);
        }

        string getDBPath(string dbName)
        {
            //Path to store the DB
            //string currentEvn = synchronizationHelper.sharedInstance.currentEnvironment.ToString();
            //var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            String dbPath = Path.Combine(pathFileShare, dbName + ".sqlite");
            return dbPath;
        }


        public async Task StartSyncAsync(ClientCredential principalCredentials, string emails, string clientSecret)
        {
            await this.createDBContext(dbName);
            if (_syncRunning && DateTime.UtcNow.Second - _lastSyncStartTime >= _syncTimeoutInSeconds)
                _syncRunning = false;
            var token = await GetTokenAsync(principalCredentials);
            bool syncReachable = this.isSyncReachableAsync(token.Item1).Result;
            if (!_syncRunning && syncReachable)
            {
                _syncRunning = true;
                var ReferenceDate = new DateTime(2001, 1, 1);
                DateTime CacheUtcTime = ReferenceDate.AddSeconds(DateTimeOffset.Now.ToUnixTimeSeconds());

                _lastSyncStartTime = CacheUtcTime.Second;

                PTASDBContext myDBContext = dbContext.copyContext();

                if (_syncMngr == null || string.IsNullOrEmpty(_syncMngr.SecurityToken))
                {
                    string ctxKey = "defaultContext";
                    Dictionary<string, PTASDBContext> contextDict = new Dictionary<string, PTASDBContext>();
                    contextDict[ctxKey] = dbContext;
                    _syncMngr = new PTASDBSyncManager(contextDict, ctxKey, _syncServer_URL, _blobServer_URL, token.Item1, this.deviceID, this.deviceName, false, emails, principalCredentials.ClientId, clientSecret, area);
                    _syncMngr.setFileDownloadInstance(FileDownloadManager.sharedInstance);
                    _syncMngr.setPTASImageInstance(new PTASTImage());
                    _syncMngr.setArea(area);
                    _syncMngr.setPTASSketchServiceURL(PTASSketchServiceURL); //REVISAR
                    _syncMngr.setPTASSAASTokenURL(PTASSAASTokenURL); 
                    _syncMngr.syncMessageEvent += SyncMngr_SyncMessageEvent; //MODIFICAR PARA VER SI SE QUITA
                    _syncMngr.syncCompletedEvent += SyncMngr_SyncCompletedEvent;
                    _syncMngr.syncCommandReceived += SyncMngr_SyncCommandReceived;
                }
                else
                    _syncMngr.SecurityToken = token.Item1;

                if (syncStartedEvent != null)
                    syncStartedEvent(this, new EventArgs());
                Console.WriteLine("********** Sync will be called **********");
                await _syncMngr.synchronize(null, null);
                Console.WriteLine("********** Sync In progress **********");
            }
            else if (!syncReachable)
            {
                Console.WriteLine("Error : Sync server is not reachable.");
            }
        }

        void SyncMngr_SyncCommandReceived(object sender, syncCommandReceivedEventArgs e)
        {

        }

        void SyncMngr_SyncCompletedEvent(object sender, EventArgs e)
        {
            Console.WriteLine("********** Sync Completed **********");
            _lastSyncEndTime = DateTime.Now;
        }

        private void SyncMngr_SyncMessageEvent(object sender, EventArgs e)
        {
            string msg = string.Empty;
            syncMessageEventArgs syncEvent = (syncMessageEventArgs)e;
            if (syncEvent.notificationType == notificationTypeValue.progressSync)
            {
                if ((int)syncEvent.totalAdvancePercentage == -1)
                {
                    msg = syncEvent.message;
                }
                else
                {
                    msg = string.Format("{0} {1}% Completed", syncEvent.message, (int)syncEvent.totalAdvancePercentage);
                }
                Console.WriteLine(msg);
            }
        }

        public async Task<bool> isSyncReachableAsync(string securityToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri(_syncServer_URL);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", securityToken);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    HttpResponseMessage response = await client.GetAsync(_syncServer_URL + "/api/Sync/HealthCheck");
                    return response.IsSuccessStatusCode;
                }
            }
            catch { }
            return false;
        }

        private async Task<Tuple<string, string>> GetTokenAsync(ClientCredential principalCredentials)
        {
            try
            {
                var authContext = new AuthenticationContext(Auth.AuthenticationParameters.Authority(environment));
                if (authContext.TokenCache.ReadItems().Any())
                {
                    authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);
                }
                var uri = new Uri(Auth.AuthenticationParameters.ReturnUri(environment));
                var authResult = await authContext.AcquireTokenAsync(
                        Auth.AuthenticationParameters.ScopeResourceUri(environment),
                        principalCredentials);

                #region CODE
                //var uri = new Uri(Auth.AuthenticationParameters.ReturnUri(environment));
                //PlatformParameters p = new PlatformParameters(PromptBehavior.Auto, new CustomWebUi(Dispatcher.CurrentDispatcher));
                //var authResult = await authContext.AcquireTokenAsync(
                //    Auth.AuthenticationParameters.ScopeResourceUri(environment),
                //    Auth.AuthenticationParameters.ApplicationId(environment), uri, p);

                //if (!isValid)
                //{
                //    return null;
                //} 
                #endregion

                return Tuple.Create(authResult.AccessToken, authResult.ExpiresOn.ToString());
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return null;
            }
        }
    }
}
