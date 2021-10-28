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
    /// Controller for handling bookmarks in dynamics.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class BookmarkTagsController : CommonAPIController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkTagsController"/> class.
        /// </summary>
        /// <param name="wrapper">CRM.</param>
        public BookmarkTagsController(CRMWrapper wrapper)
            : base(wrapper)
        {
        }

        /// <summary>
        /// Get the bookmarks for this parcel.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [SwaggerOperation(OperationId = "GetAllBookmarkTags")]
        [HttpGet]
        public async Task<ActionResult> GetAllBookmarTags()
        {
            try
            {
                var r = await this.Wrapper.ExecuteGet<BookmarkTag>("ptas_bookmarktags", "$filter=statecode  eq 0 and statuscode eq 1");
                return new OkObjectResult(new { results = r });
            }
            catch (Exception ex)
            {
                return this.ReportError(ex);
            }
        }

        private class BookmarkTag
        {
            [JsonProperty("ptas_bookmarktagid")]
            public string PtasBookmarktagid { get; set; }

            [JsonProperty("ptas_name")]
            public string PTASName { get; set; }
        }
    }
}