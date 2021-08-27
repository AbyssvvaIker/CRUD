using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;

namespace Warehouse.Common.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public ExceptionMiddleware(RequestDelegate next, IConfiguration config)
        {
            _config = config;
            _next = next;

            string path = _config.GetValue<string>(
                "ExceptionMiddleware:Path");

            _logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.File(path, rollingInterval: RollingInterval.Hour)
                .CreateLogger();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Something went wrong: {ex.Message}");
                context.Response.StatusCode = 500;
                throw;
            }
        }

        
    }
}
