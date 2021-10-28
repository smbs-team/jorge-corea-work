// <copyright file="Helpers.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASCRMHelpers.Utilities
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Extra helpers for dynamics crm & batch ops.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Remove Empty Children.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="entityName">Entity to remove the fields from.</param>
        /// <returns>Object with no empty children.</returns>
        public static JToken RemoveEmptyChildren(JToken token, string entityName)
        {
            if (token.Type == JTokenType.Object)
            {
                JObject copy = new JObject();
                foreach (JProperty prop in token.Children<JProperty>())
                {
                    JToken child = prop.Value;
                    if (child.HasValues)
                    {
                        child = RemoveEmptyChildren(child, entityName);
                    }

                    copy.Add(prop.Name, child);
                }

                return copy;
            }
            else if (token.Type == JTokenType.Array)
            {
                JArray copy = new JArray();
                foreach (JToken item in token.Children())
                {
                    JToken child = item;
                    if (child.HasValues)
                    {
                        child = RemoveEmptyChildren(child, entityName);
                    }

                    copy.Add(child);
                }

                return copy;
            }

            return token;
        }

        /// <summary>
        /// Deserialize a composite response.
        /// </summary>
        /// <param name="stream">Stream to deserialize.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> DeserializeToResponse(Stream stream)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            response.Content = new ByteArrayContent(memoryStream.ToArray());
            response.Content.Headers.Add("Content-Type", "application/http;msgtype=response");
            return await response.Content.ReadAsHttpResponseMessageAsync();
        }

        /// <summary>
        /// Create content for a batch send.
        /// </summary>
        /// <param name="tableName">Name of the entity to apply to.</param>
        /// <param name="itemId">Sequential Id of the item.</param>
        /// <param name="contentId">Id of the content.</param>
        /// <param name="content">Content to post.</param>
        /// <param name="cRMUri">Uri of the CRM.</param>
        /// <param name="apiRoute">Route to the api.</param>
        /// <returns>Created message content.</returns>
        public static HttpMessageContent CreateHttpMessageContent(
            string tableName,
            string itemId,
            int contentId,
            string content,
            string cRMUri,
            string apiRoute)
        {
            var requestUri = $"{cRMUri}{apiRoute}{tableName}({itemId})";
            ////var requestUri = $"{cRMUri}{apiRoute}{tableName}";
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri)
            {
                Version = new Version("1.1"),
            };
            HttpMessageContent messageContent = new HttpMessageContent(requestMessage);
            messageContent.Headers.Remove("Content-Type");
            messageContent.Headers.Add("Content-Type", "application/http");
            messageContent.Headers.Add("Content-Transfer-Encoding", "binary");

            if (!string.IsNullOrEmpty(content))
            {
                StringContent stringContent = new StringContent(content);
                stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;type=entry");
                requestMessage.Content = stringContent;
            }

            messageContent.Headers.Add("Content-ID", contentId.ToString());

            return messageContent;
        }

        /// <summary>
        /// Pluralize an entity name.
        /// </summary>
        /// <param name="entityName">Entity name.</param>
        /// <returns>The pluralized name.</returns>
        public static string Pluralize(this string entityName)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                return string.Empty;
            }

            // Strange exception for the rule in dynamics, sketch is pluralized incorrectly as sketchs.
            if (entityName.EndsWith("etch"))
            {
                return entityName + "s";
            }

            // If the singular noun ends in ‑s, -ss, -sh, -ch, -x, or - z, add ‑es to the end to make it plural.
            if (entityName.EndsWith("s") || entityName.EndsWith("ss") || entityName.EndsWith("sh") || entityName.EndsWith("ch") || entityName.EndsWith("x") || entityName.EndsWith("z"))
            {
                return entityName + "es";
            }

            if (entityName.EndsWith("y"))
            {
                return entityName.Substring(0, entityName.Length - 1) + "ies";
            }

            return entityName + "s";
        }
    }
}
