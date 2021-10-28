// <copyright file="SEAppPredefNotesController.cs" company="King County">
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
    /// Controller to generate the Read of the list of all Senior Exemption Application Predefined Notes.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SEAppPredefNotesController : AbstractReadonlyController<FormSEAppPredefNote, DynamicsSEAppPredefNote
        >
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SEAppPredefNotesController"/> class.
        /// </summary>
        /// <param name="wrapper">Wrapper for CRM access.</param>
        /// <param name="mapper">Automapper to convert types.</param>
        public SEAppPredefNotesController(CRMWrapper wrapper, IMapper mapper)
          : base(wrapper, mapper)
        {
        }

        /// <inheritdoc/>
        protected override string TableName => "ptas_seapppredefnoteses";

        /// <inheritdoc/>
        protected override string GetOpQuery => "$top=1000";

        /// <summary>
        /// http get action.
        /// </summary>
        /// <returns>A list of option sets as defined in dynamics.</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "GetTheSeniorExemptionApplicationPredefinedNotesList")]
        public async Task<ActionResult<IEnumerable<FormSEAppPredefNote>>> Get()
        {
            return await ControllerExtensions<IEnumerable<FormSEAppPredefNote>>.HandleExceptions(async () =>
            {
                return await this.AbstractGet();
            });
        }

        /// <summary>
        /// Sort the elements.
        /// </summary>
        /// <param name="input">Elements to be sorted.</param>
        /// <returns>Sorted elements.</returns>
        protected override IEnumerable<FormSEAppPredefNote> PostProcess(IEnumerable<FormSEAppPredefNote> input) => input.OrderBy(y => y.Name);
    }
}