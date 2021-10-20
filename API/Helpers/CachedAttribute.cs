using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IResponseCacheService service = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            string cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            string response = await service.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(response))
            {
                ContentResult result = new ContentResult
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = result;
                return;
            }

            ActionExecutedContext executedContext = await next();
            if (executedContext.Result is OkObjectResult okObjectResult)
                await service.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{request}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
                builder.Append($"|{key}-{value}");

            return builder.ToString();
        }
    }
}