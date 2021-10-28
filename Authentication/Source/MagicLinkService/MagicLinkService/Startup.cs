//-----------------------------------------------------------------------
// <copyright file="Startup.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MagicLinkService
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using PTAS.MagicLinkService.Models;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// Web project startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile(
                   "appsettings.json",
                   optional: false,
                   reloadOnChange: true)
               .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            // Sets the configuration.
            this.Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Sample: Load the app settings section and bind to AppSettingsModel object graph
            services.Configure<AppSettingsModel>(this.Configuration);

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.IncludeXmlComments(string.Format(@"{0}\PTAS.MagicLinkService.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "PTAS Magic Link Service",
                    Description = "Identity API For PTAS Magic Link Service",
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi Swagger API v1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
