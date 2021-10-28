using BaseWorkerLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomSearchesWorkerLibrary.RScript
{
    /// <summary>
    /// Payload for RScript jobs.
    /// </summary>
    public class RScriptJobPayload 
    {
        /// <summary>
        /// The dataset that the RScript job will be executed for.
        /// </summary>
        public int DatasetPostProcessId;
    }
}
