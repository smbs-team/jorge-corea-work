// <copyright file="GenericDynamicsController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Caching.Memory;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PTASCRMHelpers;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Get media for parcel.
    /// </summary>
    [Route("v1/api/[controller]")]
    public class MediaForParcelController : GenericDynamicsControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaForParcelController"/> class.
        /// </summary>
        /// <param name="config">Configuration.</param>
        /// <param name="memoryCache">Me cache.</param>
        /// <param name="tokenManager">Token Manager.</param>
        public MediaForParcelController(
            IConfigurationParams config,
            IMemoryCache memoryCache,
            ITokenManager tokenManager)
            : base(config, memoryCache, tokenManager)
        {
        }

        /// <summary>
        /// Attempts to get the media id for item.
        /// </summary>
        /// <param name="parcelId">Parcel id to search for.</param>
        /// <returns>ParcelId.</returns>
        [HttpGet("{parcelId}")]
        public async Task<ActionResult> GetMediaId(string parcelId)
        {
            try
            {
                var foundLand = await this.ExecuteQuery(new EntityRequest
                {
                    EntityName = "ptas_parceldetail",
                    Query = $"$top=1&$select=ptas_landid,ptas_propertytypeid&$filter=ptas_parceldetailid eq {parcelId}&$expand=ptas_landid($select=ptas_landid),ptas_propertytypeid($select=ptas_name)",
                });
                JArray returnedObject = (JObject.Parse(foundLand) as dynamic).value;
                dynamic firstLand = returnedObject.FirstOrDefault() as dynamic;
                string foundLandId = firstLand?.ptas_landid?.ptas_landid;
                bool isResidential = "R".Equals(firstLand?.ptas_propertytypeid?.ptas_name);

                var queries = CreateQueries(parcelId, foundLandId, "($filter=ptas_mediatype eq 1 and ptas_posttoweb eq true and ptas_imagetype eq 1)");
                var tasks = queries.Select(async q =>
                {
                    var resultStr = await this.ExecuteQuery(q);
                    var resultObj = JsonConvert.DeserializeObject<QueryResult>(resultStr).Value.Where(v => true.Equals(v.Values?.Any()))
                        .Select(v => v.Values);
                    return new
                    {
                        q.EntityName,
                        q.ResidentialOrder,
                        q.NonResidentialOrder,
                        resultObj,
                        ////resultStr,
                    };
                });
                var results =
                        (await Task.WhenAll(tasks))
                            .Where(t => true.Equals(t.resultObj?.Any()))
                            .Select(t =>
                            {
                                return t.resultObj.SelectMany(ro => ro.Select(ri => new
                                {
                                    t.EntityName,
                                    ri.Ptas_mediarepositoryid,
                                    ri.Ptas_primary,
                                    MediaPath =
                                        $"{ri.Ptas_rootfolder}/{ri.Ptas_blobpath}.{ri.Ptas_fileextension ?? "jpg"}"
                                        .Replace("//", "/"),
                                    ri.Ptas_order,
                                    t.ResidentialOrder,
                                    HadExtension = !string.IsNullOrEmpty(ri.Ptas_fileextension),
                                    t.NonResidentialOrder,
                                }));
                            })
                            .SelectMany(itm => itm)
                            .OrderByDescending(itm => itm.Ptas_primary)
                            .ThenBy(itm => isResidential ? itm.ResidentialOrder : itm.NonResidentialOrder)
                            .Select(itm => new
                            {
                                itm.EntityName,
                                MediaId = itm.Ptas_mediarepositoryid,
                                IsPrimary = itm.Ptas_primary,
                                itm.MediaPath,
                                ImageOrder = itm.Ptas_order,
                                IsResidential = isResidential,
                                itm.HadExtension,
                            })
                            .FirstOrDefault();

                return new OkObjectResult(results);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new
                {
                    message = ex.Message,
                });
            }
        }

        private static EntityRecord[] CreateQueries(string parcelId, string foundLandId, string tail)
        {
            return new EntityRecord[]
                            {
                                new EntityRecord
                                {
                                    EntityName = "ptas_buildingdetail",
                                    Query = $"$select=_ptas_parceldetailid_value&$filter=_ptas_parceldetailid_value eq {parcelId}&$expand=ptas_buildingdetail_ptas_mediarepository{tail}",
                                    ResidentialOrder = 1,
                                    NonResidentialOrder = 1,
                                },
                                new EntityRecord
                                {
                                    EntityName = "ptas_condounit",
                                    Query = $"$select=_ptas_parcelid_value&$filter=_ptas_parcelid_value eq {parcelId}&$expand=ptas_condounit_ptas_mediarepository{tail}",
                                    ResidentialOrder = 2,
                                    NonResidentialOrder = 99,
                                },
                                new EntityRecord
                                {
                                    EntityName = "ptas_land",
                                    Query = $"$select=ptas_landid&$filter=ptas_landid eq {foundLandId}&$expand=ptas_land_ptas_mediarepository{tail}",
                                    ResidentialOrder = 3,
                                    NonResidentialOrder = 2,
                                },
                                new EntityRecord
                                {
                                    EntityName = "ptas_condocomplex",
                                    Query = $"$select=_ptas_parcelid_value,&$filter=_ptas_parcelid_value eq {parcelId}&$expand=ptas_condocomplex_ptas_mediarepository{tail}",
                                    ResidentialOrder = 100,
                                    NonResidentialOrder = 100,
                                },
                                new EntityRecord
                                {
                                    EntityName = "ptas_accessorydetail",
                                    Query = $"$select=_ptas_parceldetailid_value&$filter=_ptas_parceldetailid_value eq {parcelId}&$expand=ptas_accessorydetail_ptas_mediarepository{tail}",
                                    ResidentialOrder = 101,
                                    NonResidentialOrder = 101,
                                },
                                new EntityRecord
                                {
                                    EntityName = "ptas_parceldetail",
                                    Query = $"$select=ptas_parceldetailid&$filter=ptas_parceldetailid eq {parcelId}&$expand=ptas_parceldetail_ptas_mediarepository{tail}",
                                    ResidentialOrder = 102,
                                    NonResidentialOrder = 102,
                                },
                            };
        }

        private class InnerInnerQuery
        {
            [JsonProperty("ptas_mediarepositoryid")]
            public string Ptas_mediarepositoryid { get; set; }

            [JsonProperty("ptas_primary")]
            public bool Ptas_primary { get; set; }

            [JsonProperty("ptas_blobpath")]
            public string Ptas_blobpath { get; set; }

            [JsonProperty("ptas_rootfolder")]
            public string Ptas_rootfolder { get; set; }

            [JsonProperty("ptas_order")]
            public int? Ptas_order { get; set; }

            [JsonProperty("ptas_fileextension")]
            public string Ptas_fileextension { get; set; }
        }

        private class InnerQuery
        {
            [JsonProperty("ptas_parceldetail_ptas_mediarepository")]
            public InnerInnerQuery[] Ptas_parceldetail_ptas_mediarepository { get; set; }

            [JsonProperty("ptas_condounit_ptas_mediarepository")]
            public InnerInnerQuery[] Ptas_condounit_ptas_mediarepository { get; set; }

            [JsonProperty("ptas_condocomplex_ptas_mediarepository")]
            public InnerInnerQuery[] Ptas_condocomplex_ptas_mediarepository { get; set; }

            [JsonProperty("ptas_accessorydetail_ptas_mediarepository")]
            public InnerInnerQuery[] Ptas_accessorydetail_ptas_mediarepository { get; set; }

            [JsonProperty("ptas_land_ptas_mediarepository")]
            public InnerInnerQuery[] Ptas_land_ptas_mediarepository { get; set; }

            [JsonProperty("ptas_buildingdetail_ptas_mediarepository")]
            public InnerInnerQuery[] Ptas_buildingdetail_ptas_mediarepository { get; set; }

            public InnerInnerQuery[] Values
            {
                get
                {
                    return this.Ptas_parceldetail_ptas_mediarepository ?? this.Ptas_condounit_ptas_mediarepository ?? this.Ptas_condocomplex_ptas_mediarepository ?? this.Ptas_accessorydetail_ptas_mediarepository ?? this.Ptas_land_ptas_mediarepository ?? this.Ptas_buildingdetail_ptas_mediarepository ??
                      new InnerInnerQuery[] { };
                }
            }
        }

        private class QueryResult
        {
            [JsonProperty("value")]
            public InnerQuery[] Value { get; set; }
        }
    }
}