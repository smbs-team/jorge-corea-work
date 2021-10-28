namespace PTASODataLibrary.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.OData;

    /// <summary>
    /// Wrapper for IODataRequestMessage and IODataResponseMessage.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ODataMessageWrapper : IODataRequestMessageAsync, IODataResponseMessageAsync, IODataPayloadUriConverter, IContainerProvider, IDisposable
    {
        private static readonly Regex ContentIdReferencePattern = new Regex(@"\$\d", RegexOptions.Compiled);

        private Stream stream;
        private Dictionary<string, string> headers;
        private IDictionary<string, string> contentIdMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataMessageWrapper"/> class.
        /// </summary>
        internal ODataMessageWrapper()
            : this(stream: null, headers: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataMessageWrapper"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        internal ODataMessageWrapper(Stream stream)
            : this(stream: stream, headers: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataMessageWrapper"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="headers">The headers.</param>
        internal ODataMessageWrapper(Stream stream, Dictionary<string, string> headers)
            : this(stream: stream, headers: headers, contentIdMapping: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataMessageWrapper"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="contentIdMapping">The content identifier mapping.</param>
        internal ODataMessageWrapper(Stream stream, Dictionary<string, string> headers, IDictionary<string, string> contentIdMapping)
        {
            this.stream = stream;
            if (headers != null)
            {
                this.headers = headers;
            }
            else
            {
                this.headers = new Dictionary<string, string>();
            }

            this.contentIdMapping = contentIdMapping ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets an enumerable over all the headers for this message.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> IODataRequestMessage.Headers
        {
            get
            {
                return this.headers;
            }
        }

        /// <summary>
        /// Gets an enumerable over all the headers for this message.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> IODataResponseMessage.Headers
        {
            get
            {
                return this.headers;
            }
        }

        /// <summary>
        /// Gets or sets the HTTP method used for this request message.
        /// </summary>
        string IODataRequestMessage.Method
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the request URL for this request message.
        /// </summary>
        Uri IODataRequestMessage.Url
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the result status code of the response message.
        /// </summary>
        int IODataResponseMessage.StatusCode
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a container which implements <see cref="T:System.IServiceProvider" /> and contains
        /// all the services registered.
        /// </summary>
        IServiceProvider IContainerProvider.Container
        {
            get { return this.Container; }
        }

        /// <summary>
        /// Gets or sets a container which implements <see cref="T:System.IServiceProvider" /> and contains
        /// all the services registered.
        /// </summary>
        internal IServiceProvider Container { get; set; }

        /// <summary>
        /// Returns a value of an HTTP header.
        /// </summary>
        /// <param name="headerName">The name of the header to get.</param>
        /// <returns>
        /// The value of the HTTP header, or null if no such header was present on the message.
        /// </returns>
        string IODataRequestMessage.GetHeader(string headerName)
        {
            string value;
            if (this.headers.TryGetValue(headerName, out value))
            {
                return value;
            }

            // try case-insensitive
            foreach (string key in this.headers.Keys)
            {
                if (key.Equals(headerName, StringComparison.OrdinalIgnoreCase))
                {
                    return this.headers[key];
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a value of an HTTP header.
        /// </summary>
        /// <param name="headerName">The name of the header to get.</param>
        /// <returns>
        /// The value of the HTTP header, or null if no such header was present on the message.
        /// </returns>
        string IODataResponseMessage.GetHeader(string headerName)
        {
            string value;
            if (this.headers.TryGetValue(headerName, out value))
            {
                return value;
            }

            // try case-insensitive
            foreach (string key in this.headers.Keys)
            {
                if (key.Equals(headerName, StringComparison.OrdinalIgnoreCase))
                {
                    return this.headers[key];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the stream backing for this message.
        /// </summary>
        /// <returns>
        /// The stream backing for this message.
        /// </returns>
        Stream IODataRequestMessage.GetStream()
        {
            return this.stream;
        }

        /// <summary>
        /// Gets the stream backing for this message.
        /// </summary>
        /// <returns>
        /// The stream backing for this message.
        /// </returns>
        Stream IODataResponseMessage.GetStream()
        {
            return this.stream;
        }

        /// <summary>
        /// Asynchronously get the stream backing for this message.
        /// </summary>
        /// <returns>
        /// The stream for this message.
        /// </returns>
        Task<Stream> IODataResponseMessageAsync.GetStreamAsync()
        {
            TaskCompletionSource<Stream> taskCompletionSource = new TaskCompletionSource<Stream>();
            taskCompletionSource.SetResult(this.stream);
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Asynchronously get the stream backing for this message.
        /// </summary>
        /// <returns>
        /// The stream for this message.
        /// </returns>
        Task<Stream> IODataRequestMessageAsync.GetStreamAsync()
        {
            TaskCompletionSource<Stream> taskCompletionSource = new TaskCompletionSource<Stream>();
            taskCompletionSource.SetResult(this.stream);
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Sets the value of an HTTP header.
        /// </summary>
        /// <param name="headerName">The name of the header to set.</param>
        /// <param name="headerValue">The value of the HTTP header or 'null' if the header should be removed.</param>
        void IODataRequestMessage.SetHeader(string headerName, string headerValue)
        {
            this.headers[headerName] = headerValue;
        }

        /// <summary>
        /// Sets the value of an HTTP header.
        /// </summary>
        /// <param name="headerName">The name of the header to set.</param>
        /// <param name="headerValue">The value of the HTTP header or 'null' if the header should be removed.</param>
        void IODataResponseMessage.SetHeader(string headerName, string headerValue)
        {
            this.headers[headerName] = headerValue;
        }

        /// <summary>
        /// Implements a custom URL conversion scheme. This method returns null if no custom conversion is desired. If the method returns a non-null URL that value will be used without further validation.
        /// </summary>
        /// <param name="baseUri">The (optional) base URI to use for the conversion.</param>
        /// <param name="payloadUri">The URI read from the payload.</param>
        /// <returns>
        /// An instance that reflects the custom conversion of the method arguments into a URL or null if no custom conversion is desired; in that case the default conversion is used.
        /// </returns>
        Uri IODataPayloadUriConverter.ConvertPayloadUri(Uri baseUri, Uri payloadUri)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        /// <inheritdoc/>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.stream != null)
                {
                    this.stream.Dispose();
                }
            }
        }
    }
}