// <copyright file="Program.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using TheBIADevCompany.BIADemo.Presentation.Api;
#pragma warning restore SA1200 // Using directives should be placed correctly

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// NLog: setup the logger first to catch all errors
try
{
    builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
        config.AddJsonFile("bianetconfig.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"bianetconfig.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
    }).ConfigureLogging((hostingContext, logging) =>
    {
        IConfiguration configuration = hostingContext.Configuration;
        LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
    })
.UseNLog();
}
catch (Exception ex)
{
    // NLog: catch setup errors
    var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit
    LogManager.Shutdown();
}

Startup startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

WebApplication app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();
