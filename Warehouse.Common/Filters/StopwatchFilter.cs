using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Diagnostics;
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
            var log = $"Action: {context.ActionDescriptor.DisplayName} \n\t ExecutionTime: {elapsedTime} ms";
            Log.Error(log);
        }
    }
}
