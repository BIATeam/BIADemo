// <copyright file="Program.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using System;
using BIA.Net.Core.Application.Authentication;
using BIA.Net.Core.Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using TheBIADevCompany.BIADemo.Presentation.Api;
#pragma warning restore SA1200 // Using directives should be placed correctly

// NLog: setup the logger first to catch all errors
try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
        config.AddJsonFile("bianetconfig.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"bianetconfig.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
    }).ConfigureLogging((hostingContext, logging) =>
    {
        IConfiguration configuration = hostingContext.Configuration;
        LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
    })
    .UseNLog();

    Startup startup = new(builder.Configuration);

    startup.ConfigureServices(builder.Services);

    WebApplication app = builder.Build();

    BiaNetSection biaNetSection = new();
    builder.Configuration.GetSection("BiaNet").Bind(biaNetSection);
    startup.Configure(app, app.Environment, new JwtFactory(Options.Create<Jwt>(biaNetSection.Jwt)));

    app.Run();
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