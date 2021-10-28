using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Xml;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.Linq;

namespace PTASSyncService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        private RsaSecurityKey key;

        private TokenAuthOptions tokenOptions;

        private Settings settings;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            // ********************
            // Setup CORS
            // ********************
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            //corsBuilder.AllowAnyOrigin(); // For anyone access.
            corsBuilder.WithOrigins("http://localhost", "http://35.167.242.10"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });

            services.AddMvc();

            var rsa = RSA.Create();
            var parameters = FromXmlString(Configuration["appSettings:JWKPublicPrivateToken"]);
            rsa.ImportParameters(parameters);
            key = new RsaSecurityKey(rsa);
            string TokenAudience = Configuration["appSettings:JWKAudience"];
            string TokenIssuer = Configuration["appSettings:JWKIssuer"];
            tokenOptions = new TokenAuthOptions()
            {
                audience = TokenAudience,
                issuer = TokenIssuer,
                signingCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
            };

            settings = new Settings()
            {
                connectionString = Environment.GetEnvironmentVariable("connectionString"),
                backendConnectionString = Environment.GetEnvironmentVariable("backendConnectionString"),
                passwordVerificationUrl = Configuration["Settings:passwordVerificationUrl"],
                documentPath = Configuration["Settings:DocumentPath"],
                logLevel = int.Parse(Configuration["Settings:LogLevel"]),
                logFile = Configuration["Settings:LogFile"],
                chunkSize = int.Parse(Configuration["Settings:ChunkSize"])
            };


            services.AddSingleton(_ => Configuration);

            // Save the token options into an instance so they're accessible to the 
            // controller.
            services.AddSingleton<TokenAuthOptions>(tokenOptions);
            services.AddSingleton<Settings>(settings);

            // Enable the use of an [Authorize("Bearer")] attribute on methods and
            // classes to protect.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IncludeXmlComments(string.Format(@"{0}\PTASSyncService.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "PTAS Sync Services",
                    Description = "API For PTAS Sync Services",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            /*

               app.UseJwtBearerAuthentication(new JwtBearerOptions
               {
                   TokenValidationParameters = new TokenValidationParameters
                   {
                       IssuerSigningKey = key,
                       ValidAudience = tokenOptions.audience,
                       ValidIssuer = tokenOptions.issuer,

                       // When receiving a token, check that it is still valid.
                       ValidateLifetime = true,

                       // This defines the maximum allowable clock skew - i.e.
                       // provides a tolerance on the token expiry time 
                       // when validating the lifetime. As we're creating the tokens 
                       // locally and validating them on the same machines which 
                       // should have synchronised time, this can be set to zero. 
                       // Where external tokens are used, some leeway here could be 
                       // useful.
                       ClockSkew = TimeSpan.FromMinutes(0)
                   }
               });
               */
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi Swagger API v1");
            });
            app.UseMvc();

            // ********************
            // USE CORS - might not be required.
            // ********************
            app.UseCors("SiteCorsPolicy");
        }

        public RSAParameters FromXmlString(string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            if (xmlString != null)
            {
                xmlDoc.LoadXml(xmlString);

                if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
                {
                    foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                            case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                            case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                            case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                            case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                            case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                            case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                            case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                        }
                    }
                }
                else
                {
                    throw new Exception("Invalid XML RSA key.");
                }

            }

            return parameters;
        }

    }

    public class TokenAuthOptions
    {
        public string audience;
        public string issuer;
        public SigningCredentials signingCredentials;
    }

    public class Settings
    {
        public string connectionString;
        public string backendConnectionString;
        public string passwordVerificationUrl;
        public string connectorServiceUrl;
        public string documentPath;
        public string logFile;
        public int logLevel;
        public int chunkSize;
    }
}
