namespace PTASDynamicsCrudCore.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PTASCRMHelpers.Exception;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// User information controller.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        private readonly CRMWrapper wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemUserController"/> class.
        /// </summary>
        /// <param name="wrapper">CRM wrapper.</param>
        public SystemUserController(CRMWrapper wrapper)
        {
            this.wrapper = wrapper;
        }

        /// <summary>
        /// Get User info.
        /// </summary>
        /// <returns>User info.</returns>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var email = JWTDecoder.GetEmailFromHeader(this.HttpContext);
            DynamicsSystemUser[] users = await this.wrapper.ExecuteGet<DynamicsSystemUser>("systemusers", $"$filter=windowsliveid eq '{email}'");
            if (!users.Any())
            {
                throw new DynamicsHttpRequestException("Cannot find user info.", null);
            }

            return new OkObjectResult(users.First());
        }
    }
}