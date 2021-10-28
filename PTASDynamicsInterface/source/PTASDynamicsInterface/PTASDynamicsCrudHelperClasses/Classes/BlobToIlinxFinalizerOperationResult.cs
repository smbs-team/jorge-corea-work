// <copyright file="BlobToIlinxFinalizerOperationResult.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    /// <summary>
    /// This class defines the return result of BlobToIlinxFinalizerController.
    /// </summary>
    public class BlobToIlinxFinalizerOperationResult
    {
        /// <summary>
        /// Gets or sets "OK" or "Error" depending on the execution.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets "Error" if the executions throws an error, empty string if not.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets 0 value If result == "Error", or the lenght of array returning result.
        /// </summary>
        public int Count { get; set; }
    }
}