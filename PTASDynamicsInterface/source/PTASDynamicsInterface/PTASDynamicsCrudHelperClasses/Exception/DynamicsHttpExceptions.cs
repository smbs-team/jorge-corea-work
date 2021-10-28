// <copyright file="DynamicsHttpExceptions.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Exception
{
    using System;
    using System.Net;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// DynamicsHttpExceptions: throw an exception dependind on response after odata invocation.
    /// </summary>
    public static class DynamicsHttpExceptions
    {
        /// <summary>
        /// AddKnownException: thows an exception if the statuscode is known.
        /// </summary>
        /// <param name="response">The last response receives after invoke odata.</param>
        /// <param name="fullRoute">URL Route sent to odata.</param>
        public static void ThrowException(System.Net.Http.HttpResponseMessage response, string fullRoute)
        {
            string errorMessage;
            if (fullRoute == null)
            {
                throw new ArgumentNullException();
            }

            var s = response.Content.ReadAsStringAsync().Result;
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    errorMessage = $"Request unautorized on: {fullRoute}.";
                    break;

                case HttpStatusCode.GatewayTimeout:
                    errorMessage = $"Request time out: {fullRoute}.";
                    break;

                case HttpStatusCode.BadGateway:
                    errorMessage = $"Request went to Bad Gateway: {fullRoute}.";
                    break;

                case HttpStatusCode.BadRequest:
                    errorMessage = $"Bad Request: {fullRoute}";
                    break;

                case HttpStatusCode.MethodNotAllowed:
                    errorMessage = $"Method Not Allowed: {fullRoute}.";
                    break;

                case HttpStatusCode.NotFound:
                    errorMessage = $"Id Not found: {fullRoute}.";
                    break;

                case HttpStatusCode.PreconditionFailed:
                    errorMessage = $"Precondition failed.: {fullRoute}. {s}.";
                    break;

                default:
                    throw new DynamicsInterfaceException($"(Request failed with status: on {fullRoute}. With status: {response.StatusCode}. With Reason: {response.ReasonPhrase}. With Content: {s}).", (int)HttpStatusCode.BadGateway, null);
            }

            var formatted = JValue.Parse(s).ToString(Formatting.Indented);
            throw new DynamicsInterfaceException($"Error message: {errorMessage}<br>Response content: <br><pre>{formatted}</pre>", (int)response.StatusCode, null);
        }
    }
}