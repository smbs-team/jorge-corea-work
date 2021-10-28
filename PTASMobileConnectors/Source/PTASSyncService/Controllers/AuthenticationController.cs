using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using PTASSyncService.Utilities;
using Npgsql;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using Swashbuckle.AspNetCore.Annotations;

namespace PTASSyncService.Controllers
{
    /// <summary>
    /// Class to handle authentication.
    /// </summary>

    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly TokenAuthOptions tokenOptions;
        private readonly Settings Configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tokenOptions">Token options</param>
        /// <param name="config">App configurations settings</param>
        public AuthenticationController(TokenAuthOptions tokenOptions, Settings config)
        {
            this.tokenOptions = tokenOptions;
            this.Configuration = config;
        }

        /// <summary>
        /// Check if currently authenticated. Will throw an exception of some sort which should be caught by a general
        /// exception handler and returned to the user as a 401, if not authenticated. Will return a fresh token if
        /// the user is authenticated, which will reset the expiry.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        //[Authorize("Bearer")]
        [SwaggerOperation(OperationId = "AuthenticationGet")]
        public dynamic Get()
        {
            /* 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            THIS METHOD ALLOWS A USER WITH A VALID TOKEN TO REMAIN LOGGED IN 
            FOREVER, WITH NO WAY OF EVER EXPIRING THEIR RIGHT TO USE THE 
            APPLICATION. Consider whether this is appropriate for your 
            application's needs.
            Refresh Tokens (see https://auth0.com/docs/refresh-token) could be used to 
            retrieve new tokens. 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            */
            bool authenticated = false;
            string user = null;
            int entityId = -1;
            string token = null;
            DateTime? tokenExpires = default(DateTime?);
            var currentUser = HttpContext.User;
            if (currentUser != null)
            {
                authenticated = currentUser.Identity.IsAuthenticated;
                if (authenticated)
                {
                    user = currentUser.Identity.Name;
                    foreach (Claim c in currentUser.Claims) if (c.Type == "EntityID") entityId = Convert.ToInt32(c.Value);
                    tokenExpires = DateTime.UtcNow.AddDays(3);
                    token = GetToken(currentUser.Identity.Name, tokenExpires);
                }
            }
            return new { authenticated = authenticated, user = user, entityId = entityId, token = token, tokenExpires = tokenExpires };
        }



        public class AuthRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }


        /// <summary>
        /// Request a new token for a given username/password pair.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("SiteCorsPolicy")]
        [SwaggerOperation(OperationId = "AuthenticationPost")]
        public dynamic Post([FromBody] AuthRequest req)
        {
            try
            {
                string backConnStr = Environment.GetEnvironmentVariable("backendConnectionString");
                string sqlToken = SQLTokenUtility.GetSQLToken(backConnStr);
                using (SqlConnection cnn = new SqlConnection(backConnStr))
                {
                    cnn.AccessToken = sqlToken;
                    //Read the changesetID related with the download.
                    using (SqlCommand cmd = new SqlCommand("Select user_id, password From users Where email = '" + req.username +  "'", cnn))
                    {
                        string password = null;
                        long userid = 0;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = cnn.ConnectionTimeout;

                        cnn.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            userid = (Int64)rdr["user_id"];
                            password = (string)rdr["password"];
                        }
                        cnn.Close();

                        if (password != null)
                        {
                            if (VerifyPassword(req.password, password).Result)
                            {
                                DateTime? expires = DateTime.UtcNow.AddDays(3);
                                var token = GetToken(req.username, expires);
                                if (token != null)
                                    return new { authenticated = true, entityId = 1, token = token, expire = expires, userId = userid };
                            }
                            else
                            {
                                return new { authenticated = false };
                            }
                        }
                        else {
                            return new { authenticated = false };
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return new { authenticated = false };
            }
        }

        private async Task<bool> VerifyPassword(string password, string hashed_password)
        {
            // This function performs the same validation that PHP's password_verify() does:
            var validPassword = BCrypt.Net.BCrypt.Verify(password, hashed_password);

            return validPassword;

            // The old code will be kept here in case anything about password validation changes:
            /*bool validPassword = false;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

                list.Add(new KeyValuePair<string, string>("password", password));
                list.Add(new KeyValuePair<string, string>("hashed_password", hashed_password));

                HttpResponseMessage response = await client.PostAsync(Configuration.passwordVerificationUrl, new FormUrlEncodedContent(list));
                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadAsStringAsync();
                    if (result == "true")
                        return true;
                }
            }
            return validPassword;*/
        }


        private string GetToken(string user, DateTime? expires)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                // Here, you should create or look up an identity for the user which is being authenticated.
                // For now, just creating a simple generic identity.
                ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user, "TokenAuth"), new[] { new Claim("EntityID", "1", ClaimValueTypes.Integer), new Claim("userName", user) });
                var securityToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                {
                    Issuer = tokenOptions.issuer,
                    Audience = tokenOptions.audience,
                    SigningCredentials = tokenOptions.signingCredentials,
                    Subject = identity,
                    Expires = expires
                });

                return handler.WriteToken(securityToken);
            }catch(Exception ex)
            {
                LogClass.ReportError("AuthenticationController", "GetToken", ex, Configuration);
            }
            return null;
        }
    }

    
}
