// <copyright file="ParcelLookupController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to lookup a Parcel by id or account.
    /// </summary>
    [Route("v1/api/parcellookup")]
    [ApiController]
    public class ParcelLookupController : ControllerBase
    {
        private readonly IParcelManager parcelManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParcelLookupController"/> class.
        /// </summary>
        /// <param name="parcelManager">To get the parcel info.</param>
        /// <param name="mapper">Automapper for conversions.</param>
        public ParcelLookupController(IParcelManager parcelManager, IMapper mapper)
        {
            this.parcelManager = parcelManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a parcel from the id, the accunt or the address.
        /// </summary>
        /// <param name="id">Partial parcel id, or account or address.</param>
        /// <returns>List of parcel look up resutls.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetParcelById")]
        public async Task<ActionResult<List<ParcelLookupResult>>> Get(string id)
        {
            return await ControllerExtensions<List<ParcelLookupResult>>.HandleExceptions(async () =>
            {
                var result = this.mapper.Map<List<ParcelLookupResult>>(await this.parcelManager.LookupParcel(id));
                JsonResult jResult = new JsonResult(result);
                if (result.Count == 0)
                {
                    jResult.StatusCode = (int)HttpStatusCode.NoContent;
                }
                else
                {
                    jResult.StatusCode = (int)HttpStatusCode.OK;
                }

                return jResult;
            });
        }
    }
}