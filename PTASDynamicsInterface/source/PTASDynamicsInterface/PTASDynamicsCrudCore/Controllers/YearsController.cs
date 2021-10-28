// <copyright file="YearsController.cs" company="King County">
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
    /// Controller to generate the R in ptas_years.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class YearsController : AbstractReadonlyController<OutgoingYear, DynamicsYear>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YearsController"/> class.
        /// </summary>
        /// <param name="wrapper">Wrapper for CRM access.</param>
        /// <param name="mapper">Automapper to convert types.</param>
        public YearsController(CRMWrapper wrapper, IMapper mapper)
          : base(wrapper, mapper)
        {
        }

        /// <inheritdoc/>
        protected override string TableName => "ptas_years";

        /// <inheritdoc/>
        protected override string GetOpQuery => "$top=1000";

        /// <summary>
        /// http get action.
        /// </summary>
        /// <returns>A list of option sets as defined in dynamics.</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "GetTheYearsList")]
        public async Task<ActionResult<IEnumerable<OutgoingYear>>> Get()
        {
            return await ControllerExtensions<IEnumerable<OutgoingYear>>.HandleExceptions(async () =>
            {
                return await this.AbstractGet();
            });
        }

        /// <summary>
        /// Sort the elements.
        /// </summary>
        /// <param name="input">Elements to be sorted.</param>
        /// <returns>Sorted elements.</returns>
        protected override IEnumerable<OutgoingYear> PostProcess(IEnumerable<OutgoingYear> input) => input.OrderBy(y => y.StartDate);
    }
}