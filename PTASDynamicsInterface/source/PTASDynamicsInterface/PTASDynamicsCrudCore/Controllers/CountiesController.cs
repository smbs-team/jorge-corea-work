// <copyright file="CountiesController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to return counties from dynamics.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class CountiesController : AbstractReadonlyController<OutgoingCounty, DynamicsCounty>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountiesController"/> class.
        /// </summary>
        /// <param name="crmWrapper">Injected CRM wrapper.</param>
        /// <param name="mapper">Automapper to convert types.</param>
        public CountiesController(CRMWrapper crmWrapper, IMapper mapper)
          : base(crmWrapper, mapper)
        {
        }

        /// <inheritdoc/>
        protected override string TableName => "ptas_counties";

        /// <inheritdoc/>
        protected override string GetOpQuery => "$top=1000";

        /// <summary>
        /// http get action.
        /// </summary>
        /// <returns>A list of option sets as defined in dynamics.</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "GetTheCountiesList")]
        public async Task<ActionResult<IEnumerable<OutgoingCounty>>> Get()
        {
            return await ControllerExtensions<IEnumerable<OutgoingCounty>>.HandleExceptions(async () =>
            {
                return await this.AbstractGet();
            });
        }

        /// <summary>
        /// Sort the counties.
        /// </summary>
        /// <param name="input">Unsorted counties.</param>
        /// <returns>Sorted counties.</returns>
        protected override IEnumerable<OutgoingCounty> PostProcess(IEnumerable<OutgoingCounty> input) => input.OrderBy(county => county.Name);
    }
}