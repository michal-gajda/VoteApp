using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Reflection;
using VoteApp.Api.Helpers;
using VoteApp.Application;
using VoteApp.Domain;
using VoteApp.Infrastructure;
using VoteApp.Infrastructure.Database;
using VoteApp.Infrastructure.Database.Repositories;

namespace VoteApp.Api.StartupConfigs
{
    public static class DIConfig
    {
        public static void AddVoteAppDI(this IServiceCollection services, AppConfig appConfig)
        {
            AddDatabaseStack(services, appConfig);
            AddMediatRBehaviors(services);
            AddApplicationLayer(services);
            AddInfrastructureServices(services);
            AddPolly(services, appConfig);
            OtherItems(services);
        }

        private static void AddDomainLayer(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        public static void OtherItems(IServiceCollection services)
        {
            services.AddSingleton<ApplicationContext>();
            services.AddSingleton<IAppConfig, AppConfig>();

            services.AddScoped<IHandlerContext, HandlerContext>();
            services.AddScoped<ICurrentUser, HttpCurrentUser>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void AddMediatRBehaviors(IServiceCollection services)
        {
            //attention: here the order matters
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTransactionPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorsExecutionPipelineBehavior<,>));
        }

        private static void AddHostedServices(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        private static void AddDatabaseStack(IServiceCollection services, AppConfig appConfig)
        {
            services.AddDbContext<VoteAppDbContext>(options => options
                                            .UseSqlServer(appConfig.VoteAppConnectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

            var infraAssembly = Assembly.GetAssembly(typeof(VoteAppDbContext))!;

            services.AddScopedAllByTypeRule(infraAssembly,
                t => t.Name.EndsWith("Repository") && !t.IsAbstract && !t.IsInterface,
                i => i.Name.EndsWith("Repository"));

            services.AddScoped(typeof(IBaseRepository<>), typeof(Repository<>));
        }

        public static void AddApplicationLayer(IServiceCollection services)
        {
            var applicationAssembly = Assembly.GetAssembly(typeof(IHandlerContext))!;

            services.AddMediatR(applicationAssembly);

            services.AddScopedAllByTypeRule(applicationAssembly,
                t => t.Name.EndsWith("Service") && !t.IsAbstract && !t.IsInterface,
                i => i.Name.EndsWith("Service"));

            services.AddValidatorsFromAssembly(applicationAssembly);
        }

        private static void AddInfrastructureServices(IServiceCollection services)
        {
            var infraAssembly = Assembly.GetAssembly(typeof(VoteAppDbContext))!;

            services.AddScopedAllByTypeRule(infraAssembly,
                t => t.Name.EndsWith("Service") && !t.IsAbstract && !t.IsInterface,
                i => i.Name.EndsWith("Service"));
        }

        private static void AddPolly(IServiceCollection services, AppConfig appConfig)
        {
            services.AddResiliencePipeline(PollyMiddleware.PollyPipelineKey, builder =>
            {
                builder
                    .AddRetry(PollyMiddleware.PollyRetryStrategyOptionsFactory(appConfig))
                    .AddCircuitBreaker(new CircuitBreakerStrategyOptions());
            });
        }
    }
}
