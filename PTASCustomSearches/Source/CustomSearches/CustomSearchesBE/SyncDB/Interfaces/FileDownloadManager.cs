using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using PTASDBFramework;
using PTASDBFramework.Interfaces;
using PTASDBFramework.Sync;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using static SyncDataBase.SyncDB;

namespace SyncDataBase.Interfaces
{
    public class FileDownloadManager : IPTASFileDownloadManager
    {

        /// <summary>
        /// Storage to handle the queue
        /// </summary>
        private PTASDBContext _fileMgrContext;

        /// <summary>
        /// Singletone instance
        /// </summary>
        private static FileDownloadManager _sharedInstance = null;

        /// <summary>
        /// Max number of downloads at the same time
        /// </summary>
        private const int _numOfMaxOpenDownloads = 5;

        private readonly PTASEnvironment environment;
        /// <summary>
        /// Delegate to call back
        /// </summary>
        private IPTASFileDownloadCallback _delegate = null;

        private FileDownloadManager()
        {
            this.loadURLSession();
            string pathFileShare = SyncDB.pathFileShare;
            //Creates the database
            var xmlStream = typeof(PTASDBContext).GetTypeInfo().Assembly.GetManifestResourceStream("PTASDBFramework.Sync.FileDownloadModel.xml");
            String fileDBPath = Path.Combine(pathFileShare, "FileDownload" + SyncDB.area + ".sqlite");
            _fileMgrContext = new PTASDBContext(xmlStream, fileDBPath);
        }

        public static FileDownloadManager sharedInstance
        {
            get
            {
                if (_sharedInstance == null)
                {
                    _sharedInstance = new FileDownloadManager();
                }
                return _sharedInstance;
            }
        }

        public void setDelegate(IPTASFileDownloadCallback newDelegate)
        {
            _delegate = newDelegate;
        }

        public void loadURLSession()
        {
        }

        public string refreshTokenAsync(string token, string client, string clientSecret)
        {
            try
            {
                ClientCredential principalCredentials = new ClientCredential(client, clientSecret);
                var authContext = new AuthenticationContext(Auth.AuthenticationParameters.Authority(environment));
                if (authContext.TokenCache.ReadItems().Any())
                {
                    authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);
                }
                var uri = new Uri(Auth.AuthenticationParameters.ReturnUri(environment));
                var authResult = Task.Run(async () => await authContext.AcquireTokenAsync(
                        Auth.AuthenticationParameters.ScopeResourceUri(environment),
                        principalCredentials)).Result;

                return authResult.AccessToken;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return null;
            }

        }

        public async Task startNewBlobDownload(string blobGuid, string fileDownloadURL, string contextId, string token, string filePath = null)
        {
            PTASDBContext context = _fileMgrContext.copyContext();
            PTASDBEntity fileInfo = context.newEntity("FileDownloadInfo_mb");
            fileInfo["name"] = blobGuid;
            fileInfo["url"] = fileDownloadURL;
            fileInfo["downloadProgress"] = 0.0;
            fileInfo["isDownloading"] = false;
            fileInfo["downloadComplete"] = false;
            fileInfo["downloadType"] = (long)enumFileDownloadType.downloadBlob;
            fileInfo["createdDate"] = DateTime.Now;
            fileInfo["contextId"] = contextId;
            fileInfo["param1"] = string.Empty;

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient httpClient = new HttpClient(clientHandler))
            {
                httpClient.Timeout = new TimeSpan(0, 0, 240);
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, fileDownloadURL))
                {
                    using (HttpResponseMessage response = await httpClient.SendAsync(request))
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Uri uri = new Uri(fileDownloadURL.Replace("-med.JPG", ".JPG"));
                            filePath = filePath.Replace("-med.JPG", ".JPG");
                            fileInfo["url"] = uri.ToString();
                        }
                    }
                }
            }

            //string error;
            context.saveChanges();
            await startDownloadsFromQueueAsync(token, filePath);
        }

        public async Task startNewSketchDownload(string blobGuid, string fileDownloadURL, string token, string contextId)
        {
            PTASDBContext context = _fileMgrContext.copyContext();
            PTASDBEntity fileInfo = context.newEntity("FileDownloadInfo_mb");
            fileInfo["name"] = blobGuid;
            fileInfo["url"] = fileDownloadURL;
            fileInfo["downloadProgress"] = 0.0;
            fileInfo["isDownloading"] = false;
            fileInfo["downloadComplete"] = false;
            fileInfo["downloadType"] = (long)enumFileDownloadType.downloadBlob;
            fileInfo["createdDate"] = DateTime.Now;
            fileInfo["contextId"] = contextId;
            fileInfo["param1"] = token;
            //string error;
            context.saveChanges();
            await startDownloadsFromQueueAsync(token);
        }

        private async Task startDownloadsFromQueueAsync(string token, string filePath = null)
        {
            string myErr;
            PTASDBContext currentContext = _fileMgrContext.copyContext();
            //Gets the number of tasks in progress
            string whereStr = string.Format("isDownloading != {0} and downloadComplete == {1}", 1, 0);
            int numTaskInProgress = (int)currentContext.query("FileDownloadInfo_mb").where(whereStr).count();

            int numTaskToAdd = _numOfMaxOpenDownloads - numTaskInProgress;
            //Gets the pending tasks
            whereStr = string.Format("isDownloading == {0} and downloadComplete == {1}", 0, 0);
            List<PTASDBEntity> pendingQuueTaskList = currentContext.query("FileDownloadInfo_mb").where(whereStr).orderBy("createdDate").limit(numTaskToAdd).toList();

            //Adds pending download task in to the queue
            foreach (var currentTask in pendingQuueTaskList)
            {
                string fileDownloadURL = currentTask.getStringValue("url");
                Uri baseUri = new Uri(fileDownloadURL);
                currentTask["isDownloading"] = 1;
                currentContext.saveChanges(false, false, out myErr);
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (HttpClient httpClient = new HttpClient(clientHandler))
                {
                    httpClient.Timeout = new TimeSpan(0, 0, 240);
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri))
                    {
                        if (!string.IsNullOrEmpty(token))
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            httpClient.DefaultRequestHeaders.Accept.Clear();
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        }

                        using (HttpResponseMessage response = await httpClient.SendAsync(request))
                        {
                            string blobGuid = currentTask.getStringValue("name");
                            string contextID = currentTask.getStringValue("contextId");
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                currentTask["isDownloading"] = false;
                                currentTask["downloadComplete"] = true;
                                currentContext.saveChanges(false, false, out myErr);
                                //Download Content
                                byte[] content = await httpClient.GetByteArrayAsync(request.RequestUri);

                                if (request.RequestUri.ToString().Contains("media"))
                                {
                                    string location_mb = filePath;
                                    this._delegate.didFinishBlobDownloading(content, blobGuid, contextID, true, location_mb);
                                    return;
                                }
                                this._delegate.didFinishBlobDownloading(content, blobGuid, contextID);
                            }
                        }
                    }
                }
            }
        }

        public void startNewBlobDownloadWithGuid(string blobGuid, string fileDownloadURL, string contextId)
        {
            throw new NotImplementedException();
        }

        public void startNewSketchDownloadWithGuid(string blobGuid, string fileDownloadURL, string token, string contextId)
        {
            throw new NotImplementedException();
        }
    }
}