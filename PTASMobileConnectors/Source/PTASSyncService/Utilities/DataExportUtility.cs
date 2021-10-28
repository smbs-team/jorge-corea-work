using PTASConnectorSDK;
using PTASSyncService.Utilities;
using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ConnectorService.Utilities
{
    class DataExportUtility
    {
        private ConnectorSDK connectorSDK;

        private readonly PTASSyncService.Settings Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataExportUtility" /> class.
        /// </summary>
        public DataExportUtility()
        {
            connectorSDK = new ConnectorSDK(Environment.GetEnvironmentVariable("connectionString"), SQLServerType.MSSQL);
            
        }
    
        /// <summary>
        /// Gets the user password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        private string GetUserPassword(string userName)
        {
            Debug.Assert(!string.IsNullOrEmpty(userName), "The user name should not be null or empty");
            string backConnStr = Environment.GetEnvironmentVariable("backendConnectionString");
            Debug.Assert(!string.IsNullOrWhiteSpace(backConnStr), "The cama DB connection string in the config file is wrong");
            object password = null;
            string sqlToken = SQLTokenUtility.GetSQLToken(backConnStr);
            using (SqlConnection cnn = new SqlConnection(backConnStr))
            {
                cnn.AccessToken = sqlToken;
                using (SqlCommand cmd = new SqlCommand("select passwd from dbo.users where id = '" + userName + "'", cnn))
                {
                    cmd.CommandTimeout = cnn.ConnectionTimeout;
                    cnn.Open();
                    password = cmd.ExecuteScalar();
                    cnn.Close();
                }
            }

            if (password != null)
                return password.ToString();
            else
                return null;
        }
    }
}
