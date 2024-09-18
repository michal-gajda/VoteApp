// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Boilerplate item", Scope = "member", Target = "~M:VoteApp.Api.StartupConfigs.DIConfig.AddHostedServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "Boilerplate item", Scope = "member", Target = "~M:VoteApp.Api.StartupConfigs.DIConfig.AddHostedServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "Boilerplate item", Scope = "member", Target = "~M:VoteApp.Api.StartupConfigs.DIConfig.AddDomainLayer(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Boilerplate item", Scope = "member", Target = "~M:VoteApp.Api.StartupConfigs.DIConfig.AddDomainLayer(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Needs to maintain consistency (ConfigureServices vs Configure in Startup)", Scope = "member", Target = "~M:VoteApp.Api.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment,Microsoft.Extensions.Logging.ILoggerFactory)")]
[assembly: SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "The Net Core app entry point", Scope = "type", Target = "~T:VoteApp.Api.Program")]
