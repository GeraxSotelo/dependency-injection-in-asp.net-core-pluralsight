﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TennisBookings.Web.Services;
using TennisBookings.Web.Configuration;


namespace TennisBookings.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Register application services
        public void ConfigureServices(IServiceCollection services)
        {
            // 1st param is the abstract type.
            // 2nd param is the implementation type (concrete weatherforecaster class). Type of the object the container will create and return when resolving a request for an IWeatherForecaster
            services.AddTransient<IWeatherForecaster, WeatherForecaster>();

            // Bind the configuration to POCO class in order to use it with the options pattern.
            // Combine a section of your configuration to your options class (FeaturesConfiguration) using the ConfigureExtension method
            services.Configure<FeaturesConfiguration>(Configuration.GetSection("Features"));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseMvc();
        }
    }
}
