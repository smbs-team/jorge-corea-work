// <copyright file="DynamicsHttpRequestException.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASCRMHelpers.Exception
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Exception based on a serialized object.
    /// </summary>
    public class ObjBearingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjBearingException"/> class.
        /// </summary>
        /// <param name="errorObj">Object with error info.</param>
        /// <param name="innerException">Inner exception.</param>
        public ObjBearingException(object errorObj)
            : base(Serialize(errorObj))
        {
        }

        private static string Serialize(object errorObj) => JsonConvert.SerializeObject(errorObj);
    }
}