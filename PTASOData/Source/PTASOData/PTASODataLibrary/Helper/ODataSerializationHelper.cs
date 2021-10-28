namespace PTASODataLibrary.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Xml;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNet.OData.Formatter;
    using Microsoft.AspNet.OData.Formatter.Serialization;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OData;
    using Microsoft.OData.Edm;
    using Microsoft.OData.UriParser;
    using ODataPath = Microsoft.AspNet.OData.Routing.ODataPath;

    /// <summary>
    /// Helper for OData Serialization.
    /// </summary>
    internal class ODataSerializationHelper
    {
        private const string ApiPath = "/V1.0/API/";

        /// <summary>
        /// Creates a new XmlWriterSettings instance using the encoding.
        /// </summary>
        /// <param name="messageWriterSettings">Configuration settings of the OData writer.</param>
        /// <param name="encoding">Encoding to  use in the writer settings.</param>
        /// <returns>The Xml writer settings to use for this writer.</returns>
        internal static XmlWriterSettings CreateXmlWriterSettings(ODataMessageWriterSettings messageWriterSettings, Encoding encoding)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = messageWriterSettings.EnableCharactersCheck;
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.OmitXmlDeclaration = false;
            settings.Encoding = encoding;
            settings.NewLineHandling = NewLineHandling.Entitize;

            // we do not want to close the underlying stream when the OData writer is closed since we don't own the stream.
            settings.CloseOutput = false;

            return settings;
        }

        /// <summary>
        /// Writes to stream.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="model">The model.</param>
        /// <param name="version">The version.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="internalRequest">The internal/simulated request.</param>
        /// <param name="oDataMessageWrapper">The get o data message wrapper.</param>
        internal static void WriteToStream(
            Type type,
            object value,
            IEdmModel model,
            ODataVersion version,
            MediaTypeHeaderValue contentType,
            HttpRequest internalRequest,
            ODataMessageWrapper oDataMessageWrapper)
        {
            string serviceRoot = internalRequest.Host.ToString() + ODataSerializationHelper.ApiPath;
            IServiceProvider serviceProvider = internalRequest.HttpContext.RequestServices;

            ODataResourceSetSerializer serializer = serviceProvider.GetRequiredService<ODataResourceSetSerializer>();

            ODataSerializerContext oDataSerializerContext = new ODataSerializerContext()
            {
                Request = internalRequest,
            };

            ODataFeature requestODataFeature = internalRequest.ODataFeature() as ODataFeature;
            DefaultODataPathHandler pathParser = new DefaultODataPathHandler();

            var builder = new UriBuilder();
            builder.Scheme = internalRequest.Scheme;
            builder.Host = internalRequest.Host.Host;
            builder.Port = internalRequest.Host.Port ?? 80;
            builder.Path = internalRequest.Path;
            var serviceRootUri = builder.Uri;

            string queryString = internalRequest.QueryString.HasValue ? internalRequest.QueryString.ToString() : null;
            string oDataPathAndQuery = (internalRequest.Path.ToString() + queryString).Substring(ODataSerializationHelper.ApiPath.Length);

            ODataPath path = pathParser.Parse(serviceRootUri.ToString(), oDataPathAndQuery, internalRequest.HttpContext.RequestServices);

            IEdmNavigationSource targetNavigationSource = path == null ? null : path.NavigationSource;

            IODataResponseMessage responseMessage = oDataMessageWrapper;

            ODataMessageWriterSettings writerSettings = internalRequest.GetWriterSettings();
            writerSettings.BaseUri = serviceRootUri;
            writerSettings.Version = version;
            writerSettings.Validations = writerSettings.Validations & ~ValidationKinds.ThrowOnUndeclaredPropertyForNonOpenType;

            writerSettings.ODataUri = new ODataUri
            {
                ServiceRoot = serviceRootUri,

                Apply = requestODataFeature.ApplyClause,
                Path = (path == null || IsOperationPath(path)) ? null : path.Path,
            };

            ODataMetadataLevel metadataLevel = ODataMetadataLevel.MinimalMetadata;
            if (contentType != null)
            {
                IEnumerable<KeyValuePair<string, string>> parameters =
                    contentType.Parameters.Select(val => new KeyValuePair<string, string>(val.Name, val.Value));
                metadataLevel = ODataMediaTypes.GetMetadataLevel(contentType.MediaType, parameters);
            }

            using (ODataMessageWriter messageWriter = new ODataMessageWriter(responseMessage, writerSettings, model))
            {
                ODataSerializerContext writeContext = oDataSerializerContext;
                writeContext.NavigationSource = targetNavigationSource;
                writeContext.Model = model;
                writeContext.RootElementName = GetRootElementName(path) ?? "root";
                writeContext.SkipExpensiveAvailabilityChecks = serializer.ODataPayloadKind == ODataPayloadKind.ResourceSet;
                writeContext.Path = path;
                writeContext.MetadataLevel = metadataLevel;
                writeContext.SelectExpandClause = requestODataFeature.SelectExpandClause;

                serializer.WriteObject(value, value.GetType(), messageWriter, writeContext);
            }
        }

        /// <summary>
        /// Creates an Xml writer over the specified stream, with the provided settings and encoding.
        /// </summary>
        /// <param name="stream">The stream to create the XmlWriter over.</param>
        /// <param name="messageWriterSettings">The OData message writer settings used to control the settings of the Xml writer.</param>
        /// <param name="encoding">The encoding used for writing.</param>
        /// <returns>An <see cref="XmlWriter"/> instance configured with the provided settings and encoding.</returns>
        internal static XmlWriter CreateXmlWriter(Stream stream, ODataMessageWriterSettings messageWriterSettings, Encoding encoding)
        {
            XmlWriterSettings xmlWriterSettings = CreateXmlWriterSettings(messageWriterSettings, encoding);
            XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings);
            return writer;
        }

        // This function is used to determine whether an OData path includes operation (import) path segments.
        // We use this function to make sure the value of ODataUri.Path in ODataMessageWriterSettings is null
        // when any path segment is an operation. ODL will try to calculate the context URL if the ODataUri.Path
        // equals to null.
        private static bool IsOperationPath(ODataPath path)
        {
            if (path == null)
            {
                return false;
            }

            foreach (ODataPathSegment segment in path.Segments)
            {
                if (segment is OperationSegment ||
                    segment is OperationImportSegment)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the name of the root element.
        /// </summary>
        /// <param name="path">The path.</param>
        private static string GetRootElementName(ODataPath path)
        {
            if (path != null)
            {
                ODataPathSegment lastSegment = path.Segments.LastOrDefault();
                if (lastSegment != null)
                {
                    OperationSegment actionSegment = lastSegment as OperationSegment;
                    if (actionSegment != null)
                    {
                        IEdmAction action = actionSegment.Operations.Single() as IEdmAction;
                        if (action != null)
                        {
                            return action.Name;
                        }
                    }

                    PropertySegment propertyAccessSegment = lastSegment as PropertySegment;
                    if (propertyAccessSegment != null)
                    {
                        return propertyAccessSegment.Property.Name;
                    }
                }
            }

            return null;
        }
    }
}
