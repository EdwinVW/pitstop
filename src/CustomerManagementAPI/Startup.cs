using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Pitstop.CustomerManagementAPI.DataAccess;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Pitstop.CustomerManagementAPI.Model;
using Pitstop.Infrastructure.Messaging;
using System;
using Pitstop.CustomerManagementAPI.Events;
using Pitstop.CustomerManagementAPI.Commands;

namespace Pitstop.CustomerManagementAPI
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add DBContext classes
            var sqlConnectionString = Configuration.GetConnectionString("CustomerManagementCN");
            services.AddDbContext<CustomerManagementDBContext>(options => options.UseSqlServer(sqlConnectionString));

            // add messagepublisher classes
            var configSection = Configuration.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];
            services.AddTransient<IMessagePublisher>((sp) => new RabbitMQMessagePublisher(host, userName, password, "Pitstop"));

            // Add framework services.
            services.AddMvc();
            
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CustomerManagement API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CustomerManagementDBContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            SetupAutoMapper();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerManagement API - v1");
            });
        }

        private void SetupAutoMapper()
        {
            // setup automapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RegisterCustomer, Customer>();
                cfg.CreateMap<Customer, RegisterCustomer>()
                    .ForCtorParam("messageId", opt => opt.ResolveUsing(c => Guid.NewGuid()));
                cfg.CreateMap<RegisterCustomer, CustomerRegistered>()
                    .ForCtorParam("messageId", opt => opt.ResolveUsing(c => Guid.NewGuid()));
            });
        }
    }
}
