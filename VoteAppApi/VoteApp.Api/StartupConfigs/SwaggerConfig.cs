using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace VoteApp.Api.StartupConfigs
{
    public static class SwaggerConfig
    {
        public static void AddVoteAppSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VoteApp API",
                    Description = "VoteApp API",
                    Contact = new OpenApiContact
                    {
                        Name = "Tomasz Broniewski",
                        Email = "tomasz.broniewski@gmail.com"
                    }
                });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.CustomSchemaIds(type => type.ToString());
            });
        }
    }
}
