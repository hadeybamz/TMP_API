using System.Net;
using TMP_API.Models;

namespace TMP_API.Helpers
{
    public class RateLimitingMiddleware
    {
        private RequestDelegate _next;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));

        }
        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var decorator = endpoint?.Metadata.GetMetadata<LimitRequests>();
            if (decorator is null)
            {
                await _next(context);
                return;
            }
            var key = GenerateClientKey(context);
            var clientStatistics = await GetClientStatisticsByKey(key);
            if (clientStatistics != null &&
                   DateTime.UtcNow < clientStatistics.LastSuccessfulResponseTime.AddSeconds(decorator.TimeWindow) &&
                   clientStatistics.NumberOfRequestsCompletedSuccessfully == decorator.MaxRequests)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                return;
            }
            await UpdateClientStatisticsStorage(key, decorator.MaxRequests);
            await _next(context);
        }

        private static string GenerateClientKey(HttpContext context) => $"{context.Request.Path}_{context.Connection.RemoteIpAddress}";

        private async Task<ClientStatistics> GetClientStatisticsByKey(string key)
        {
            return await _cache.GetCacheValueAsync<ClientStatistics>(key);
        }
    }
}
