// <copyright file="SketchController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.Caching.Memory;

    using Newtonsoft.Json;

    using PTASCRMHelpers;
    using PTASCRMHelpers.Exception;

    using PTASDynamicsCrudCore.Classes;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller that adds to dynamics and manages sketches too.
    /// </summary>
    [Route("v1/api/[controller]")]
    public class SketchController : GenericDynamicsControllerBase
    {
        private const string AccesoryIdFieldName = "_ptas_accessoryid_value";
        private const string BuildingIdFieldname = "_ptas_buildingid_value";
        private const string LastesOfficialSketch = "Latest official sketch";
        private const int Limit = 20;
        private const string MediaRepositoryTableName = "ptas_mediarepository";
        private const string ParcelDetailsEntityName = "ptas_parceldetails";
        private const string PTASIsOfficial = "ptas_isofficial";
        private const string SketchesEntityName = "ptas_sketch";
        private const string SketchIdField = "ptas_sketchid";
        private const string SketchIdFieldName = "_ptas_sketchid_value";
        private const string SystemUsersEntityName = "systemusers";
        private const string UnitIdFieldName = "_ptas_unitid_value";
        private const string VisitedEntityName = "ptas_visitedsketchs";
        private static readonly string SketchesEntityNamePlural = SketchesEntityName.Pluralize();
        private readonly CRMWrapper wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SketchController"/> class.
        /// </summary>
        /// <param name="config">App config.</param>
        /// <param name="memoryCache">Memory Cache.</param>
        /// <param name="wrapper">CRM Wrapper.</param>
        /// <param name="tokenManager">Token.</param>
        public SketchController(IConfigurationParams config, IMemoryCache memoryCache, CRMWrapper wrapper, ITokenManager tokenManager)
            : base(config, memoryCache, tokenManager)
        {
            this.wrapper = wrapper;
        }

        private string TokenTypeStr => this.HttpContext.Request.Query["TokenType"] == "AAD" ? "&TokenType=AAD" : string.Empty;

        /// <summary>
        /// Deletes the sketch related files on document storage.
        /// </summary>
        /// <param name="sketchId">Id of the sketch to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("DeleteSketch")]
        public async Task<ActionResult> DeleteSketch(string sketchId)
        {
            try
            {
                var deleteSketchTask = this.CallDocumentServices(sketchId, false, true);
                var deleteSketchEntityTask = this.wrapper.ExecuteDelete(SketchesEntityNamePlural, sketchId);
                await Task.WhenAll(deleteSketchTask, deleteSketchEntityTask);
                var message = deleteSketchTask.Result;
                var wasDeleted = deleteSketchEntityTask.Result;
                return new OkObjectResult(new { message, wasDeleted });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Gets a list of entities and the related sketch.
        /// </summary>
        /// <param name="requests">Requests to process.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [SwaggerOperation(OperationId = "GetItemsAndSketch")]
        [HttpPost("GetItemsAndSketch")]
        public async Task<ActionResult> GetItemsAndSketch([FromBody] EntitysetRequestsAndSketch requests)
        {
            try
            {
                if (requests is null)
                {
                    throw new ArgumentNullException(nameof(requests));
                }

                string sketchId = requests.SketchId;

                var fetchItemsTask = this.FetchItems(requests.Requests, false);
                var foundSketchTask = this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, $"$filter=ptas_sketchid eq '{requests.SketchId}'&$expand=ptas_parcelid($select=ptas_name, ptas_addr1_compositeaddress_oneline)");
                var sketchContentTask = this.CallDocumentServices(sketchId, isSvg: false, isDelete: false);

                await Task.WhenAll(fetchItemsTask, foundSketchTask, sketchContentTask);

                EntityRequestResult[] fetchedItems = fetchItemsTask.Result;
                dynamic[] foundSketches = foundSketchTask.Result;
                string sketch = sketchContentTask.Result;

                var foundParcel = foundSketches.Select(itm => new
                {
                    ParcelName = itm.ptas_parcelid.ptas_name,
                    ParcelId = itm._ptas_parcelid_value,
                    ParcelAddress = itm.ptas_parcelid.ptas_addr1_compositeaddress_oneline,
                }).FirstOrDefault();

                var foundNode = foundSketches.FirstOrDefault();
                if (foundNode == null)
                {
                    throw new Exception($"Sketch not found: {requests.SketchId}");
                }

                string building = foundNode._ptas_buildingid_value;
                string accesory = foundNode._ptas_accessoryid_value;
                string unit = foundNode._ptas_unitid_value;

                (Guid itemId, string itemEntity) = this.GetRelatedEntityFieldFrom(building, accesory, unit);

                var draftTask = this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, $"$filter={itemEntity} eq '{itemId}' and ptas_isofficial eq false and ptas_iscomplete eq false&$expand=ptas_lockedbyid&$top=1");
                var completedTask = this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, $"$filter={itemEntity} eq '{itemId}' and ptas_isofficial eq true and ptas_iscomplete eq true&$top=1");

                var greatestVersionTask = this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, $"$filter={itemEntity} eq '{itemId}'&$orderby=ptas_version desc&$select=ptas_version&$top=1");

                var getUserTask = this.GetUser(this.HttpContext);

                await Task.WhenAll(draftTask, completedTask, getUserTask, greatestVersionTask);

                var greatestVersionItem = greatestVersionTask.Result.FirstOrDefault();

                var draftItem = draftTask.Result.FirstOrDefault();

                string draftId = draftItem?.ptas_sketchid;
                var draftUser = draftItem?.ptas_lockedbyid;
                var draftLocked = draftItem?.ptas_locked;

                var completedItem = completedTask.Result.FirstOrDefault();
                var completedId = completedItem?.ptas_sketchid;
                var completedVersion = greatestVersionItem?.ptas_version;

                var user = getUserTask.Result;
                return new OkObjectResult(new
                {
                    Parcel = foundParcel,
                    ImageUrl = this.ImageUrl(draftId),
                    DraftLocked = draftLocked,
                    DraftLockedBy = draftUser,
                    User = user,
                    Items = fetchedItems,
                    DraftId = draftId,
                    CompletedId = completedId,
                    CompletedVersion = completedVersion,
                    Sketch = sketch,
                    Svg = this.ToSVG(sketch),
                });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Gets recent sketches for current user.
        /// </summary>
        /// <returns>List of recent sketches if any.</returns>
        [HttpGet("GetRecent")]
        public async Task<ActionResult> GetRecentSketches()
        {
            try
            {
                var userId = await this.GetUserId(this.HttpContext);
                string query = $"$top=40&$orderby=ptas_visiteddate desc&$filter=_ptas_visitedbyid_value eq '{userId}'";
                VisitedSketchInfoWithImageUrl[] visitedInfo = await this.wrapper.ExecuteGet<VisitedSketchInfoWithImageUrl>(VisitedEntityName, query);
                VisitedSketchInfoWithImageUrl[] results = visitedInfo.Distinct((p1, p2) => p1.Ptas_sketchid == p2.Ptas_sketchid).ToArray();
                var taskResults = await Task.WhenAll(this.GetSketchTasks(results));
                var sketchedDict = taskResults.Select(x => x.FirstOrDefault()).Where(x => x != null).ToDictionary(
                item =>
                {
                    return ((Guid)item.ptas_sketchid).ToString();
                },
                item =>
                    {
                        (Guid itemId, string itemEntity) = this.GetEntityFrom((Guid?)item._ptas_buildingid_value, (Guid?)item._ptas_accessoryid_value, (Guid?)item._ptas_unitid_value);
                        string x = this.GetSVGForSketch((Guid)item.ptas_sketchid).Result;
                        return new
                        {
                            itemId,
                            itemEntity,
                            item.ptas_isofficial,
                            item._ptas_parcelid_value,
                            item.ptas_parcelid.ptas_addr1_compositeaddress_oneline,
                            item.ptas_parcelid.ptas_name,
                            svg = x,
                        };
                    });

                IEnumerable<ReturnedVisitedSketch> findResults =
                    results.Where(r => r.Ptas_sketchid != null)
                    .Select(r =>
                   {
                       string sketchid = r.Ptas_sketchid;
                       var sketchRef = sketchedDict[sketchid];
                       return new ReturnedVisitedSketch
                       {
                           Edited = r.Ptas_edited,
                           SketchId = sketchid,
                           VisitedDate = r.Ptas_visiteddate,
                           ImageUrl = this.ImageUrl(sketchid),
                           VisitedSketchId = sketchid,
                           RelatedEntityId = sketchRef.itemId,
                           Svg = sketchRef.svg,
                           RelatedEntityType = sketchRef.itemEntity,
                           IsOfficial = sketchRef.ptas_isofficial,
                           ParcelId = sketchRef._ptas_parcelid_value,
                           ParcelName = sketchRef.ptas_name,
                           Address = sketchRef.ptas_addr1_compositeaddress_oneline,
                       };
                   }).ToList();

                return new OkObjectResult(findResults);
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// History of a sketch.
        /// </summary>
        /// <param name="sketchId">Sketch Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("History")]
        public async Task<ActionResult> History(string sketchId)
        {
            try
            {
                if (sketchId is null)
                {
                    throw new ArgumentNullException(nameof(sketchId));
                }

                var sketchRef = (await this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, $"$filter=ptas_sketchid eq '{sketchId}'")).FirstOrDefault();
                if (sketchRef == null)
                {
                    throw new DynamicsHttpRequestException("Cannot find sketch info.", null);
                }

                (Guid relatedEntityId, string relatedEntityField) = this.GetRelatedEntityFieldFrom((Guid?)sketchRef._ptas_buildingid_value, (Guid?)sketchRef._ptas_accessoryid_value, (Guid?)sketchRef._ptas_unitid_value);
                string query = $"$filter={relatedEntityField} eq '{relatedEntityId}'";
                var otherSketches = (await this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, query))
                    .Select(item => new
                    {
                        item.ptas_sketchid,
                        imageUrl = this.ImageUrl(item.ptas_sketchid),
                        isOfficial = item.ptas_isofficial,
                        isComplete = item.ptas_iscomplete,
                        version = item.ptas_version,
                        drawDate = item.ptas_drawdate,
                    }).ToArray();
                IEnumerable<Task<string>> sketchTasks = otherSketches.Select(itm =>
                {
                    Task<string> sketch = this.CallDocumentServices($"{itm.ptas_sketchid}", false, false);
                    return sketch;
                });
                var results = (await Task.WhenAll(sketchTasks)).Select((r, i) =>
                {
                    var sketchData = otherSketches[i];
                    return new
                    {
                        sketchData.drawDate,
                        sketchData.imageUrl,
                        sketchData.isComplete,
                        sketchData.isOfficial,
                        sketchData.ptas_sketchid,
                        sketchData.version,
                        svg = this.ToSVG(r),
                        sketch = r,
                    };
                }).ToList();
                return new OkObjectResult(results);
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Searches for a string.
        /// </summary>
        /// <param name="searchFor">string to search for.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("Search")]
        public async Task<ActionResult> Search(string searchFor)
        {
            try
            {
                searchFor = searchFor.Trim();

                if (searchFor.Length < 3)
                {
                    return new OkObjectResult(new dynamic[] { });
                }

                var searchs = new List<string>()
                {
                    $"contains(ptas_tags,'{searchFor}')",
                    $"contains(ptas_parcelid/ptas_addr1_compositeaddress_oneline,'{searchFor}')",
                };

                if (searchFor.CouldBeId())
                {
                    if (!searchFor.Contains("-") && searchFor.Length > 6)
                    {
                        searchFor = $"{searchFor.Substring(0, 6)}-{searchFor.Substring(6)}";
                    }

                    searchs.Add($"startswith(ptas_parcelid/ptas_name,'{searchFor}')");
                }

                var found = (await Task.WhenAll(searchs.Select(s => this.GetSketches(s))))
                    .SelectMany(_ => _).ToArray();

                var svgTasks = found
                    .Select(sketchRef => $"{sketchRef.ptas_sketchid}")
                    .Select(sketchId => this.GetSVGForSketch(sketchId));

                if (!svgTasks.Any())
                {
                    return new OkObjectResult(new object[] { });
                }

                var allsvg = await Task.WhenAll(svgTasks);
                var converted = found.Select((sketchRef, index) =>
                {
                    (Guid relatedEntityId, string relatedEntityType) = this.GetEntityFrom((Guid?)sketchRef._ptas_buildingid_value, (Guid?)sketchRef._ptas_accessoryid_value, (Guid?)sketchRef._ptas_unitid_value);
                    //// ptas_isofficial eq false and ptas_iscomplete eq false

                    string address = $"{sketchRef.ptas_parcelid.ptas_addr1_compositeaddress_oneline}".Trim();
                    return new SketchSearchResult
                    {
                        ParcelId = sketchRef._ptas_parcelid_value,
                        ParcelName = sketchRef.ptas_parcelid.ptas_name,
                        Address = address,
                        SketchId = sketchRef.ptas_sketchid,
                        IsOfficial = sketchRef.ptas_isofficial as bool? ?? false,
                        IsDraft = !(sketchRef.ptas_isofficial as bool? ?? false) && !(sketchRef.ptas_iscomplete as bool? ?? false),
                        RelatedEntityId = relatedEntityId,
                        RelatedEntityType = relatedEntityType,
                        ImageUrl = this.ImageUrl(sketchRef.ptas_sketchid),
                        Svg = allsvg[index],
                    };
                }).Distinct((a, b) =>
                {
                    return a.SketchId == b.SketchId;
                }).ToList();
                return new OkObjectResult(converted);
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Sets sketch to official.
        /// </summary>
        /// <param name="officialToggle">
        /// Toggle for official setting. Only one of the three related id's must be not null.
        /// </param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [SwaggerOperation(OperationId = "SetSketchAsOfficial")]
        [HttpPost("SetSketchAsOfficial")]
        public async Task<ActionResult> SetSketchAsOfficial([FromBody] IsOfficialToggle officialToggle)
        {
            try
            {
                var fetchedSketch = await this.CallDocumentServices(officialToggle.SketchId, isSvg: false, isDelete: false, throwError: true, isXML: true);
                var (id, name) = this.GetEntityFrom(officialToggle.BuildingId, officialToggle.AccesoryId, officialToggle.UnitId);
                var mockChanges = new EntitySetChangesAndSketch
                {
                    Items = new EntityChanges[]
                    {
                    new EntityChanges
                    {
                        EntityId = id.ToString(),
                        EntityName = name,
                        Changes = new Dictionary<string, object>
                        {
                            { SketchIdFieldName, officialToggle.SketchId },
                        },
                    },
                    new EntityChanges
                    {
                        EntityId = officialToggle.SketchId,
                        EntityName = SketchesEntityName,
                        Changes = new Dictionary<string, object>
                        {
                            { PTASIsOfficial, true },
                            { BuildingIdFieldname, officialToggle.BuildingId },
                            { UnitIdFieldName, officialToggle.UnitId },
                            { AccesoryIdFieldName, officialToggle.AccesoryId },
                            { SketchIdField, officialToggle.SketchId },
                        },
                    },
                    },
                    SketchId = officialToggle.SketchId,
                    Sketch = fetchedSketch,
                    DeactivateMigrated = officialToggle.DeactivateMigrated,
                };
                return await this.UpdateItems(mockChanges);
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        /// <summary>
        /// Attempts to apply json to dynamics.
        /// </summary>
        /// <param name="changes">Json to apply.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("UpdateItemsAndSketch")]
        [SwaggerOperation(OperationId = "UpdateItemsAndSketch")]
        public async Task<ActionResult> UpdateItems([FromBody] EntitySetChangesAndSketch changes)
        {
            try
            {
                // step 1: send batch changes.
                var result = await this.TransactionHelper.PrepareAndSendBatch(
                     changes.Items.Select(c => new EntityChanges
                     {
                         Changes = c.Changes,
                         EntityId = c.EntityId,
                         EntityName = c.EntityName,
                     }), excludeUserfields: false);

                // step2: if this sketch is official, then update other sketches.
                Task<OfficialSketchInfo> officialSketchTask = this.UpdateOtherItemsIfOfficial(changes);

                // step3: mark the sketch as visited.
                Task<bool> visitedSketchTask = this.SaveVisitedSketch(changes.SketchId);

                // step4: save the sketch.
                Task<string> savedSketchTask = this.SaveSketch(changes.SketchId, changes.Sketch);

                await Task.WhenAll(officialSketchTask, visitedSketchTask, savedSketchTask);

                OfficialSketchInfo officialSketch = officialSketchTask.Result;
                if (officialSketch != null)
                {
                    var svg = this.ToSVG(savedSketchTask.Result);
                    await this.SaveToMediaRepository(officialSketch, svg);
                }

                return await Task.FromResult(new OkObjectResult(new
                {
                    changes.SketchId,
                    result,
                }));
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        private static (string fieldName, int returnType, string relationshipeEntity, string relatedTable) GetEntityKeyForMedia(string entity)
        {
            switch (entity)
            {
                case BuildingIdFieldname:
                    return ("ptas_buildingguid", 2, "ptas_buildingdetail_ptas_mediarepository", "ptas_buildingdetails");

                case UnitIdFieldName:
                    return ("ptas_unitguid", 4, "ptas_condounit_ptas_mediarepository", "ptas_condounits");

                case AccesoryIdFieldName:
                    return ("ptas_accessoryguid", 3, "ptas_accessorydetail_ptas_mediarepository", "ptas_accessorydetails");

                default:
                    throw new DynamicsHttpRequestException("Unknown entity type: " + entity, null);
            }
        }

        private static Guid? GetGuid(string buildingId)
        {
            return string.IsNullOrEmpty(buildingId) ? (Guid?)null : new Guid(buildingId);
        }

        private static Guid? GetValueOrNull(dynamic sketchNode, string key)
        {
            string value = sketchNode[key] as string;
            return string.IsNullOrEmpty(value) || !Guid.TryParse(value, out Guid g) ? null : (Guid?)g;
        }

        private static Guid? GetValueOrNull(EntityChanges sketchNode, string key)
        {
            if (!sketchNode.Changes.TryGetValue(key, out object b))
            {
                return null;
            }

            string value = b as string;
            return string.IsNullOrEmpty(value) || !Guid.TryParse(value, out Guid g) ? null : (Guid?)g;
        }

        private async Task<string> CallDocumentServices(string sketchId, bool isSvg, bool isDelete, bool throwError = false, bool isXML = false)
        {
            var fullRoute = this.GetFullRoute(sketchId, isSvg);
            if (isXML)
            {
                fullRoute = fullRoute.Replace(".json", ".xml");
            }

            using (
                var httpClient = this.CreateAuthenticatedClient())
            {
                HttpResponseMessage httpResponseMessage;
                if (isDelete)
                {
                    httpResponseMessage = await httpClient.DeleteAsync(fullRoute);
                }
                else
                {
                    httpResponseMessage = await httpClient.GetAsync(fullRoute);
                }

                var sketchStr = await httpResponseMessage.Content.ReadAsStringAsync();
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return throwError
                        ? throw new Exception("An error occurred trying to fetch item: " + httpResponseMessage.ReasonPhrase)
                        : JsonConvert.SerializeObject(new
                        {
                            status = httpResponseMessage.StatusCode,
                            message = sketchStr,
                        });
                }

                return sketchStr;
            }
        }

        private async Task ClearOtherOffficialFlags(Guid itemId, string itemEntity, Guid? sketchId)
        {
            var otherOfficialSketches = await this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, $"$filter={itemEntity} eq {itemId} and ptas_sketchid ne {sketchId} and ptas_isofficial eq true");
            var sketchTasks = otherOfficialSketches.Select(sk
                =>
            {
                Task<bool> task = this.wrapper
                    .ExecutePatch<dynamic>(
                        SketchesEntityNamePlural,
                        new
                        {
                            sk.ptas_sketchid,
                            ptas_isofficial = false,
                        },
                        $"{sk.ptas_sketchid}");
                return task;
            });
            _ = await Task.WhenAll(sketchTasks);
        }

        private HttpClient CreateAuthenticatedClient()
        {
            string authHeader = JWTDecoder.GetAuthHeader(this.HttpContext)?.Replace("Bearer ", string.Empty);
            AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue(
               "Bearer",
               authHeader);
            var client = DynamicsHelpers.CreateClient(authenticationHeaderValue);
            return client;
        }

        private string CreateBlobPath(string v) => v.Length >= 4 ? $"/{v[0]}/{v[1]}/{v[2]}/{v[3]}/{v}" : v;

        private async Task DeactivateMigratedItems(Guid itemId, string itemEntity, Guid? sketchId)
        {
            var otherOfficialSketches = await this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, $"$filter={itemEntity} eq {itemId} and ptas_sketchid ne {sketchId} and contains(ptas_tags,'Migrated')");
            var sketchTasks = otherOfficialSketches.Select(sk
                =>
            {
                Task<bool> task = this.wrapper
                    .ExecutePatch<dynamic>(
                        SketchesEntityNamePlural,
                        new
                        {
                            sk.ptas_sketchid,
                            statecode = 1,
                            statuscode = 2,
                        },
                        $"{sk.ptas_sketchid}");
                return task;
            });
            _ = await Task.WhenAll(sketchTasks);
        }

        private (Guid itemId, string itemEntity) GetEntityFrom(string buildingId, string accesoryId, string unitId)
        {
            return this.GetEntityFrom(GetGuid(buildingId), GetGuid(accesoryId), GetGuid(unitId));
        }

        private (Guid itemId, string itemEntity) GetEntityFrom(Guid? buildingId, Guid? accesoryId, Guid? unitId)
                            => buildingId.HasValue
                ? (buildingId.Value, "ptas_buildingdetail")
                : accesoryId.HasValue
                ? (accesoryId.Value, "ptas_accessorydetail")
                : unitId.HasValue
                ? (unitId.Value, "ptas_condounit")
                : ((Guid itemId, string itemEntity))(default, default);

        private string GetFullRoute(string sketchId, bool isSVG) => (isSVG
                ? $"{this.Config.DocumentStorageUrl}api/svg?route={sketchId}.svg"
                : $"{this.Config.DocumentStorageUrl}api/sketchToJson?route={sketchId}.json") + this.TokenTypeStr;

        private async Task<int> GetLastOrder(OfficialSketchInfo officialSketch, string fieldName)
        {
            string query = $"$filter={fieldName} eq {officialSketch.ItemId}&$orderby=ptas_order desc&$top=1&$select=ptas_order";
            WithOrder[] results = await this.wrapper.ExecuteGet<WithOrder>(MediaRepositoryTableName.Pluralize(), query);
            WithOrder firstItem = results.FirstOrDefault();
            return 1 + (firstItem?.Ptas_order ?? 0);
        }

        private async Task<(string itemId, bool isNew)> GetPreviousIdOrNewId(OfficialSketchInfo officialSketch, string fieldName)
        {
            var query = $"$filter={fieldName} eq {officialSketch.ItemId} and ptas_description eq '{LastesOfficialSketch}'"
                        + "&$top=1&$select=ptas_mediarepositoryid";
            var result = await this.wrapper.ExecuteGet<dynamic>(MediaRepositoryTableName.Pluralize(), query);

            dynamic foundMedia = result.FirstOrDefault();
            var idToUpdate = (foundMedia?.ptas_mediarepositoryid ?? Guid.NewGuid()).ToString();
            return (idToUpdate, foundMedia == null);
        }

        private (Guid itemId, string itemEntity) GetRelatedEntityFieldFrom(string buildingId, string accesoryId, string unitId)
                            => !string.IsNullOrEmpty(buildingId)
                ? (new Guid(buildingId), BuildingIdFieldname)
                : !string.IsNullOrEmpty(accesoryId)
                ? (new Guid(accesoryId), AccesoryIdFieldName)
                : !string.IsNullOrEmpty(unitId)
                ? (new Guid(unitId), UnitIdFieldName)
                : ((Guid itemId, string itemEntity))(default, default);

        private (Guid itemId, string itemEntity) GetRelatedEntityFieldFrom(Guid? buildingId, Guid? accesoryId, Guid? unitId)
            => buildingId.HasValue
                ? (buildingId.Value, BuildingIdFieldname)
                : accesoryId.HasValue
                ? (accesoryId.Value, AccesoryIdFieldName)
                : unitId.HasValue
                ? (unitId.Value, UnitIdFieldName)
                : ((Guid itemId, string itemEntity))(default, default);

        private async Task<dynamic[]> GetSketches(string filter)
        {
            string query = $"$top={Limit}&$filter={filter}&$expand=ptas_parcelid($select=ptas_addr1_compositeaddress_oneline,ptas_name)".Replace("'", "%27");
            var found = await this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, query);
            return found;
        }

        private IEnumerable<Task<dynamic[]>> GetSketchTasks(VisitedSketchInfoWithImageUrl[] visitedSketches)
            => visitedSketches.Select(r => r.Ptas_sketchid)
            .Where(itm => itm != null)
                    .Distinct()
                    .Select(item
                        =>
                    {
                        string query = $"$filter=ptas_sketchid eq '{item}'&$expand=ptas_parcelid($select=ptas_addr1_compositeaddress_oneline,ptas_name)";
                        return this.wrapper.ExecuteGet<dynamic>(SketchesEntityNamePlural, query);
                    });

        private async Task<string> GetSVGForSketch(object ptas_sketchid)
        {
            try
            {
                return await this.CallDocumentServices(ptas_sketchid.ToString(), true, false);
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task<DynamicsSystemUser> GetUser(HttpContext httpContext)
        {
            var email = JWTDecoder.GetEmailFromHeader(httpContext);
            DynamicsSystemUser[] users = await this.wrapper.ExecuteGet<DynamicsSystemUser>(SystemUsersEntityName, $"$filter=windowsliveid eq '{email}'");
            if (!users.Any())
            {
                throw new DynamicsHttpRequestException("Cannot find user info.", null);
            }

            DynamicsSystemUser dynamicsSystemUser = users.First();
            return dynamicsSystemUser;
        }

        private async Task<string> GetUserId(HttpContext httpContext)
            => (await this.GetUser(httpContext)).Systemuserid;

        private string ImageUrl(object item) => $"{this.Config.DocumentStorageUrl}api/SVG?route={item}{this.TokenTypeStr}";

        private async Task ProcessOtherSketches(Guid itemId, string itemEntity, Guid? sketchId, bool deactivateMigrated)
        {
            List<Task> tasks = new List<Task>()
            {
                this.ClearOtherOffficialFlags(itemId, itemEntity, sketchId),
            };
            if (deactivateMigrated)
            {
                tasks.Add(this.DeactivateMigratedItems(itemId, itemEntity, sketchId));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private ObjectResult ReportError(Exception ex)
            => ex.Message.StartsWith("{") ?
            this.StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.DeserializeObject(ex.Message)) :
            this.StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message,
            });

        private async Task<string> SaveSketch(string sketchId, dynamic sketch)
        {
            try
            {
                var sketchToSave = sketch is string r ? r : JsonConvert.SerializeObject(sketch);
                if (string.IsNullOrEmpty(sketchToSave) || sketchToSave == "{}")
                {
                    return "{}";
                }

                var fullRoute = this.GetFullRoute(sketchId, false);
                var httpContent = new StringContent(sketchToSave, Encoding.UTF8, "application/json");
                HttpClient client = this.CreateAuthenticatedClient();
                HttpResponseMessage httpResponseMessage = await client.PostAsync(fullRoute, httpContent);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    var response = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new DynamicsHttpRequestException(response, null);
                }

                return await Task.FromResult(sketchToSave);
            }
            catch (Exception ex)
            {
                throw new DynamicsHttpRequestException("Failed on SaveSketch", ex);
            }
        }

        private async Task SaveSVG(string id, string svg)
        {
            var url = string.Format(this.Config.SaveMediaUri, id) + this.TokenTypeStr;
            var client = this.CreateAuthenticatedClient();

            var requestContent = new MultipartFormDataContent();
            byte[] buffer = Encoding.UTF8.GetBytes(svg);
            MemoryStream content = new MemoryStream(buffer)
            {
                Position = 0,
            };
            var fileContent = new StreamContent(content);
            requestContent.Add(fileContent, "file", id);
            HttpResponseMessage result = await client.PostAsync(url, requestContent);
            string message = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Save SVG id {id}. Error: {message}");
            }
        }

        private async Task<bool> SaveToMediaRepository(OfficialSketchInfo officialSketch, string svg)
        {
            var (fieldName, fieldId, relationshipEntity, relatedTable) = GetEntityKeyForMedia(officialSketch.Entity);

            Task<int> orderTask = this.GetLastOrder(officialSketch, fieldName);
            Task<(string itemId, bool isNew)> mediaRecordIdTask = this.GetPreviousIdOrNewId(officialSketch, fieldName);
            ////await orderTask;
            ////await mediaRecordIdTask;
            await Task.WhenAll(orderTask, mediaRecordIdTask);

            (string itemId, bool isNew) = mediaRecordIdTask.Result;
            await this.SaveSVG(itemId, svg);
            string blobPath = this.CreateBlobPath(Path.GetFileNameWithoutExtension(itemId));

            /*$"{entityRef.ReferencingAttribute}@odata.bind", $"/{plurarized}({cc})"*/
            string systemuserid = await this.GetUserId(this.HttpContext);
            object entity = isNew
                ? new
                {
                    ptas_mediarepositoryid = itemId,
                    ptas_rootfolder = "/media",
                    ptas_blobpath = blobPath,
                    ptas_description = LastesOfficialSketch,
                    ptas_fileextension = "svg",
                    ptas_order = orderTask.Result,
                    ptas_primary = false,
                    ptas_posttoweb = false,
                    ptas_mediatype = 1,
                    ptas_imagetype = 2,
                    ptas_relatedobjectmediatype = fieldId == 3 ? 1 : fieldId,
                    ptas_usagetype = fieldId,
                    ptas_mediadate = DateTime.Now,
                    ptas_updatedbyuser = systemuserid,
                }
                : (object)new
                {
                    ptas_mediarepositoryid = itemId,
                    ptas_blobpath = blobPath,
                    ptas_mediadate = DateTime.Now,
                    ptas_updatedbyuser = systemuserid,
                };

            var dict = new RouteValueDictionary(entity)
            {
                [fieldName] = officialSketch.ItemId,
            };
            bool passed = await this.wrapper.ExecutePatch(MediaRepositoryTableName.Pluralize(), dict, itemId);

            if (!passed)
            {
                return false;
            }

            // NOTE: insert the relationship.
            // NOTE: fixed 11/17/2020 WR
            var s = $"{this.Config.CRMUri}{this.Config.ApiRoute}{relatedTable}({officialSketch.ItemId})";
            var toSave = new DynamicsNTONRelationship(s);
            bool result = await this.wrapper.ExecuteNTONPost(
                MediaRepositoryTableName.Pluralize(),
                toSave,
                itemId,
                relationshipEntity);
            return result;
        }

        private async Task<bool> SaveVisitedSketch(string sketchId)
        {
            try
            {
                string newId = Guid.NewGuid().ToString();
                string systemuserid = await this.GetUserId(this.HttpContext);
                var visitedSketch = new VisitedSketchInfo
                {
                    Ptas_edited = true,
                    Ptas_name = string.Empty,
                    Ptas_sketchid = $"/ptas_sketchs({sketchId})",
                    Ptas_visiteddate = DateTime.Now,
                    Ptas_visitedbyid = $"/systemusers({systemuserid})",
                    Ptas_visitedsketchid = newId,
                };
                var r = await this.wrapper.ExecutePost<dynamic>(VisitedEntityName, visitedSketch, string.Empty);
                if (!r)
                {
                    throw new DynamicsHttpRequestException($"Executed post on {VisitedEntityName}. NewId: {newId} and received empty result. ", null);
                }

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new DynamicsHttpRequestException("Failed on SaveVisitedSketch. Error: " + ex.Message, ex);
            }
        }

        private string ToSVG(string json)
        {
            var sketch = (PTASMobileSketch.SketchControl)JsonConvert.DeserializeObject(json, typeof(PTASMobileSketch.SketchControl), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var svg = PTASMobileSketch.SketchToSVG.Write(sketch);
            return svg;
        }

        private async Task<OfficialSketchInfo> UpdateOtherItemsIfOfficial(EntitySetChangesAndSketch changes)
        {
            EntityChanges foundNode = changes.Items.FirstOrDefault(c
                => c.EntityName == SketchesEntityName && c.Changes.ContainsKey("ptas_isofficial") && (bool)c.Changes["ptas_isofficial"]);

            if (foundNode == null)
            {
                return await Task.FromResult<OfficialSketchInfo>(null);
            }

            var building = GetValueOrNull(foundNode, BuildingIdFieldname);
            var accesory = GetValueOrNull(foundNode, AccesoryIdFieldName);
            var unit = GetValueOrNull(foundNode, UnitIdFieldName);
            var sketchId = GetValueOrNull(foundNode, SketchIdField);
            if (sketchId is null)
            {
                throw new DynamicsHttpRequestException("Sketchid field is missing", null);
            }

            var (itemId, itemEntity) = this.GetRelatedEntityFieldFrom(building, accesory, unit);
            await this.ProcessOtherSketches(itemId, itemEntity, sketchId, changes.DeactivateMigrated ?? false);

            dynamic newSaveItem = new OfficialSketchInfo
            {
                SketchId = sketchId.Value,
                ItemId = itemId,
                Entity = itemEntity,
                SketchInfo = foundNode,
            };

            return newSaveItem;
        }
    }
}