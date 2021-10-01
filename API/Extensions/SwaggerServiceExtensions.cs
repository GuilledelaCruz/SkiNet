using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API.Exceptions
{
    public static class SwaggerServiceExtensions
    {

        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1" });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerApp(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

            return app;
        }
    }
}