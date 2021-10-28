// <copyright file="AbstractReadonlyController{TForm,TDynamics}.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for items that only have a get command.
    /// </summary>
    /// <typeparam name="TForm">Incomming data.</typeparam>
    /// <typeparam name="TDynamics">Dynamics representation.</typeparam>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public abstract class AbstractReadonlyController<TForm, TDynamics> : ControllerBase
    where TForm : class
    where TDynamics : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractReadonlyController{TForm, TDynamics}"/> class.
        /// </summary>
        /// <param name="wrapper">CRM wrapper.</param>
        /// <param name="mapper">Automapper.</param>
        public AbstractReadonlyController(CRMWrapper wrapper, IMapper mapper)
        {
            this.Wrapper = wrapper;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Gets name of the referenced table.
        /// </summary>
        protected abstract string TableName { get; }

        /// <summary>
        /// Gets the query operation for the get.
        /// </summary>
        protected abstract string GetOpQuery { get; }

        /// <summary>
        /// Gets current CRM Wrapper.
        /// </summary>
        protected CRMWrapper Wrapper { get; }

        /// <summary>
        /// Gets current Automapper.
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// Gets a list of years.
        /// </summary>
        /// <returns>An array of years.</returns>
        protected async Task<ActionResult<IEnumerable<TForm>>> AbstractGet()
        {
            var result = (await this.Wrapper.ExecuteGet<TDynamics>(this.TableName, $"{this.GetOpQuery}"))
              .Select(item => this.Mapper.Map<TForm>(item)).ToList();
            JsonResult jResult = new JsonResult(this.PostProcess(result))
            {
                StatusCode = (int)HttpStatusCode.OK,
            };
            return jResult;
        }

        /// <summary>
        /// Do after items fetched. Could sort or filter.
        /// </summary>
        /// <param name="input">Items to process.</param>
        /// <returns>Processed items. In this class it is just the items themselves.</returns>
        protected virtual IEnumerable<TForm> PostProcess(IEnumerable<TForm> input) => input;
    }
}