using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Warehouse.Common.Middleware;
using Warehouse.Infrastructure.DataAccess;

namespace Warehouse.Api
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
            services.AddControllers();
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            AddSwagger(ref services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            UseSwagger(ref app);
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);
        }

        private void AddSwagger(ref IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Warehouse.Api",
                        Version = "v1"
                    });
                x.CustomSchemaIds((type) => type.FullName);

                var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
                x.DescribeAllParametersInCamelCase();
            });
        }
        private void UseSwagger(ref IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            x.SwaggerEndpoint("../swagger/v1/swagger.json", "API V1"));
        }
    }
}
