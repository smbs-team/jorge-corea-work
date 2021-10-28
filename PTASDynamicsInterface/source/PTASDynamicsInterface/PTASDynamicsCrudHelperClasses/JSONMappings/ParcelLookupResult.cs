// <copyright file="ParcelLookupResult.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using Newtonsoft.Json;

    /// <summary>
    /// Smaller data result of parcel to use in lookup lists.
    /// </summary>
    public class ParcelLookupResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParcelLookupResult"/> class.
        /// </summary>
        /// <param name="item">the DynamicsParcelDetail from where get the data.</param>
        /// <param name="source">the source from where get the data.</param>
        /// <param name="condoComplex">Condo complex string.</param>
        public ParcelLookupResult(DynamicsParcelDetail item, string source, string condoComplex = "")
        {
            this.ParcelDetailId = item.ParcelDetailId;
            this.AcctNbr = item.AcctNbr;
            this.Name = item.Name;
            this.Address = item.Address;
            this.StreetType = item.StreetType;
            this.District = item.District;
            this.ZipCode = item.ZipCode;
            this.Source = source;
            this.NamesOnAccount = item.NamesOnAccount;
            this.CondoComplex = condoComplex ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets a value for ParcelDetailId.
        /// ptas_parceldetailid.
        /// </summary>
        [JsonProperty("ptas_parceldetailid")]
        public string ParcelDetailId { get; set; }

        /// <summary>
        ///  Gets or sets a value for AcctNbr.
        ///  ptas_acctnbr.
        /// </summary>
        [JsonProperty("ptas_acctnbr")]
        public string AcctNbr { get; set; }

        /// <summary>
        ///  Gets or sets a value for Name.
        ///  ptas_name.
        /// </summary>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets a value for NamesOnAccount.
        ///  ptas_name.
        /// </summary>
        [JsonProperty("ptas_namesonaccount")]
        public string NamesOnAccount { get; set; }

        /// <summary>
        /// Gets optional condo complex name.
        /// </summary>
        [JsonProperty("ptas_condocomplex_name")]
        public string CondoComplex { get; }

        /// <summary>
        ///  Gets or sets a value for Address.
        ///  ptas_address.
        /// </summary>
        [JsonProperty("ptas_address")]
        public string Address { get; set; }

        /// <summary>
        ///  Gets or sets a value for StreetType.
        ///  ptas_streettype.
        /// </summary>
        [JsonProperty("ptas_streettype")]
        public string StreetType { get; set; }

        /// <summary>
        ///  Gets or sets a value for District.
        ///  ptas_district.
        /// </summary>
        [JsonProperty("ptas_district")]
        public string District { get; set; }

        /// <summary>
        ///  Gets or sets a value for ZipCode.
        ///  ptas_zipcode.
        /// </summary>
        [JsonProperty("ptas_zipcode")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets a value for source of found data.
        /// </summary>
        [JsonProperty("lookup_source")]
        public string Source { get; set; }
    }
}