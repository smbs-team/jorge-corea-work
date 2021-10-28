namespace PTASODataLibraryUnitTest.Helper
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Creates fakes requests.
    /// </summary>
    public static class RequestFactory
    {
        private const string DefaultTestHost = "ptas-dev-odataservices.azurewebsites.net";
        private const string DefaultApiPath = "/V1.0/API/";
        private const string GetVerb = "GET";
        private const string HttpScheme = "http";

        /// <summary>
        /// Creates the fake OData get request.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public static HttpRequest CreateFakeODataRequest(string resourceName, string queryString, object body = null)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = RequestFactory.GetVerb;
            httpContext.Request.Scheme = RequestFactory.HttpScheme;
            httpContext.Request.Host = new HostString(RequestFactory.DefaultTestHost);
            httpContext.Request.Path = new PathString(DefaultApiPath + resourceName);
            httpContext.Request.QueryString = new QueryString(queryString);
             
            if (body != null)
            {
                if (!(body is Stream bodyStream))
                {
                    bodyStream = new MemoryStream();
                    using (var streamWriter = new StreamWriter(stream: bodyStream, encoding: Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(jsonWriter, body);
                        streamWriter.Flush();
                        bodyStream.Seek(0, SeekOrigin.Begin);
                    }
                }

                httpContext.Request.Body = bodyStream;
            }


            return httpContext.Request;
        }
    }
}
