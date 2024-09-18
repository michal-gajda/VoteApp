using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;
using VoteApp.Api;
using VoteApp.Application;
using VoteApp.Domain;
using VoteApp.Infrastructure;
using VoteApp.Infrastructure.Database;
using VoteApp.Infrastructure.Database.Repositories;

namespace VoteApp.Tests.Common
{
    public abstract class BaseTestContainer
    {
        protected readonly ICurrentUser _currentUser;
        protected readonly VoteAppDbContext _dbContext;
        protected readonly IMediator _mediator;
        protected readonly ServiceProvider _serviceProvider;

        protected BaseTestContainer()
        {
            _currentUser = this.MockCurrentUser(Guid.NewGuid().ToString());
            _dbContext = this.CreateInMemoryDbContext(this);

            var context = new DefaultHttpContext();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(hca => hca.HttpContext).Returns(context);

            var applicationAssembly = Assembly.GetAssembly(typeof(IHandlerContext))!;

            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddScoped(sp => _currentUser)
                .AddScoped(sp => mockHttpContextAccessor.Object)
                .AddMediatR(applicationAssembly)
                .AddSingleton(sp => this.CreateConfiguration())
                .AddSingleton<IAppConfig, AppConfig>()
                .AddScoped(sp => _dbContext)
                .AddScoped(typeof(IBaseRepository<>), typeof(Repository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddValidatorsFromAssembly(applicationAssembly)
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTransactionPipelineBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorsExecutionPipelineBehavior<,>))
                .AddSingleton<ApplicationContext>()
                .AddScoped<IHandlerContext, HandlerContext>()
                .BuildServiceProvider();
            _mediator = _serviceProvider.GetRequiredService<IMediator>();
        }
    }
}
