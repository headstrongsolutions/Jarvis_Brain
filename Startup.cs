using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Jarvis_Brain.Models;
using Jarvis_Brain.Services;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.HttpOverrides;

namespace Jarvis_Brain
{
    /// <summary>
    /// Startup class for the Jarvis Brain application
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration property
        /// </summary>
        /// <value></value>
        public IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <Description>
        /// Raises up the base path and then binds in the configuration file
        /// </Description>
        /// <param name="env">Hosting Environment information</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json");

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// The Configure Services method for the Startup class
        /// </summary>
        /// <Description>
        /// This method sets the configuration for the application,
        /// implementing DI for services, setting up the DB Context and
        /// rigging up default settings for the underlying MVC framework
        /// for JSON serialisation, compatibility settings etc 
        /// </Description>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Bind in the DBContext, ready to be hoofed into stuff
            services.AddDbContext<Jarvis_BrainDBContext>(options =>
                options.UseSqlite(Configuration["Production:SqliteConnectionString"])
            );

            // DI up all our things!!!1
            services.AddScoped<IDHTService, DHTService>();

            // CORS policies
            services.AddCors(o => o.AddPolicy("GardenTemperatureSensorHost", builder =>
            {
                builder.WithOrigins(Configuration["GardenTemperatureSensorHost"])
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Finally, run up the MVC framework
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// Configures the underlying principles for the MVC framework
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            // Patch path base with forwarded path
            app.Use(async (context, next) =>
            {
                var forwardedPath = context.Request.Headers["X-Forwarded-Path"].ToString();
                if (!string.IsNullOrEmpty(forwardedPath))
                {
                    context.Request.PathBase = forwardedPath;
                }

                await next();
            });

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            app.UseStaticFiles();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseMvcWithDefaultRoute();
        }
    }
}
