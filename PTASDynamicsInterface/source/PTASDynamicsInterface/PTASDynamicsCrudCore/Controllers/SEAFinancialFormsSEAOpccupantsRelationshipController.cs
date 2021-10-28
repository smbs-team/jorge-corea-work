// <copyright file="SEAFinancialFormsSEAOpccupantsRelationshipController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to handle SEAppOccupant entity.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SEAFinancialFormsSEAOpccupantsRelationshipController : Controller
    {
        private const string TableName = "ptas_sefinancialformses";
        private const string NavigationProperty = "ptas_seappoccupant_ptas_sefinancialforms";
        private const string CounterpartTableName = "ptas_seappoccupants";
        private readonly CRMWrapper wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SEAFinancialFormsSEAOpccupantsRelationshipController"/> class.
        /// </summary>
        /// <param name="wrapper">Generic wrapper.</param>
        /// <param name="config">Config values.</param>
        public SEAFinancialFormsSEAOpccupantsRelationshipController(CRMWrapper wrapper, IConfigurationParams config)
        {
            this.wrapper = wrapper;
            this.Config = config;
        }

        /// <summary>
        /// Gets the Global configuration params.
        /// </summary>
        private IConfigurationParams Config { get; }

        /// <summary>
        /// Tries to create a new entity from body input.
        /// </summary>
        /// <param name="id">Id on  SEAFinancialForms for the relationship.</param>
        /// <param name="value">Input information to save containing the id on SEAOccupants.</param>
        /// <returns>A FormSEAFinancialFormsSEAOpccupantsRelationship<see cref="FormSEAFinancialFormsSEAOpccupantsRelationship"/> representing the asynchronous operation.</returns>
        [HttpPost("{id}")]
        [SwaggerOperation(OperationId = "PostFormSEAFinancialFormsSEAOpccupantsRelationship")]
        public virtual async Task<ActionResult<FormSEAFinancialFormsSEAOpccupantsRelationship>> Post(string id, [FromBody] FormSEAFinancialFormsSEAOpccupantsRelationship value)
        {
            return await ControllerExtensions<FormSEAFinancialFormsSEAOpccupantsRelationship>.HandleExceptions(async () =>
            {
                return await this.SaveOccupantRelationship(id, value);
            });
        }

        /// <summary>
        /// Saves a financial = occupand relationship.
        /// </summary>
        /// <param name="id">Id of the financial form.</param>
        /// <param name="value">Value to save.</param>
        /// <returns>Created object.</returns>
        internal async Task<ActionResult<FormSEAFinancialFormsSEAOpccupantsRelationship>> SaveOccupantRelationship(string id, FormSEAFinancialFormsSEAOpccupantsRelationship value)
        {
            if (value.SEAppOccupantId == null)
            {
                return default;
            }

            var s = $"{this.Config.CRMUri}{this.Config.ApiRoute}{CounterpartTableName}({value.SEAppOccupantId})";
            var toSave = new DynamicsNTONRelationship(s);
            bool result = await this.wrapper.ExecuteNTONPost(TableName, toSave, id, NavigationProperty);
            JsonResult jresult = result ? new JsonResult(value) : new JsonResult((FormSEAFinancialFormsSEAOpccupantsRelationship)null);
            jresult.StatusCode = result ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NoContent;
            return jresult;
        }
    }
}