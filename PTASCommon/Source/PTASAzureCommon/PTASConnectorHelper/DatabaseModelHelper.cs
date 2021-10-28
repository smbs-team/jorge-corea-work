// <copyright file="DatabaseModelHelper.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace ConnectorService.Utilities
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Xml.Linq;

    /// <summary>Returns the database model.</summary>
    public static class DatabaseModelHelper
    {
        // private static string xmlModelPath = "PTASdb.dbx";
        private static DatabaseModel dbModel = null;
        private static string dbxPath = "https://ptasdevpipetools.blob.core.windows.net/pipetools/TransferFiles/DBXModel/PTASdb.dbx?sv=2020-04-08&st=2021-10-18T19%3A51%3A51Z&se=2221-10-19T19%3A51%3A00Z&sr=b&sp=r&sig=%2Fa%2BzXF6hVK7wPusVNE1no8at%2BXjskXf5U6xaudzAIV4%3D";

        /// <summary>
        /// Gets the database model.
        /// </summary>
        /// <returns>The database model.</returns>
        public static DatabaseModel GetDatabaseModel()
        {
            if (!string.IsNullOrWhiteSpace(dbxPath))
            {
                WebRequest request = WebRequest.Create(dbxPath);
                request.Timeout = 30 * 60 * 1000;
                request.UseDefaultCredentials = true;
                request.Proxy.Credentials = request.Credentials;
                WebResponse response = (WebResponse)request.GetResponse();
                using (Stream s = response.GetResponseStream())
                {
                    using (var dbx = new StreamReader(s))
                    {
                        if (dbx != null)
                        {
                            if (dbModel == null)
                            {
                                XDocument xmlModel = XDocument.Load(dbx);
                                dbModel = new DatabaseModel(xmlModel.Root);
                            }
                        }
                    }
                }
            }

            return dbModel;
        }

        /////// <summary>
        /////// Gets the database model.
        /////// </summary>
        /////// <returns>The database model</returns>
        ////public static DatabaseModel GetDatabaseModel()
        ////{
        ////    var assembly = Assembly.GetExecutingAssembly();

        ////    if (!string.IsNullOrWhiteSpace(xmlModelPath))
        ////    {
        ////        using (var dbx = new StreamReader(assembly.GetManifestResourceStream("PTASConnectorHelper." + xmlModelPath)))
        ////        {
        ////            if (dbx != null)
        ////            {
        ////                if (dbModel == null)
        ////                {
        ////                    XDocument xmlModel = XDocument.Load(dbx);
        ////                    dbModel = new DatabaseModel(xmlModel.Root);
        ////                }
        ////            }
        ////        }
        ////    }

        ////    return dbModel;
        ////}
    }
}
