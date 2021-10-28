// <copyright file="AbstractCrudController{TForm,TDynamics}.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Exception;

    /// <summary>
    /// Controller for all types of dynamics entities.
    /// </summary>
    /// <typeparam name="TForm">Incoming form class with normalized names.</typeparam>
    /// <typeparam name="TDynamics">Mapping for dynamics field names in JSON.</typeparam>
    public abstract class AbstractCrudController<TForm, TDynamics> : AbstractReadonlyController<TForm, TDynamics>
        where TForm : FormInput
        where TDynamics : class
    {
        private const string NoDataFoundValue = "No Data Found";

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCrudController{TForm, TDynamics}"/> class.
        /// </summary>
        /// <param name="wrapper">CRM wrapper.</param>
        /// <param name="mapper">Automapper.</param>
        public AbstractCrudController(CRMWrapper wrapper, IMapper mapper)
          : base(wrapper, mapper)
        {
        }

        /// <summary>
        /// Gets the indexing field for the get.
        /// </summary>
        protected abstract string KeyField { get; }

        /// <summary>
        /// Gets a particular entity by id.
        /// </summary>
        /// <param name="id">Id to search for in id field of the entity.</param>
        /// <returns>Found item or null.</returns>
        protected virtual async Task<ActionResult<TForm>> AbstractGet(string id)
        {
            var result = await this.Wrapper.ExecuteGet<TDynamics>(this.TableName, $"$filter={this.KeyField} eq '{id}'");
            JsonResult jResult = result == null || result.Length == 0
                ? new JsonResult((TForm)null)
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                }
                : new JsonResult(this.Mapper.Map<TForm>(result[0]))
                {
                    StatusCode = (int)HttpStatusCode.OK,
                };

            return jResult;
        }

        /// <summary>
        /// Tries to create a new entity from body input.
        /// </summary>
        /// <param name="value">Input information to save.</param>
        /// <returns>A entity described in json format <see cref="FromBody"/> representing the asynchronous operation.</returns>
        protected virtual async Task<ActionResult<TForm>> AbstractPost([FromBody] TForm value)
        {
            value.SetId();
            bool result = await this.Wrapper.ExecutePost(this.TableName, this.Mapper.Map<TDynamics>(value), this.KeyField);
            return result ? new JsonResult(value) : new JsonResult((TForm)null);
        }

        /// <summary>
        /// Updates file metadata.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="value">Form values of entity.</param>
        /// <returns>A TForm cref="TForm "/> representing the asynchronous operation.</returns>
        protected virtual async Task<ActionResult<TForm>> AbstractPatch(string id, [FromBody] TForm value)
        {
            var result = await this.Wrapper.ExecutePatch(this.TableName, this.Mapper.Map<TDynamics>(value), $"{this.KeyField}={id}");
            if (!result)
            {
                throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
            }

            return new JsonResult(value);
        }

        /// <summary>
        /// Delete a row in a entity.
        /// </summary>
        /// <param name="id">the id of the instance to be delete.</param>
        /// <returns>true if delete is done, false if not.</returns>
        protected virtual async Task<ActionResult<TForm>> AbstractDelete(string id)
        {
            var result = await this.Wrapper.ExecuteDelete(this.TableName, id.ToString());
            if (!result)
            {
                throw new DynamicsInterfaceException(NoDataFoundValue, (int)HttpStatusCode.NotFound, null);
            }

            JsonResult jsonResult = new JsonResult((TForm)null)
            {
                StatusCode = (int)HttpStatusCode.OK,
            };
            return jsonResult;
        }
    }
}