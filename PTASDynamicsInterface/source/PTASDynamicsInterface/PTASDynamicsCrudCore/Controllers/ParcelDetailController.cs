// <copyright file="ParcelDetailController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Exception;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to handle Parcel Detail.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ParcelDetailController : AbstractCrudController<FormParcelDetail, DynamicsParcelDetail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParcelDetailController"/> class.
        /// </summary>
        /// <param name="wrapper">Generic wrapper.</param>
        /// <param name="mapper">Automapper.</param>
        public ParcelDetailController(CRMWrapper wrapper, IMapper mapper)
          : base(wrapper, mapper)
        {
        }

        /// <inheritdoc/>
        protected override string TableName => "ptas_parceldetails";

        /// <inheritdoc/>
        protected override string GetOpQuery => "$top=10000";

        /// <inheritdoc/>
        protected override string KeyField => "ptas_parceldetailid";

        /// <summary>
        /// http get action.
        /// </summary>
        /// <param name="id">Id of the Parcel Detail to fetch.</param>
        /// <returns>A list of option sets as defined in dynamics.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetTheParcelDetailById")]
        public async Task<ActionResult<FormParcelDetail>> Get(string id)
        {
            return await ControllerExtensions<FormParcelDetail>.HandleExceptions(async () =>
            {
                var result = await this.AbstractGet(id);
                if ((result.Result as JsonResult).StatusCode == (int)HttpStatusCode.NoContent)
                {
                    throw new DynamicsInterfaceException(
    $"Request returns not data on reading {this.TableName} with key {id}.",
    (int)HttpStatusCode.NotFound,
    null);
                }

                return result;
            });
        }

        /// <summary>
        /// Tries to create a new entity from body input.
        /// </summary>
        /// <param name="value">Input information to save.</param>
        /// <returns>A FormParcelDetail<see cref="FormParcelDetail"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "PostParcelDetails")]
        public async Task<ActionResult<FormParcelDetail>> Post([FromBody] FormParcelDetail value)
        {
            return await ControllerExtensions<FormParcelDetail>.HandleExceptions(async () =>
            {
                return await this.AbstractPost(value);
            });
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="value">Form values of entity.</param>
        /// <returns>A FormParcelDetail<see cref="FormParcelDetail"/> representing the asynchronous operation.</returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(OperationId = "PatchParcelDetails")]
        public virtual async Task<ActionResult<FormParcelDetail>> Patch(string id, [FromBody] FormParcelDetail value)
        {
            return await ControllerExtensions<FormParcelDetail>.HandleExceptions(async () =>
            {
                return await this.AbstractPatch(id, value);
            });
        }

        /// <summary>
        /// Delete a row in a entity.
        /// </summary>
        /// <param name="id">the id of the instance to be delete.</param>
        /// <returns>true if delete is done, false if not.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteParcelDetails")]
        public virtual async Task<ActionResult<FormParcelDetail>> Delete(string id)
        {
            return await ControllerExtensions<FormParcelDetail>.HandleExceptions(async () =>
            {
                return await this.AbstractDelete(id);
            });
        }
    }
}