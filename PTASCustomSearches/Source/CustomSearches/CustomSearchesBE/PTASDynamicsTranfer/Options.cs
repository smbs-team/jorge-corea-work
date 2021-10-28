namespace PTASDynamicsTranfer
{
    public class Options
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// </summary>
        /// <param name="chunkSize">chunkSize.</param>
        /// <param name="connectionString">connectionString.</param>
        /// <param name="authURI">authURI.</param>
        /// <param name="dynamicsURL">dynamicsURL.</param>
        /// <param name="entityName">entityName.</param>
        /// <param name="clientID">clientID.</param>
        /// <param name="clientSecret">clientSecret.</param>
        /// <param name="useBulkInsert">useBulkInsert.</param>
        public Options(int chunkSize, string connectionString, string authURI, string dynamicsURL, string entityName, string clientID, string clientSecret, int useBulkInsert)
        {
            this.ChunkSize = chunkSize;
            this.ConnectionString = connectionString;
            this.AuthUri = authURI;
            this.DynamicsURL = dynamicsURL;
            this.EntityName = entityName;
            this.ClientId = clientID;
            this.ClientSecret = clientSecret;
            this.useBulkInsert = useBulkInsert;
        }

        public int ChunkSize { get; set; }

        public string ConnectionString { get; set; }

        public string AuthUri { get; set; }

        public string DynamicsURL { get; set; }

        public string EntityName { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public int useBulkInsert { get; set; }
    }
}