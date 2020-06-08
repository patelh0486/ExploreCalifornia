using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreTexas.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.StaticFiles;


namespace ExploreTexas
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FormattingService>();
            services.AddTransient<FetureToggles>(x => new FetureToggles
            {
                DeveloperExceptions = configuration.GetValue<bool>("FeatureToggles:DeveloperExceptions")
            });

            services.AddDbContext<BlogDataContext>(options =>
            {
                var connectionstring = configuration.GetConnectionString("BlogDataContext");
                options.UseSqlServer(connectionstring);
            });
            services.AddDbContext<IdentityDataContext>(options =>
            {
                var connectionstring = configuration.GetConnectionString("IdentityDataContext");
                options.UseSqlServer(connectionstring);
            });
            /*services.AddDbContext<ImageDbContext>(options =>
            {
               var connectionstring  = configuration.GetConnectionString("ImageDbContext");
                options.UseSqlServer(connectionstring);
            });*/

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>();
                
            services.AddMvc(option => option.EnableEndpointRouting = false);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            FetureToggles feature)
        {
            app.UseExceptionHandler("/error.html");
            if (feature.DeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }
            // demonstrating error / error handling
             app.UseRouting();
            app.Use(async (context, next) =>
            {
                if(context.Request.Path.Value.StartsWith("invalid"))
                {
                    throw new Exception("Error");
                }
                await next();
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default",
                    "{controller=Home}/{action=Index}/{id?}"
                    );
            });

            app.UseFileServer();
        }
    }
}
