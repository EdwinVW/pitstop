using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Pitstop.Models;
using Pitstop.ViewModels;
using System;
using WebApp.Commands;
using WebApp.RESTClients;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.HealthChecks;
using System.Threading.Tasks;

namespace PitStop
{
    public class Startup
    {
        private IHostingEnvironment CurrentEnvironment { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            CurrentEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // add custom services
            services.AddTransient<ICustomerManagementAPI, CustomerManagementAPI>();
            services.AddTransient<IVehicleManagementAPI, VehicleManagementAPI>();
            services.AddTransient<IWorkshopManagementAPI, WorkshopManagementAPI>();

            services.AddHealthChecks(checks =>
            {
                checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            SetupAutoMapper();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void SetupAutoMapper()
        {
            // setup automapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Customer, RegisterCustomer>()
                    .ForCtorParam("messageId", opt => opt.ResolveUsing(c => Guid.NewGuid()))
                    .ForCtorParam("customerId", opt => opt.ResolveUsing(c => Guid.NewGuid()));
                cfg.CreateMap<Vehicle, RegisterVehicle>()
                    .ForCtorParam("messageId", opt => opt.ResolveUsing(c => Guid.NewGuid()));
                cfg.CreateMap<VehicleManagementNewViewModel, RegisterVehicle>().ConvertUsing((vm, rv) =>
                    new RegisterVehicle(Guid.NewGuid(), vm.Vehicle.LicenseNumber, vm.Vehicle.Brand, vm.Vehicle.Type, vm.SelectedCustomerId));
            });
        }
    }
}
