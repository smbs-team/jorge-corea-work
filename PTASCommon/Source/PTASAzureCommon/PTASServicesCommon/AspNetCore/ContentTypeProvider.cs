namespace PTASServicesCommon.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.StaticFiles;

    /// <summary>
    /// Helps transform file names to mime types.
    /// </summary>
    public static class ContentTypeProvider
    {
        /// <summary>
        /// Gets the MIME type for a filename.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A mime type.</returns>
        public static string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
