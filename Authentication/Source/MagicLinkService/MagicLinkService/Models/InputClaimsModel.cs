//-----------------------------------------------------------------------
// <copyright file="InputClaimsModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Input Claims Model.
    /// </summary>
    public class InputClaimsModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string RedirectUrlOverride { get; set; }

        /// <summary>
        /// De-serializes the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>The de-serialized model.</returns>
        public static InputClaimsModel Parse(string json)
        {
            return JsonConvert.DeserializeObject(json, typeof(InputClaimsModel)) as InputClaimsModel;
        }

        /// <summary>
        /// Converts to string (serializes the model).
        /// </summary>
        /// <returns>The serialized model.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
