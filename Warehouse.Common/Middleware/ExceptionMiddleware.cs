using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace Warehouse.Common.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next)
        {

            _next = next;
            _logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.File(@"Warehouse.Api_log.txt", rollingInterval: RollingInterval.Hour)
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
                throw;
            }
        }

        
    }
}
