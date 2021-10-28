namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    using PTASDynamicsCrudHelperClasses.Classes;

    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Common API controller.
    /// </summary>
    public class CommonAPIController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonAPIController"/> class.
        /// </summary>
        /// <param name="wrapper">CRM Wrapper.</param>
        public CommonAPIController(CRMWrapper wrapper)
        {
            this.Wrapper = wrapper;
        }

        /// <summary>
        /// Gets or sets cRM.
        /// </summary>
        protected CRMWrapper Wrapper { get; set; }

        /// <summary>
        /// Reports an error.
        /// </summary>
        /// <param name="ex">Exception to report about.</param>
        /// <returns>Object result for http.</returns>
        protected ObjectResult ReportError(Exception ex)
            => ex.Message.StartsWith("{") ?
            this.StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.DeserializeObject(ex.Message)) :
            this.StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message,
            });
    }
}