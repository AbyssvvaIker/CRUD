using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Serilog;
using System.Threading.Tasks;

namespace Warehouse.Common.Filters
{
    public class StopwatchFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            await next();
            stopwatch.Stop();
            var elapsedTime = stopwatch.ElapsedMilliseconds;
            Debug.WriteLine($"{elapsedTime}");
        }
    }
}
