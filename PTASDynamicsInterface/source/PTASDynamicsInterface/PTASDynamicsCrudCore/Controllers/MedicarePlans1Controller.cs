// <copyright file="MedicarePlans1Controller.cs" company="King County">
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
    /// Controller for the crud of medicar plans.
    /// </summary>
    public class MedicarePlans1Controller : AbstractCrudController<FormMedicarePlan, DynamicsMedicarePlan>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MedicarePlans1Controller"/> class.
        /// </summary>
        /// <param name="wrapper">CRM wrapper for ancestor.</param>
        /// <param name="mapper">Automapper for ancestor.</param>
        public MedicarePlans1Controller(CRMWrapper wrapper, IMapper mapper)
          : base(wrapper, mapper)
        {
        }

        /// <inheritdoc/>
        protected override string TableName => "ptas_medicareplans";

        /// <inheritdoc/>
        protected override string GetOpQuery => "$top=1000";

        /// <inheritdoc/>
        protected override string KeyField => "ptas_medicareplanid";

        /// <summary>
        /// http get action.
        /// </summary>
        /// <param name="id">Id of the Medicare Plan to fetch.</param>
        /// <returns>A list of option sets as defined in dynamics.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetTheMedicarePlanById")]
        public async Task<ActionResult<FormMedicarePlan>> Get(string id)
        {
            return await ControllerExtensions<FormMedicarePlan>.HandleExceptions(async () =>
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
        /// <returns>A FormMedicarePlan<see cref="FormMedicarePlan"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "PostMedicarePlans")]
        public async Task<ActionResult<FormMedicarePlan>> Post([FromBody] FormMedicarePlan value)
        {
                return await ControllerExtensions<FormMedicarePlan>.HandleExceptions(async () =>
                {
                    return await this.AbstractPost(value);
                });
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="value">Form values of entity.</param>
        /// <returns>A FormMedicarePlan<see cref="FormMedicarePlan"/> representing the asynchronous operation.</returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(OperationId = "PatchMedicarePlans")]
        public virtual async Task<ActionResult<FormMedicarePlan>> Patch(string id, [FromBody] FormMedicarePlan value)
        {
            return await ControllerExtensions<FormMedicarePlan>.HandleExceptions(async () =>
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
        [SwaggerOperation(OperationId = "DeleteMedicarePlans")]
        public virtual async Task<ActionResult<FormMedicarePlan>> Delete(string id)
        {
            return await ControllerExtensions<FormMedicarePlan>.HandleExceptions(async () =>
            {
                return await this.AbstractDelete(id);
            });
        }
    }
}
