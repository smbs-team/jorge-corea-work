// <copyright file="AddressLookupController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to lookup a address by id or address parts.
    /// </summary>
    [Route("v1/api/addresslookup")]
    [ApiController]
    public class AddressLookupController : ControllerBase
    {
        private readonly IParcelManager parcelManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressLookupController"/> class.
        /// </summary>
        /// <param name="parcelManager">To get the parcel info.</param>
        /// <param name="mapper">Automapper for conversions.</param>
        public AddressLookupController(IParcelManager parcelManager, IMapper mapper)
        {
            this.parcelManager = parcelManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a parcel from the search value.
        /// </summary>
        /// <param name="searchParam">Partial parcel id, or account or address.</param>
        /// <returns>List of parcel look up resutls.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "SearchForAddress")]
        public async Task<ActionResult<List<FormattedAddress>>> Post([FromBody] AddressLookupParam searchParam)
        {
            return await this.Get(searchParam.Searchvalue);
        }

        /// <summary>
        /// Get a parcel from the search value.
        /// </summary>
        /// <param name="searchvalue">Partial parcel id, or account or address.</param>
        /// <returns>List of parcel look up resutls.</returns>
        [HttpGet("{searchvalue}")]
        [SwaggerOperation(OperationId = "GetAddress")]
        public async Task<ActionResult<List<FormattedAddress>>> Get(string searchvalue)
        {
            return await ControllerExtensions<List<FormattedAddress>>.HandleExceptions(async () =>
            {
                string clean = searchvalue.Replace("\n", ", ");
                var result = this.mapper.Map<List<FormattedAddress>>(await this.parcelManager.LookupAddress(clean));
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