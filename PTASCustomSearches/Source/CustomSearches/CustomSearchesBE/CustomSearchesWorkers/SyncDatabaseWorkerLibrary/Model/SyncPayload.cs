using System;
using System.Collections.Generic;
using System.Text;

namespace SyncDatabaseWorkerLibrary.Model
{
    public class SyncPayload
    {
        /// <summary>
        /// Gets or sets the area to sync.
        /// </summary>
        public int Area { get; set; }

        /// <summary>
        /// Gets or sets the Blob Server URL
        /// </summary>
        public string BlobServer_URL { get; set; }

        /// <summary>
        /// Gets or sets the Sync Server URL
        /// </summary>
        public string SyncServer_URL { get; set; }

        /// <summary>
        /// Gets or sets the Sketch Service URL
        /// </summary>
        public string PTASSketchServiceURL { get; set; }

        /// <summary>
        /// Gets or sets the Token server URL
        /// </summary>
        public string PTASSAASTokenURL { get; set; }
    }
}
