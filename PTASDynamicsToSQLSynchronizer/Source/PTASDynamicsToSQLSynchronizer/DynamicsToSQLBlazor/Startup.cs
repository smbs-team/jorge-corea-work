// <copyright file="Startup.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using BlazorStrap;
    using D2SSyncHelpers.Interfaces;
    using D2SSyncHelpers.Models;
    using D2SSyncHelpers.Services;
    using Dapper;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// App startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">App configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets application global config.
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        /// <summary>
        /// Configure App Services.
        /// </summary>
        /// <param name="services">Services to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => this.Configuration.Bind("AzureAd", options));

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddTransient<IDataAccessLibrary, DataAccessLibrary>();
            services.AddTransient<DBTablesService>();
            services.AddTransient<MetadataFromDynamicsService>();
            services.AddTransient<IAsyncStringReader, DynamicsMetadataReader>();
            services.AddTransient<IDynamicsRelationsLoader, DynamicsRelationsLoader>();
            services.AddBootstrapCSS();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        /// <summary>
        /// Configures the app.
        /// </summary>
        /// <param name="app">App to configure.</param>
        /// <param name="env">Environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            // Configure dapper sql mapper for our classes to use the column name decorator.
            ConfigureMapper(typeof(DBTable), typeof(DBField));
        }

        private static void ConfigureMapper(params Type[] types)
        {
            types.ToList().ForEach(tableType =>
            {
                SqlMapper.SetTypeMap(
                    tableType,
                    new CustomPropertyTypeMap(
                        tableType,
                        (type, columnName)
                            => type.GetProperties()
                                .FirstOrDefault(prop
                                => prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
            });
        }
    }
}
