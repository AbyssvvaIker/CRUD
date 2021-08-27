using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Serilog;
using System.Threading.Tasks;
using Serilog;

namespace Warehouse.Common.Filters
{
    public class StopwatchFilter : IAsyncActionFilter
    {
        protected readonly ILogger _logger;
        public StopwatchFilter(ILogger logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            await next();
            stopwatch.Stop();
            var elapsedTime = stopwatch.ElapsedMilliseconds;
            var log = $"Action: {context.ActionDescriptor.DisplayName} \n\t ExecutionTime: {elapsedTime} ms";
            Debug.WriteLine(log);
            _logger.Debug(log);
        }
    }
}
