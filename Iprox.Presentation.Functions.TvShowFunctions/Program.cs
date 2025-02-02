using Iprox.Application.Common.Interface;
using Iprox.Application.TvShowFunc.Interfaces;
using Iprox.Application.TvShowFunc.Services;
using Iprox.Application.TvShowsApi.Interfaces;
using Iprox.Application.TvShowsApi.Services;
using Iprox.Domain.Core;
using Iprox.Domain.Interface;
using Iprox.Domain.Interface.IRepositories;
using Iprox.Infrastructure.ExternalServices;
using Iprox.Infrastructure.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//var host = new HostBuilder()
//    .ConfigureFunctionsWebApplication()
//    .ConfigureServices(services =>
//    {
//        services.AddApplicationInsightsTelemetryWorkerService();
//        services.ConfigureFunctionsApplicationInsights();
//    })
//    .Build();

//host.Run();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        // Configure Application Insights for telemetry
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Fetch connection string from configuration and configure DbContext
        string? connectionString = context.Configuration.GetConnectionString("ApplicationDbContext");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'ApplicationDbContext' is missing.");
        }

        // Register your DbContext with SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddHttpClient();

        // Add scoped services for your custom functionality
        services.AddScoped<IExternalServiceClient, ExternalServiceClient>();
        services.AddScoped<ISyncDataService, SyncDataService>();
        services.AddScoped<IShowCoreService, ShowCoreService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    })
    .Build();

host.Run();