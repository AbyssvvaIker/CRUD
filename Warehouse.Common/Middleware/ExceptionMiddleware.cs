using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Warehouse.Common.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Something went wrong: {ex.Message}");
                context.Response.StatusCode = 500;
                throw;
            }
        }


    }
}
