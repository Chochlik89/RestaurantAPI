using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly Stopwatch _timer;
        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _timer.Start();

            await next.Invoke(context);

            _timer.Stop();

            TimeSpan timeTaken = _timer.Elapsed;


            if (timeTaken.TotalSeconds > 4)
            {
                _logger.LogInformation($"czasownik Http: {context.Request.Method}, ścieżka: {context.Request.Path}, czas: {timeTaken.TotalSeconds}s");

            }
        }
    }
}
