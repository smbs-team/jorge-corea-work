// <copyright file="ReceivedFileInfo.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
    /// <summary>
    /// File received through http post.
    /// </summary>
    public class ReceivedFileInfo
    {
        /// <summary>
        /// Gets or sets file bits.
        /// </summary>
        public byte[] FileBits { get; set; }

        /// <summary>
        ///  Gets or sets file extension.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets file Name.
        /// </summary>
        public string FileName { get; set; }
    }
}