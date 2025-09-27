using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Infrastructure.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, (DateTime Time, int Count)> _requests = new();
        private readonly int _limit = 5; 
        private readonly TimeSpan _period = TimeSpan.FromSeconds(10); 

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var key = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var now = DateTime.UtcNow;

            var entry = _requests.GetOrAdd(key, _ => (now, 0));
            if ((now - entry.Time) > _period)
                entry = (now, 0);

            entry = (entry.Time, entry.Count + 1);
            _requests[key] = entry;

            if (entry.Count > _limit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Try again later.");
                return;
            }

            await _next(context);
        }
    }
}
