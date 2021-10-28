// <copyright file="Startup.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore
{
    using System;

    using AutoMapper;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json.Serialization;

    using PTASCRMHelpers;

    using PTASDynamicsCrudCore.Mappings;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Implementations;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// Startup class to initialize the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">Configuration object to get the params from.</param>
        public Startup(IHostingEnvironment env)
        {
            var appSettingsName = env.IsProduction() ? $"appsettings.json" : $"appsettings.{env.EnvironmentName}.json";
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(
                appSettingsName,
                optional: false,
                reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            this.Configuration = builder.Build();
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime to add services to the container.
        /// </summary>
        /// <param name="services">Services to configure.</param>
        /// <seealso cref="MappingProfile"/>
        /// <remarks>
        /// very important: any type that is read as form values and is stored as json and viceversa
        /// must be initialized in class mapping profile.
        /// </remarks>l
        ///// <seealso cref="MappingProfile"/>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(new Type[] { typeof(MappingProfile) });
            services.AddMvc()
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
              .AddJsonOptions(options =>
              {
                  options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                  options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
              });

            // create injection objects.
            var configParams = new ConfigParams(this.Configuration);
            services.AddSingleton<IConfigurationParams>(configParams);
            TokenManager tokenManager = new TokenManager();
            services.AddSingleton<ITokenManager>(tokenManager);
            CRMWrapper wrapper = new CRMWrapper(configParams, tokenManager);
            services.AddSingleton(wrapper);
            services.AddSingleton<IContactManager>(new ContactManager(wrapper));
            services.AddSingleton<IParcelManager>(new ParcelManager(wrapper, configParams));
            services.AddSingleton<IFileAttachmentMetadataManager>(new FileAttachmentMetadataManager(wrapper));
            services.AddSingleton<ISEAppDetailManager>(new SEAppDetailManager(wrapper));
            services.AddSingleton<ISEAppFinancialManager>(new SEAppFinancialManager(wrapper));
            services.AddSingleton<ISEAppOccupantManager>(new SEAppOccupantManager(wrapper));
            services.AddSingleton<ISEAppOtherPropManager>(new SEAppOtherPropManager(wrapper));
            services.AddSingleton<ISEAppNoteManager>(new SEAppNoteManager(wrapper));

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.IncludeXmlComments(string.Format(@"{0}\ReferenceProject.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "PTAS Data Services",
                    Description = "API For PTAS Data Services",
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">This application builder.</param>
        /// <param name="env">Environment.</param>
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