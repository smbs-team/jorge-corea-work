// <copyright file="ContactsController.cs" company="King County">
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
    using PTASDynamicsCrudHelperClasses.Implementations;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Api controller for contact crud.
    /// </summary>
    [Route("v1/api/contacts")]
    [ApiController]
    public class ContactsController : AbstractCrudController<FormContact, DynamicsContact>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsController"/> class.
        /// </summary>
        /// <param name="wrapper">CRM wrapper.</param>
        /// <param name="mapper">Automapper.</param>
        public ContactsController(CRMWrapper wrapper, IMapper mapper)
          : base(wrapper, mapper)
        {
        }

        /// <inheritdoc/>
        protected override string TableName => "contacts";

        /// <inheritdoc/>
        protected override string GetOpQuery => "$top=1000";

        /// <inheritdoc/>
        protected override string KeyField => "contactid";

        /// <summary>
        /// http get action.
        /// </summary>
        /// <param name="id">Id of the Contact to fetch.</param>
        /// <returns>A list of option sets as defined in dynamics.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetTheContactById")]
        public async Task<ActionResult<FormContact>> Get(string id)
        {
            return await ControllerExtensions<FormContact>.HandleExceptions(async () =>
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
        /// <returns>A FormContact<see cref="FormContact"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "PostContacts")]
        public async Task<ActionResult<FormContact>> Post([FromBody] FormContact value)
        {
            return await ControllerExtensions<FormContact>.HandleExceptions(async () =>
            {
                ContactManager contactManager = new ContactManager(this.Wrapper);
                var found = await contactManager.GetContactFromEmail(value.EmailAddress);
                if (found != null)
                {
                    throw new DynamicsInterfaceException(
                        $"Email {value.EmailAddress} already exists.", (int)HttpStatusCode.Conflict, null);
                }

                return await this.AbstractPost(value);
            });
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="value">Form values of entity.</param>
        /// <returns>A FormContact<see cref="FormContact"/> representing the asynchronous operation.</returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(OperationId = "PatchContacts")]
        public virtual async Task<ActionResult<FormContact>> Patch(string id, [FromBody] FormContact value)
        {
            return await ControllerExtensions<FormContact>.HandleExceptions(async () =>
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
        [SwaggerOperation(OperationId = "DeleteContacts")]
        public virtual async Task<ActionResult<FormContact>> Delete(string id)
        {
            return await ControllerExtensions<FormContact>.HandleExceptions(async () => await this.AbstractDelete(id));
        }
    }
}