// <copyright file="BookmarksController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    using PTASDynamicsCrudHelperClasses.Classes;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller for handling bookmarks in dynamics.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class BookmarksController : CommonAPIController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksController"/> class.
        /// </summary>
        /// <param name="wrapper">CRM.</param>
        public BookmarksController(CRMWrapper wrapper)
            : base(wrapper)
        {
        }

        /// <summary>
        /// Get the bookmarks a given parcel by Guid.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <param name="major">Major.</param>
        /// <param name="minor">Minor.</param>
        [SwaggerOperation(OperationId = "ForParcelByMajorAndMinor")]
        [HttpGet("{major}/{minor}")]
        public async Task<ActionResult> BookmarksForParcelbyMajorAndMinor(string major, string minor)
        {
            try
            {
                var dynamicsParcels = (await this.Wrapper.ExecuteGet<dynamic>("ptas_parceldetails", $"$filter=statecode  eq 0 and statuscode eq 1 and ptas_major eq '{major}' and ptas_minor eq '{minor}'")).ToArray();

                var parcels = dynamicsParcels.Select(r => $"{r.ptas_parceldetailid}").ToList();
                var bookmarks = await Task.WhenAll(parcels.Select(this.GetBookmarksForParcelId));

                // save parcel.
                return new OkObjectResult(new
                {
                    result = bookmarks.SelectMany(_ => _),
                });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Get the bookmarks a given parcel by Guid.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <param name="parcelId">Parcel Id.</param>
        [SwaggerOperation(OperationId = "ForParcelId")]
        [HttpGet("{parcelId}")]
        public async Task<ActionResult> BookmarksForParcelId(string parcelId)
        {
            try
            {
                var results = await this.GetBookmarksForParcelId(parcelId);
                return new OkObjectResult(new
                {
                    count = results.Length,
                    results,
                });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        private async Task<Bookmark[]> GetBookmarksForParcelId(string parcelId)
        {
            return await this.Wrapper.ExecuteGet<Bookmark>("ptas_bookmarks", $"$filter=statecode  eq 0 and statuscode eq 1 and _ptas_parceldetailid_value eq {parcelId}");
        }

        private class Bookmark
        {
            [JsonProperty("ptas_bookmarkdate")]
            public DateTime Ptas_bookmarkdate { get; set; }

            [JsonProperty("ptas_bookmarkid")]
            public string Ptas_bookmarkid { get; set; }

            [JsonProperty("ptas_bookmarknote")]
            public string Ptas_bookmarknote { get; set; }

            [JsonProperty("ptas_bookmarktype")]
            public int Ptas_bookmarktype { get; set; }

            [JsonProperty("ptas_name")]
            public string Ptas_name { get; set; }

            [JsonProperty("ptas_tags")]
            public string Ptas_tags { get; set; }

            [JsonProperty("_ptas_tag1_value")]
            public string Ptas_tag1_value { get; set; }

            [JsonProperty("_ptas_tag2_value")]
            public string Ptas_tag2_value { get; set; }

            [JsonProperty("_ptas_tag3_value")]
            public string Ptas_tag3_value { get; set; }

            [JsonProperty("_ptas_tag4_value")]
            public string Ptas_tag4_value { get; set; }

            [JsonProperty("_ptas_tag5_value")]
            public string Ptas_tag5_value { get; set; }
        }
    }
}