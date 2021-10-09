using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {

        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1" });

                OpenApiSecurityScheme scheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorizacion",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", scheme);

                OpenApiSecurityRequirement requirement = new OpenApiSecurityRequirement { { scheme, new[] { "Bearer" } } };
                c.AddSecurityRequirement(requirement);
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