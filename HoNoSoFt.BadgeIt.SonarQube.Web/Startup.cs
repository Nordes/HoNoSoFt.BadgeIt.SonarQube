using HoNoSoFt.BadgeIt.SonarQube.Web.Configurations;
using HoNoSoFt.BadgeIt.SonarQube.Web.Logics;
using HoNoSoFt.BadgeIt.SonarQube.Web.Logics.Interfaces;
using HoNoSoFt.BadgeIt.SonarQube.Web.Models;
using HoNoSoFt.BadgeIt.SonarQube.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace HoNoSoFt.BadgeIt.SonarQube.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpClient();

            services.Configure<SonarQubeApiConfig>(Configuration.GetSection("SonarQubeApi"));
            services.Configure<BadgeApiConfig>(Configuration.GetSection("BadgeApi"));
            services.AddScoped<ISonarQubeSvc, SonarQubeSvc>();
            services.AddScoped<IShieldIoSvc, ShieldIoSvc>();

            services.AddSingleton<IExtractor<ShieldsColors>, SonarQubeColorExtractor>();
            services.AddSingleton<IExtractor<string>, SonarQubeValueExtractor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var sonarConfig = app.ApplicationServices.GetService<IOptions<SonarQubeApiConfig>>();
            var badgesConfig = app.ApplicationServices.GetService<IOptions<BadgeApiConfig>>();
            sonarConfig.Value.LoadFromEnvironmentVariables();
            badgesConfig.Value.LoadFromEnvironmentVariables();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
