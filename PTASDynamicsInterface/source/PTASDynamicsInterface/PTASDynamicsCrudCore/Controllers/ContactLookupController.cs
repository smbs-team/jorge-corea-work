// <copyright file="ContactLookupController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Exception;
    using PTASDynamicsCrudHelperClasses.Interfaces;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to lookup a contact by email or token.
    /// </summary>
    [Route("v1/api/contactlookup")]
    [ApiController]
    public class ContactLookupController : ControllerBase
    {
        private readonly IContactManager contactManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactLookupController"/> class.
        /// </summary>
        /// <param name="contactManager">To get the contact info.</param>
        /// <param name="mapper">Automapper for conversions.</param>
        public ContactLookupController(IContactManager contactManager, IMapper mapper)
        {
            this.contactManager = contactManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a contact from the email (id).
        /// </summary>
        /// <param name="id">Contact email.</param>
        /// <returns>The contact id.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetContactByEmail")]
        public async Task<ActionResult<FormContact>> Get(string id)
        {
            return await ControllerExtensions<FormContact>.HandleExceptions(async () =>
            {
                var result = await this.contactManager.GetContactFromEmail(id);

                if (result == null)
                {
                    throw new DynamicsInterfaceException(
                        $"Request returns not data on reading Contact by Email with key {id}.",
                        (int)HttpStatusCode.NotFound,
                        null);
                }

                return new JsonResult(this.mapper.Map<FormContact>(result));
            });
        }
    }
}