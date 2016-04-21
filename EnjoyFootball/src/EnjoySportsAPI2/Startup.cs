using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Http;
using EnjoyFootball.Models;
using Microsoft.Data.Entity;

namespace EnjoySportsAPI2
{
    public class Startup
    {
        string connString = @"Data Source=ACADEMY030;Initial Catalog=EnjoyFootball2;Integrated Security=True;Pooling=False; MultipleActiveResultSets=True";

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddEntityFramework().AddSqlServer().AddDbContext<IdentityDbContext>(o => o.UseSqlServer(connString));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();

            app.UseIISPlatformHandler();

            app.UseIdentity()
                .UseCookieAuthentication(o =>
                {
                    o.AutomaticChallenge = true;
                    o.LoginPath = new PathString("/account/fuckoff/");
                });

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();


            app.UseMvcWithDefaultRoute();
            //app.UseMvc();
        }
        // This method gets called by the runtime. Use this method to add services to the container
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    // Add framework services.
        //    services.AddApplicationInsightsTelemetry(Configuration);

        //    services.AddMvc();
        //}

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        //{
        //    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        //    loggerFactory.AddDebug();

        //    app.UseIISPlatformHandler();

        //    app.UseApplicationInsightsRequestTelemetry();

        //    app.UseApplicationInsightsExceptionTelemetry();

        //    app.UseStaticFiles();

        //    app.UseMvc();
        //}

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
